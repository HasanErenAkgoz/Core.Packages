using Microsoft.AspNetCore.Http;

namespace Core.Packages.Application.Common.Services.FileUpload
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string containerName, CancellationToken cancellationToken);
        Task<Stream?> GetFileAsync(string fileName, string containerName);
        Task<bool> DeleteFileAsync(string fileName, string containerName,CancellationToken cancellationToken);
    }
}
