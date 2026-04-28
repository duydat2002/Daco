namespace Daco.Application.Administration.CategoryManagement.Commands
{
    public class UpdateCategoryAttributeCommandValidator : BaseValidator<UpdateCategoryAttributeCommand>
    {
        public UpdateCategoryAttributeCommandValidator() 
        {
            RuleFor(x => x.AttributeId)
                .NotEmpty().WithMessage("AttributeId is required");

            RuleFor(x => x.AttributeName)
                .NotEmpty().WithMessage("Attribute name is required")
                .MaximumLength(255).WithMessage("Attribute name must not exceed 255 characters");

            RuleFor(x => x.AttributeSlug)
                .NotEmpty().WithMessage("Attribute slug is required")
                .MaximumLength(255).WithMessage("Attribute slug must not exceed 255 characters")
                .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
                .WithMessage("Slug can only contain lowercase letters, numbers and hyphens");

            RuleFor(x => x.InputType)
                .IsInEnum().WithMessage("Invalid input type");

            RuleFor(x => x.SortOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Sort order must be greater than or equal to 0");

            RuleFor(x => x.PredefinedValues)
                .Must((cmd, values) =>
                    values == null ||
                    cmd.InputType == AttributeInputType.Select ||
                    cmd.InputType == AttributeInputType.MultiSelect)
                .WithMessage("Predefined values only allowed for Select and MultiSelect input types")
                .When(x => x.PredefinedValues != null);

            RuleForEach(x => x.PredefinedValues)
                .NotEmpty().WithMessage("Predefined value cannot be empty")
                .MaximumLength(255).WithMessage("Predefined value must not exceed 255 characters")
                .When(x => x.PredefinedValues is { Count: > 0 });
        }
    }
}
