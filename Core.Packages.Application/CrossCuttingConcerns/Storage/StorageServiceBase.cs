namespace Core.Packages.Application.CrossCuttingConcerns.Storage;

public abstract class StorageServiceBase : IStorageService
{
    protected readonly string _rootPath;

    protected StorageServiceBase(string rootPath)
    {
        _rootPath = rootPath;
    }

    public abstract Task<string> UploadAsync(string fileName, Stream fileStream);
    public abstract Task<bool> DeleteAsync(string fileName);
    public abstract Task<Stream> DownloadAsync(string fileName);
    public abstract bool Exists(string fileName);
    public abstract string GetFileUrl(string fileName);

    protected string NormalizePath(string fileName)
    {
        return fileName.Replace("\\", "/").TrimStart('/');
    }

    protected string GetFullPath(string fileName)
    {
        return Path.Combine(_rootPath, NormalizePath(fileName));
    }

    protected void EnsureDirectoryExists(string filePath)
    {
        string directory = Path.GetDirectoryName(filePath)!;
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
} 