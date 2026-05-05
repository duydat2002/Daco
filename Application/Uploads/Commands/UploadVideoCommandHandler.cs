namespace Daco.Application.Uploads.Commands
{
    public class UploadVideoCommandHandler : IRequestHandler<UploadVideoCommand, ResponseDTO>
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<UploadVideoCommandHandler> _logger;

        public UploadVideoCommandHandler(IFileStorageService fileStorageService, ILogger<UploadVideoCommandHandler> logger)
        {
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UploadVideoCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("User {UserId} updating video for type {Type}", request.UserId, request.Type);

            var folder = $"temps/{request.UserId}/{request.Type.ToLower()}";
            var extension = Path.GetExtension(request.FileName).ToLowerInvariant();
            var key = $"{Guid.NewGuid()}{extension}";

            var url = await _fileStorageService.UploadAsync(
                folder,
                key,
                request.FileStream,
                request.ContentType,
                cancellationToken);

            _logger.LogInformation("Video updated for user {UserId}", request.UserId);

            return ResponseDTO.Success(new { url }, "Video updated successfully");
        }
    }
}
