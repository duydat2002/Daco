namespace Daco.Application.Uploads.Commands
{
    public class UploadImageCommandValidator : BaseValidator<UploadImageCommand>
    {
        private static readonly string[] AllowedImageTypes = ["image/jpeg", "image/png", "image/webp"];
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        public UploadImageCommandValidator(IOptions<UploadSettings> options)
        {
            var allowedTypes = options.Value.AllowedTypes;

            RuleFor(x => x.Type)
               .Must(ct => allowedTypes.Contains(ct))
               .WithMessage($"Type must be one of: {string.Join(", ", allowedTypes)}");

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
