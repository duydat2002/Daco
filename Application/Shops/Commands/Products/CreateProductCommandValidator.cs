namespace Daco.Application.Shops.Commands.Products
{
    public class CreateProductCommandValidator : BaseValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator() 
        {
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required")
                .MaximumLength(500);

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category is required");

            RuleFor(x => x.BasePrice)
                .GreaterThan(0).WithMessage("Base price must be greater than 0")
                .When(x => x.BasePrice.HasValue);

            RuleFor(x => x.CompareAtPrice)
                .GreaterThan(0)
                .Must((cmd, compareAt) => compareAt > cmd.BasePrice)
                .WithMessage("Compare at price must be greater than base price")
                .When(x => x.CompareAtPrice.HasValue);

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Weight is required");

            RuleFor(x => x.Length)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Width)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Height)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.PreOrderLeadTime)
                .GreaterThan(0).WithMessage("Pre-order lead time is required when is pre-order")
                .When(x => x.IsPreOrder);

            RuleFor(x => x.Images)
                .NotNull()
                .Must(x => x.Count > 0)
                .WithMessage("Product must have at least 1 image")
                .Must(x => x.Count <= 9)
                .WithMessage("Max 9 images allowed");

            RuleForEach(x => x.Images).ChildRules(image =>
            {
                image.RuleFor(x => x.TempUrl)
                    .NotEmpty()
                    .MustBeValidUrl();

                image.RuleFor(x => x.SortOrder)
                    .GreaterThanOrEqualTo(0);
            });

            RuleFor(x => x.Images)
                .Must(images => images.Select(i => i.SortOrder).Distinct().Count() == images.Count)
                .WithMessage("SortOrder must be unique");

            RuleFor(x => x.Images)
                .Must(images =>
                {
                    var orders = images.Select(i => i.SortOrder).OrderBy(x => x).ToList();
                    return orders.SequenceEqual(Enumerable.Range(0, images.Count));
                })
                .WithMessage("SortOrder must be continuous from 0");

            RuleFor(x => x.Video)
                .Must(v =>
                {
                    if (v == null) return true;

                    return !string.IsNullOrEmpty(v.TempUrl)
                        && !string.IsNullOrEmpty(v.TempThumbUrl);
                })
                .WithMessage("Video must include both url and thumbnail");

            RuleFor(x => x.Video)
                .ChildRules(v =>
                {
                    v.RuleFor(x => x.TempUrl)
                        .NotEmpty()
                        .MustBeValidUrl();

                    v.RuleFor(x => x.TempThumbUrl)
                        .NotEmpty()
                        .MustBeValidUrl();

                    v.RuleFor(x => x)
                        .Must(x => x.TempUrl.Contains("/temp/")
                                && x.TempThumbUrl.Contains("/temp/"))
                        .WithMessage("Video must be uploaded to temp first");
                })
                .When(x => x.Video != null);
        }
    }
}
