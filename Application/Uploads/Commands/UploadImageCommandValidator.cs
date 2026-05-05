namespace Daco.Application.Uploads.Commands
{
    public class UploadImageCommandValidator : BaseValidator<UploadImageCommand>
    {
        private static readonly string[] AllowedImageTypes = ["image/jpeg", "image/png", "image/webp"];
        private static readonly string[] AllowedTypes = ["product"];
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        public UploadImageCommandValidator()
        {
            RuleFor(x => x.Type)
               .Must(ct => AllowedTypes.Contains(ct))
               .WithMessage($"Type must be one of: {string.Join(", ", AllowedTypes)}");

            RuleFor(x => x.FileSize)
                .GreaterThan(0).WithMessage("File is empty")
                .LessThanOrEqualTo(MaxFileSize).WithMessage("File size must not exceed 5MB");

            RuleFor(x => x.ContentType)
                .Must(ct => AllowedImageTypes.Contains(ct))
                .WithMessage("Only JPEG, PNG, and WebP images are allowed");

            RuleFor(x => x.FileName)
                .NotEmpty();
        }
    }
}
