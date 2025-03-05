using MediatR;

namespace Core.Packages.Application.Features.Files.Queries.Download
{
    public class FileDownloadQuery : IRequest<Stream?>
    {
        public string FileName { get; set; }
        public string ContainerName { get; set; }

        public FileDownloadQuery(string fileName, string containerName)
        {
            FileName = fileName;
            ContainerName = containerName;
        }
    }

}
