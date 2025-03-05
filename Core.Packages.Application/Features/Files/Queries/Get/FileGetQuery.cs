using Core.Packages.Domain.Entities;
using MediatR;

namespace Core.Packages.Application.Features.Files.Queries.Get
{
    public class FileGetQuery : IRequest<UploadedFile?>
    {
        public int FileId { get; set; }

        public FileGetQuery(int fileId)
        {
            FileId = fileId;
        }
    }

}
