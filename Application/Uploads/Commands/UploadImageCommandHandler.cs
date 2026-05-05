namespace Daco.Application.Uploads.Commands
{
    public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, ResponseDTO>
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<UploadImageCommandHandler> _logger;

        public UploadImageCommandHandler(IFileStorageService fileStorageService, ILogger<UploadImageCommandHandler> logger)
        {
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("User {UserId} updating image for type {Type}", request.UserId, request.Type);

            var folder = $"temps/{request.UserId}/{request.Type.ToLower()}";
            var extension = Path.GetExtension(request.FileName).ToLowerInvariant();
            var key = $"{Guid.NewGuid()}{extension}";

            var url = await _fileStorageService.UploadAsync(
                folder,
                key,
                request.FileStream,
                request.ContentType,
                cancellationToken);

            _logger.LogInformation("Image updated for user {UserId}", request.UserId);

            return ResponseDTO.Success(new { url }, "Image updated successfully");
        }
    }
}
