namespace Daco.Application.Shops.Commands.Products
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ResponseDTO>
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(
            ISellerRepository sellerRepository,
            IShopRepository shopRepository,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IBrandRepository brandRepository,
            IFileStorageService fileStorageService,
            IUnitOfWork unitOfWork,
            ILogger<CreateProductCommandHandler> logger)
        {
            _sellerRepository = sellerRepository;
            _shopRepository = shopRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("User {UserId} creating product '{ProductName}'",
                request.UserId, request.ProductName);

            var seller = await _sellerRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (seller is null || !seller.IsActive)
                return ResponseDTO.Failure(ErrorCodes.SellerErrors.NotFound, "Seller not found or not active");

            var shop = await _shopRepository.GetBySellerIdAsync(seller.Id, cancellationToken);
            if (shop is null)
                return ResponseDTO.Failure(ErrorCodes.ShopErrors.NotFound, "Shop not found");

            if (shop.Status != ShopStatus.Active)
                return ResponseDTO.Failure(ErrorCodes.ShopErrors.Suspended, "Shop is not active");

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
            if (category is null || !category.IsActive)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.NotFound, "Category not found");

            if (!category.IsLeaf)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.NotLeaf,
                    "Product must belong to a leaf category");

            if (request.BrandId.HasValue)
            {
                var brand = await _brandRepository.GetByIdAsync(request.BrandId.Value, cancellationToken);
                if (brand is null || !brand.IsActive)
                    return ResponseDTO.Failure(ErrorCodes.BrandErrors.NotFound, "Brand not found");
            }

            // Slug
            var slug = SlugGenerator.FromName(request.ProductName);
            var attempt = 0;

            while (await _productRepository.SlugExistsAsync(slug, null, cancellationToken))
                slug = SlugGenerator.WithSuffix(request.ProductName, ++attempt);

            // Meta
            var metaTitle = request.ProductName.Length > 255
                                    ? request.ProductName[..255]
                                    : request.ProductName;
            var metaDescription = request.Description is not null && request.Description.Length > 160
                                    ? request.Description[..160]
                                    : request.Description;
            var metaKeywords = request.ProductName;

            var product = Product.Create(
                shopId: shop.Id,
                categoryId: request.CategoryId,
                productName: request.ProductName,
                productSlug: slug,
                weight: request.Weight,
                description: request.Description,
                brandId: request.BrandId,
                basePrice: request.BasePrice,
                compareAtPrice: request.CompareAtPrice,
                stockQuantity: request.StockQuantity,
                sku: request.Sku,
                gtin: request.Gtin,
                isPreOrder: request.IsPreOrder,
                preOrderLeadTime: request.PreOrderLeadTime,
                length: request.Length,
                width: request.Width,
                height: request.Height,
                metaTitle: metaTitle,
                metaDescription: metaDescription,
                metaKeywords: metaKeywords);

            if (request.Images.Any())
            {
                var sortedImages = request.Images.OrderBy(i => i.SortOrder).ToList();

                foreach (var imageInput in sortedImages)
                {
                    try
                    {
                        var permanentUrl = await _fileStorageService.MoveProductMediaAsync(
                            "image",
                            imageInput.TempUrl,
                            shop.Id,
                            product.Id,
                            cancellationToken);

                        var image = ProductImage.Create(
                            productId: product.Id,
                            imageUrl: permanentUrl,
                            sortOrder: imageInput.SortOrder);

                        product.AddImage(image);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to move image {TempUrl} for product {ProductId}",
                            imageInput.TempUrl, product.Id);
                    }
                }
            }

            if (request.Video != null)
            {
                try
                {
                    var permanentVideoUrl = await _fileStorageService.MoveProductMediaAsync(
                        "video",
                        request.Video.TempUrl,
                        shop.Id,
                        product.Id,
                        cancellationToken);

                    var permanentThumbUrl = await _fileStorageService.MoveProductMediaAsync(
                        "video",
                        request.Video.TempThumbUrl,
                        shop.Id,
                        product.Id,
                        cancellationToken);

                    var video = ProductVideo.Create(
                        productId: product.Id,
                        videoUrl: permanentVideoUrl,
                        thumbnailUrl: permanentThumbUrl);

                    product.AddVideo(video);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to move video {TempUrl} for product {ProductId}",
                        request.Video.TempUrl, product.Id);
                }
            }

            await _productRepository.AddAsync(product, cancellationToken);

            _unitOfWork.TrackEntity(shop);

            _logger.LogInformation("Product {ProductId} created for shop {ShopId}",
                product.Id, shop.Id);

            return ResponseDTO.Success(new
            {
                product.Id,
                product.ShopId,
                product.ProductName,
                product.ProductSlug,
                product.CategoryId,
                product.BrandId,
                product.BasePrice,
                product.StockQuantity,
                Status = product.Status.ToString().ToLower(),
                Images = product.ProductImages.OrderBy(i => i.SortOrder).Select(i => new
                {
                    i.Id,
                    i.ImageUrl,
                    i.IsCover,
                    i.SortOrder
                }),
                product.CreatedAt
            }, "Product created successfully. Awaiting admin review.");
        }
    }
}
