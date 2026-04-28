namespace Daco.Infrastructure.Services.FileStorage
{
    public class FileStorageService : IFileStorageService
    {
        private readonly AmazonS3Client _s3Client;
        private readonly CloudflareR2Settings _settings;
        private readonly ILogger<FileStorageService> _logger;

        public FileStorageService(
            IOptions<CloudflareR2Settings> settings, 
            ILogger<FileStorageService> logger)
        {
            _settings = settings.Value;
            _logger = logger;

            var credentials = new BasicAWSCredentials(
                _settings.AccessKeyId,
                _settings.SecretAccessKey);

            var config = new AmazonS3Config
            {
                ServiceURL = _settings.ServiceURL,
                ForcePathStyle = true
            };

            _s3Client = new AmazonS3Client(credentials, config);
        }

        public async Task<string> UploadAsync(
            string folder,
            string key,
            Stream fileStream,
            string contentType,
            CancellationToken cancellationToken = default)
        {
            var fullKey = $"{folder.Trim('/')}/{key.TrimStart('/')}";

            _logger.LogInformation("Uploading to R2: {Key}", fullKey);

            await _s3Client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = fullKey,
                InputStream = fileStream,
                ContentType = contentType,
                DisablePayloadSigning = true,
                Headers = { CacheControl = "public, max-age=2592000" }
            }, cancellationToken);

            var url = $"{_settings.PublicUrl.TrimEnd('/')}/{fullKey}";

            _logger.LogInformation("Upload successful: {Url}", url);

            return url;
        }

        public async Task DeleteAsync(string fileUrl, CancellationToken cancellationToken = default)
        {
            var key = fileUrl.Replace(_settings.PublicUrl.TrimEnd('/') + "/", "");

            _logger.LogInformation("Deleting file from R2: {Key}", key);

            var request = new DeleteObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = key
            };

            await _s3Client.DeleteObjectAsync(request, cancellationToken);
        }

        public async Task<string> UploadAvatarAsync(
            Guid userId,
            Stream fileStream,
            string fileName,
            string contentType,
            CancellationToken cancellationToken = default)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            var key = $"users/{userId}/avatars/{Guid.NewGuid()}{extension}";

            _logger.LogInformation("Uploading avatar to R2: {Key}", key);

            var request = new PutObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = key,
                InputStream = fileStream,
                ContentType = contentType,
                DisablePayloadSigning = true,
                Headers = { CacheControl = "public, max-age=2592000" }
            };

            await _s3Client.PutObjectAsync(request, cancellationToken);

            var url = $"{_settings.PublicUrl.TrimEnd('/')}/{key}";

            _logger.LogInformation("Avatar uploaded successfully: {Url}", url);

            return url;
        }

        public async Task<string> UploadShopLogoAsync(
            Guid userId, 
            Guid shopId, 
            Stream fileStream, 
            string fileName, 
            string contentType, 
            CancellationToken cancellationToken = default)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            var key = $"users/{userId}/shops/{shopId}/logos/{Guid.NewGuid()}{extension}";

            _logger.LogInformation("Uploading shop logo to R2: {Key}", key);

            var request = new PutObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = key,
                InputStream = fileStream,
                ContentType = contentType,
                DisablePayloadSigning = true,
                Headers = { CacheControl = "public, max-age=2592000" }
            };

            await _s3Client.PutObjectAsync(request, cancellationToken);

            var url = $"{_settings.PublicUrl.TrimEnd('/')}/{key}";

            _logger.LogInformation("Shop logo uploaded successfully: {Url}", url);

            return url;
        }
    }
}
