namespace Daco.Application.Shops.Commands.Products
{
    public class CreateProductCommandValidator : BaseValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator() 
        {
            RuleFor(x => x.ProductName)
                    .NotEmpty().WithMessage("Product name is required")
                    .MaximumLength(500);

            RuleFor(x => x.ProductSlug)
                .NotEmpty().WithMessage("Product slug is required")
                .MaximumLength(500)
                .MustBeValidSlug();

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

            RuleFor(x => x.MetaTitle)
                .MaximumLength(255)
                .When(x => x.MetaTitle is not null);

            RuleFor(x => x.MetaDescription)
                .MaximumLength(500)
                .When(x => x.MetaDescription is not null);

            RuleFor(x => x.Images)
                .NotNull()
                .Must(x => x.Count > 0)
                .WithMessage("Product must have at least 1 image")
                .Must(x => x.Count <= 9)
                .WithMessage("Max 9 images allowed");
        }
    }
}
