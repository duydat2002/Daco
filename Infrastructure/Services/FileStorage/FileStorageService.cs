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

        public async Task CopyAsync(string sourceKey, string destinationKey)
        {
            var request = new CopyObjectRequest
            {
                SourceBucket = _settings.BucketName,
                SourceKey = sourceKey,
                DestinationBucket = _settings.BucketName,
                DestinationKey = destinationKey
            };

            await _s3Client.CopyObjectAsync(request);
        }

        public async Task DeleteFolderAsync(
            string folderPath,
            CancellationToken cancellationToken = default)
        {
            var prefix = folderPath.Trim('/') + "/";

            _logger.LogInformation("Deleting all files in folder: {Prefix}", prefix);

            var allKeys = new List<string>();
            string? continuationToken = null;

            do
            {
                var listRequest = new ListObjectsV2Request
                {
                    BucketName = _settings.BucketName,
                    Prefix = prefix,
                    ContinuationToken = continuationToken
                };

                var listResponse = await _s3Client.ListObjectsV2Async(listRequest, cancellationToken);

                allKeys.AddRange(listResponse.S3Objects.Select(o => o.Key));

                continuationToken = (bool)listResponse.IsTruncated
                    ? listResponse.NextContinuationToken
                    : null;

            } while (continuationToken != null);

            if (allKeys.Count == 0)
            {
                _logger.LogInformation("No files found in folder: {Prefix}", prefix);
                return;
            }

            _logger.LogInformation("Found {Count} files to delete in folder: {Prefix}",
                allKeys.Count, prefix);

            const int batchSize = 1000;

            foreach (var batch in allKeys.Chunk(batchSize))
            {
                var deleteRequest = new DeleteObjectsRequest
                {
                    BucketName = _settings.BucketName,
                    Objects = batch.Select(key => new KeyVersion { Key = key }).ToList()
                };

                var deleteResponse = await _s3Client.DeleteObjectsAsync(deleteRequest, cancellationToken);

                if (deleteResponse.DeleteErrors.Count > 0)
                {
                    foreach (var error in deleteResponse.DeleteErrors)
                    {
                        _logger.LogError(
                            "Failed to delete {Key}: [{Code}] {Message}",
                            error.Key, error.Code, error.Message);
                    }
                }

                _logger.LogInformation(
                    "Deleted batch of {Count} files from folder: {Prefix}",
                    batch.Length, prefix);
            }

            _logger.LogInformation(
                "Successfully deleted {Count} files from folder: {Prefix}",
                allKeys.Count, prefix);
        }

        public async Task<string> UploadAvatarAsync(
            Guid userId,
            Stream fileStream,
            string fileName,
            string contentType,
            CancellationToken cancellationToken = default)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            var key = $"{R2Folders.UserAvatar(userId)}/{Guid.NewGuid()}{extension}";

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
            Guid shopId, 
            Stream fileStream, 
            string fileName, 
            string contentType, 
            CancellationToken cancellationToken = default)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            var key = $"{R2Folders.ShopLogo(shopId)}/{Guid.NewGuid()}{extension}";

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

        public async Task<string> MoveProductMediaAsync(
            string type,
            string tempUrl,
            Guid shopId,
            Guid productId,
            CancellationToken cancellationToken = default)
        {
            var tempKey = tempUrl.Replace(_settings.PublicUrl.TrimEnd('/') + "/", "");

            var fileName = Path.GetFileName(tempKey);
            var folder = type == "image" ? R2Folders.ProductImages(shopId, productId) : R2Folders.ProductVideos(shopId, productId);
            var permanentKey = $"{folder}/{fileName}";

            _logger.LogInformation("Moving product image from {TempKey} to {PermanentKey}", tempKey, permanentKey);

            await CopyAsync(tempKey, permanentKey);

            try
            {
                await DeleteAsync(tempUrl, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to delete temp file {TempKey}", tempKey);
            }

            return $"{_settings.PublicUrl.TrimEnd('/')}/{permanentKey}";
        }

        public async Task<string> MoveBrandImageAsync(
            string type,
            string tempUrl,
            Guid brandId,
            CancellationToken cancellationToken = default)
        {
            var tempKey = tempUrl.Replace(_settings.PublicUrl.TrimEnd('/') + "/", "");

            var fileName = Path.GetFileName(tempKey);
            var folder = type == "logo" ? R2Folders.BrandLogo(brandId) : R2Folders.BrandSamples(brandId);
            var permanentKey = $"{folder}/{fileName}";

            _logger.LogInformation("Moving image from {TempKey} to {PermanentKey}", tempKey, permanentKey);

            await CopyAsync(tempKey, permanentKey);

            try
            {
                await DeleteAsync(tempUrl, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to delete temp file {TempKey}", tempKey);
            }

            return $"{_settings.PublicUrl.TrimEnd('/')}/{permanentKey}";
        }
    }
}
