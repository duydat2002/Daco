namespace Daco.Application.Administration.CategoryManagement.Commands
{
    public class CreateCategoryAttributeCommandValidator : BaseValidator<CreateCategoryAttributeCommand>
    {
        private static readonly AttributeInputType[] SelectTypes =
        [
            AttributeInputType.Select,
            AttributeInputType.MultiSelect
        ];
        public CreateCategoryAttributeCommandValidator()
        {
            RuleFor(x => x.AttributeName)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.AttributeSlug)
                .NotEmpty()
                .MaximumLength(255)
                .MustBeValidSlug();

            RuleFor(x => x.InputType)
                .IsInEnum().WithMessage("Invalid input type");

            RuleFor(x => x.SortOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Sort order must be >= 0");

            RuleFor(x => x.Unit)
                .MaximumLength(50)
                .When(x => x.Unit is not null);

            RuleFor(x => x.PredefinedValues)
                .NotEmpty().WithMessage("Predefined values are required for Select/MultiSelect types")
                .When(x => SelectTypes.Contains(x.InputType));

            RuleForEach(x => x.PredefinedValues)
                .NotEmpty().WithMessage("Predefined value cannot be empty")
                .MaximumLength(255)
                .When(x => x.PredefinedValues is not null);

            RuleFor(x => x.IsVariation)
                .Must((cmd, isVariation) =>
                    !isVariation || SelectTypes.Contains(cmd.InputType))
                .WithMessage("Only Select/MultiSelect attributes can be used as variations");
        }
    }
}
