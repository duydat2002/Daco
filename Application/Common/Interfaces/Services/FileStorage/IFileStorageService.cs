namespace Daco.Application.Common.Interfaces.Services.FileStorage
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(
            string folder,
            string key,
            Stream fileStream,
            string contentType,
            CancellationToken cancellationToken = default);
        Task CopyAsync(string sourceKey, string destinationKey);
        Task DeleteAsync(string fileUrl, CancellationToken cancellationToken = default);
        Task DeleteFolderAsync(string folderPath, CancellationToken cancellationToken = default); // thêm
        Task<string> UploadAvatarAsync(Guid userId, Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
        Task<string> UploadShopLogoAsync(Guid shopId, Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
        Task<string> MoveProductMediaAsync(
            string type,
            string tempUrl,
            Guid shopId,
            Guid productId,
            CancellationToken cancellationToken = default);

        Task<string> MoveBrandImageAsync(
            string type,
            string tempUrl,
            Guid brandId,
            CancellationToken cancellationToken = default);
    }
}
