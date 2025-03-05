using Azure.Storage.Blobs;
using Core.Packages.Application.Common.Services.FileUpload;
using Core.Packages.Domain.Entities;
using Core.Packages.Domain.Enums;
using Core.Packages.Domain.Repositories.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Core.Packages.Infrastructure.Services.FileUpload
{
    public class AzureBlobStorageService : IFileStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IUploadedFileRepository _fileRepository;

        public AzureBlobStorageService(string connectionString, IUploadedFileRepository fileRepository)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
            _fileRepository = fileRepository;
        }

        public async Task<bool> DeleteFileAsync(string fileName, string containerName, CancellationToken cancellationToken)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            var response = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
            return response.Value;
        }

        public async Task<Stream?> GetFileAsync(string fileName, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            if (await blobClient.ExistsAsync())
            {
                var downloadInfo = await blobClient.DownloadAsync();
                return downloadInfo.Value.Content;
            }

            return null;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string containerName, CancellationToken cancellationToken)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(file.FileName);
            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, true);

            var uploadedFile = new UploadedFile
            {
                FileName = file.FileName,
                FilePath = blobClient.Uri.ToString(),
                FileType = FileType.Other
            };

            await _fileRepository.AddAsync(uploadedFile, cancellationToken);
            return blobClient.Uri.ToString();
        }
    }

}
