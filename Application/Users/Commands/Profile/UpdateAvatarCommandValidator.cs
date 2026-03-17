namespace Daco.Application.Users.Commands.Profile
{
    public class UpdateAvatarCommandValidator : BaseValidator<UpdateAvatarCommand>
    {
        private static readonly string[] AllowedTypes = ["image/jpeg", "image/png", "image/webp"];
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        public UpdateAvatarCommandValidator()
        {
            RuleFor(x => x.FileSize)
                .GreaterThan(0).WithMessage("File is empty")
                .LessThanOrEqualTo(MaxFileSize).WithMessage("File size must not exceed 5MB");

            RuleFor(x => x.ContentType)
                .Must(ct => AllowedTypes.Contains(ct))
                .WithMessage("Only JPEG, PNG, and WebP images are allowed");

            RuleFor(x => x.FileName)
                .NotEmpty();
        }
    }
}
