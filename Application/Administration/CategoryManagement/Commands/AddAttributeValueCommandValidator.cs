namespace Daco.Application.Administration.CategoryManagement.Commands
{
    public class AddAttributeValueCommandValidator : BaseValidator<AddAttributeValueCommand>
    {
        public AddAttributeValueCommandValidator() 
        {
            RuleFor(x => x.AttributeId)
                .NotEmpty().WithMessage("AttributeId is required");

            RuleFor(x => x.Values)
                .NotEmpty().WithMessage("At least one value is required");

            RuleForEach(x => x.Values)
                .NotEmpty().WithMessage("Value cannot be empty")
                .MaximumLength(255).WithMessage("Value must not exceed 255 characters");
        }
    }
}
