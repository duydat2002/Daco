using static System.Net.Mime.MediaTypeNames;

namespace Daco.Application.Shops.Commands.Products
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ResponseDTO>
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateProductCommandHandler> _logger;

        public UpdateProductCommandHandler(
            ISellerRepository sellerRepository,
            IShopRepository shopRepository,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IBrandRepository brandRepository,
            IFileStorageService fileStorageService,
            IUnitOfWork unitOfWork,
            ILogger<UpdateProductCommandHandler> logger)
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

        public async Task<ResponseDTO> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
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

            var product = await _productRepository.GetByIdWithMediasAsync(request.ProductId, cancellationToken);
            if (product is null || product.DeletedAt != null)
                return ResponseDTO.Failure(ErrorCodes.ProductErrors.NotFound, "Product not found");

            if (product.ShopId != shop.Id)
                return ResponseDTO.Failure(ErrorCodes.AuthErrors.Unauthorized, "You do not have permission to update this product");

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

            if (!request.Images.Any())
                return ResponseDTO.Failure(ErrorCodes.ProductErrors.MustLeastOneImage, "Product must have at least 1 image");

            if (!request.Images.Any(i => i.SortOrder == 0))
                return ResponseDTO.Failure(ErrorCodes.ProductErrors.MustHaveOrderZero, "Must have an image with sortOrder = 0");

            product.Update(
                shopId: shop.Id,
                categoryId: request.CategoryId,
                productName: request.ProductName,
                productSlug: request.ProductSlug,
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
                metaTitle: request.MetaTitle,
                metaDescription: request.MetaDescription,
                metaKeywords: request.MetaKeywords);

            await _productRepository.AddAsync(product, cancellationToken);

            #region Images
            var existingImages = product.ProductImages.ToList();

            var keepImageIds = request.Images
                .Where(i => i.Id.HasValue)
                .Select(i => i.Id!.Value)
                .ToHashSet();

            var imagesToRemove = existingImages
                .Where(i => !keepImageIds.Contains(i.Id))
                .ToList();

            foreach (var image in imagesToRemove)
            {
                try
                {
                    await _fileStorageService.DeleteAsync(image.ImageUrl, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete image {ImageUrl}", image.ImageUrl);
                }

                product.RemoveImage(image.Id);
            }

            foreach (var imageInput in request.Images.Where(i => i.Id.HasValue))
            {
                var existing = existingImages.FirstOrDefault(i => i.Id == imageInput.Id!.Value);
                if (existing is null) continue;

                if (existing.SortOrder != imageInput.SortOrder)
                    product.UpdateImageSortOrder(existing.Id, imageInput.SortOrder);
            }

            foreach (var imageInput in request.Images.Where(i => !i.Id.HasValue))
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
            #endregion

            #region Video
            var currentVideo = product.ProductVideos.FirstOrDefault();
            var existingVideo = product.ProductVideos.FirstOrDefault(x => x.VideoUrl == request.Video?.TempUrl);

            if (existingVideo != null && currentVideo != null)
            {
                try
                {
                    await _fileStorageService.DeleteAsync(currentVideo.VideoUrl, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete videos {VideoId}", currentVideo.Id);
                }

                product.RemoveVideo(currentVideo.Id);
            }

            if (!string.IsNullOrEmpty(request.Video?.TempUrl))
            {
                try
                {
                    var permanentVideoUrl = await _fileStorageService.MoveProductMediaAsync(
                        "image",
                        request.Video.TempUrl,
                        shop.Id,
                        product.Id,
                        cancellationToken);

                    var permanentThumbUrl = await _fileStorageService.MoveProductMediaAsync(
                        "image",
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
            #endregion

            _unitOfWork.TrackEntity(shop);

            _logger.LogInformation("Product {ProductId} updated by user {UserId}",
                product.Id, request.UserId);

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
            }, "Product updated successfully");
        }
    }
}
