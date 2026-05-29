namespace Daco.Application.Catalog.Categories.Commands
{
    public class DeleteCategoryCommandValidator : BaseValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(x => x.CategoryId)
                .NotEmpty();
        }
    }
}
