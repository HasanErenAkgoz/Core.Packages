namespace Core.Packages.Application.CrossCuttingConcerns.Storage;

public interface IStorageService
{
    Task<string> UploadAsync(string fileName, Stream fileStream);
    Task<bool> DeleteAsync(string fileName);
    Task<Stream> DownloadAsync(string fileName);
    bool Exists(string fileName);
    string GetFileUrl(string fileName);
} 