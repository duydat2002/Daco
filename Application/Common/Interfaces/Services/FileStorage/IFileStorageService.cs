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
        Task<string> UploadAvatarAsync(Guid userId, Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
        Task DeleteAsync(string fileUrl, CancellationToken cancellationToken = default);
    }
}
