namespace Daco.Application.Administration.CategoryManagement.Commands
{
    public class UpdateCategoryCommandValidator : BaseValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator() 
        {
            RuleFor(x => x.CategoryId)
                .NotEmpty();

            RuleFor(x => x.CategoryName)
                 .MaximumLength(255);

            RuleFor(x => x.CategorySlug)
                .MaximumLength(255)
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
