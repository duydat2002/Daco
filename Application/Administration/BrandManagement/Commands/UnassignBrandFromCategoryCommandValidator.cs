namespace Daco.Application.Administration.BrandManagement.Commands
{
    public class UnassignBrandFromCategoryCommandValidator : BaseValidator<UnassignBrandFromCategoryCommand>
    {
        public UnassignBrandFromCategoryCommandValidator()
        {
            RuleFor(x => x.BrandId)
                .NotEmpty().WithMessage("BrandId is required");

            RuleFor(x => x.CategoryIds)
                .NotEmpty().WithMessage("At least one category is required");

            RuleForEach(x => x.CategoryIds)
                .NotEmpty().WithMessage("CategoryId cannot be empty");
        }
    }
}
