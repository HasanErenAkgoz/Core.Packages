using Core.Packages.Application.CrossCuttingConcerns.Storage;
using Microsoft.Extensions.Configuration;

namespace Core.Packages.Infrastructure.Storage.Local;

public class LocalStorageService : StorageServiceBase
{
    private readonly string _baseUrl;

    public LocalStorageService(IConfiguration configuration) 
        : base(configuration["Storage:Local:RootPath"] ?? "wwwroot/uploads")
    {
        _baseUrl = configuration["Storage:Local:BaseUrl"] ?? "http://localhost:5000/uploads";
    }

    public override async Task<string> UploadAsync(string fileName, Stream fileStream)
    {
        string filePath = GetFullPath(fileName);
        EnsureDirectoryExists(filePath);

        using var fileWriter = new FileStream(filePath, FileMode.Create);
        await fileStream.CopyToAsync(fileWriter);

        return GetFileUrl(fileName);
    }

    public override async Task<bool> DeleteAsync(string fileName)
    {
        string filePath = GetFullPath(fileName);
        
        if (File.Exists(filePath))
        {
            await Task.Run(() => File.Delete(filePath));
            return true;
        }

        return false;
    }

    public override async Task<Stream> DownloadAsync(string fileName)
    {
        string filePath = GetFullPath(fileName);
        
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {fileName}");

        return await Task.FromResult(File.OpenRead(filePath));
    }

    public override bool Exists(string fileName)
    {
        string filePath = GetFullPath(fileName);
        return File.Exists(filePath);
    }

    public override string GetFileUrl(string fileName)
    {
        return $"{_baseUrl.TrimEnd('/')}/{NormalizePath(fileName)}";
    }
} 