namespace Daco.Application.Common.Validators
{
    public static class UrlValidator
    {
        public static IRuleBuilderOptions<T, string?> MustBeValidUrl<T>(
            this IRuleBuilder<T, string?> ruleBuilder,
            string? message = null)
        {
            return ruleBuilder
                .Must(url =>
                    !string.IsNullOrWhiteSpace(url) &&
                    Uri.TryCreate(url, UriKind.Absolute, out var uri) &&
                    (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
                )
                .WithMessage(message ?? "Invalid URL");
        }
    }
}
