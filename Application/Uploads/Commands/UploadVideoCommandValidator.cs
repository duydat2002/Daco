namespace Daco.Application.Uploads.Commands
{
    public class UploadVideoCommandValidator : BaseValidator<UploadVideoCommand>
    {
        private static readonly string[] AllowedImageTypes = ["video/mp4"];
        private static readonly string[] AllowedTypes = ["product"];
        private const long MaxFileSize = 30 * 1024 * 1024; // 30MB

        public UploadVideoCommandValidator()
        {
            RuleFor(x => x.Type)
               .Must(ct => AllowedTypes.Contains(ct))
               .WithMessage($"Type must be one of: {string.Join(", ", AllowedTypes)}");

            RuleFor(x => x.FileSize)
                .GreaterThan(0).WithMessage("File is empty")
                .LessThanOrEqualTo(MaxFileSize).WithMessage("File size must not exceed 30MB");

            RuleFor(x => x.ContentType)
                .Must(ct => AllowedImageTypes.Contains(ct))
                .WithMessage("Only MP4 video are allowed");

            RuleFor(x => x.FileName)
                .NotEmpty();
        }
    }
}
