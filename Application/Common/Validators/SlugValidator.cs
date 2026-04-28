namespace Daco.Application.Common.Validators
{
    public static class SlugValidator
    {
        public static IRuleBuilderOptions<T, string?> MustBeValidSlug<T>(
            this IRuleBuilder<T, string?> ruleBuilder,
            string? message = null)
        {
            return ruleBuilder
                .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
                .WithMessage(message ?? "Slug can only contain lowercase letters, numbers and hyphens");
        }
    }
}
