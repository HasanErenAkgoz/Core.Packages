using Core.Packages.Application.Common.Services.FileUpload;
using MediatR;

namespace Core.Packages.Application.Features.Files.Queries.Download
{
    public class FileDownloadQueryHandler : IRequestHandler<FileDownloadQuery, Stream?>
    {
        private readonly IFileStorageService _fileStorageService;

        public FileDownloadQueryHandler(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public async Task<Stream?> Handle(FileDownloadQuery request, CancellationToken cancellationToken)
        {
            return await _fileStorageService.GetFileAsync(request.FileName, request.ContainerName);
        }
    }

}
