namespace Daco.Application.Administration.CategoryManagement.Commands
{
    public class CreateCategoryCommandValidator : BaseValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.CategoryName)
                 .NotEmpty().WithMessage("Category name is required")
                 .MaximumLength(255).WithMessage("Category name must not exceed 255 characters");

            RuleFor(x => x.CategorySlug)
                .NotEmpty().WithMessage("Category slug is required")
                .MaximumLength(255).WithMessage("Category slug must not exceed 255 characters")
                .MustBeValidSlug();

            RuleFor(x => x.SortOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Sort order must be >= 0");

            RuleFor(x => x.IconUrl)
                .MustBeValidUrl()
                .When(x => x.IconUrl is not null);

            RuleFor(x => x.ImageUrl)
                .MustBeValidUrl()
                .When(x => x.ImageUrl is not null);
        }
    }
}
