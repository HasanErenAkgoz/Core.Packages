using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Packages.Application.Features.Files.Commands.Upload
{
    public class FileUploadCommand : IRequest<string>
    {
        public IFormFile File { get; set; }
        public string ContainerName { get; set; }

    }

}

