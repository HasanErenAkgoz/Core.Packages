using Core.Packages.Application.Common.Services.FileUpload;
using Core.Packages.Domain.Entities;
using Core.Packages.Domain.Enums;
using Core.Packages.Domain.Repositories.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Core.Packages.Infrastructure.Services.FileUpload
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _rootPath;
        private readonly IUploadedFileRepository _fileRepository;

        public LocalFileStorageService(IWebHostEnvironment env, IUploadedFileRepository fileRepository)
        {
            // wwwroot/uploads dizinini ayarla
            _rootPath = Path.Combine(env.WebRootPath, "uploads");

            // Eğer uploads klasörü yoksa oluştur
            if (!Directory.Exists(_rootPath))
                Directory.CreateDirectory(_rootPath);

            _fileRepository = fileRepository;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string containerName, CancellationToken cancellationToken)
        {
            string folderPath = Path.Combine(_rootPath, containerName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, file.FileName);

            // Dosyayı kaydet
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);

            string fileUrl = $"/uploads/{containerName}/{file.FileName}";

            var uploadedFile = new UploadedFile
            {
                FileName = file.FileName,
                FilePath = fileUrl,
                FileType = GetFileType(file.FileName),
            };

            await _fileRepository.AddAsync(uploadedFile, cancellationToken);
            return fileUrl;
        }
        public async Task<bool> DeleteFileAsync(string fileName, string containerName, CancellationToken cancellationToken)
        {
            string filePath = Path.Combine(_rootPath, containerName, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

        public async Task<Stream?> GetFileAsync(string fileName, string containerName)
        {
            string filePath = Path.Combine(_rootPath, containerName, fileName);
            if (!File.Exists(filePath))
                return null;

            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }

        private FileType GetFileType(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            return extension switch
            {
                ".jpg" or ".png" or ".jpeg" or ".gif" => FileType.Image,
                ".mp4" or ".avi" or ".mov" => FileType.Video,
                ".mp3" or ".wav" => FileType.Audio,
                ".pdf" or ".docx" or ".xlsx" => FileType.Document,
                _ => FileType.Other
            };
        }
    }
}
