using Core.Packages.Application.Common.Services.FileUpload;
using MediatR;

namespace Core.Packages.Application.Features.Files.Commands.Delete
{
    public class FileDeleteCommand : IRequest<bool>
    {
        public string FileName { get; set; }
        public string ContainerName { get; set; }

        public FileDeleteCommand(string fileName, string containerName)
        {
            FileName = fileName;
            ContainerName = containerName;
        }
    }

    public class FileDeleteCommandHandler : IRequestHandler<FileDeleteCommand, bool>
    {
        private readonly IFileStorageService _fileStorageService;

        public FileDeleteCommandHandler(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public async Task<bool> Handle(FileDeleteCommand request, CancellationToken cancellationToken)
        {
            return await _fileStorageService.DeleteFileAsync(request.FileName, request.ContainerName,cancellationToken);
        }
    }

}
