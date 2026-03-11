namespace Daco.Application.Auth.Commands
{
    public class UnlinkProviderCommandValidator : BaseValidator<UnlinkProviderCommand>
    {
        private static readonly string[] AllowedProviders = [ProviderTypes.Google, ProviderTypes.Facebook];

        public UnlinkProviderCommandValidator()
        {
            RuleFor(x => x.ProviderType)
                .NotEmpty().WithMessage("Provider type is required")
                .Must(p => AllowedProviders.Contains(p))
                .WithMessage($"Provider must be one of: {string.Join(", ", AllowedProviders)}");
        }
    }
}
