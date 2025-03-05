using Core.Packages.Application.Common.Services.FileUpload;
using MediatR;

namespace Core.Packages.Application.Features.Files.Commands.Upload
{
    public class FileUploadCommandHandler : IRequestHandler<FileUploadCommand, string>
    {
        private readonly IFileStorageService _fileStorageService;

        public FileUploadCommandHandler(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public async Task<string> Handle(FileUploadCommand request, CancellationToken cancellationToken)
        {
            return await _fileStorageService.UploadFileAsync(request.File, request.ContainerName,cancellationToken);
        }
    }

}

