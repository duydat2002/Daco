namespace Daco.Application.Administration.CategoryManagement.Commands
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
