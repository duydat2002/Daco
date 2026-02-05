namespace Daco.Application.Common.Validators
{
    public static class UsernameValidator
    {
        public static IRuleBuilderOptions<T, string> ValidUsername<T>(
        this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters")
                .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers and underscores");
        }
    }
}
