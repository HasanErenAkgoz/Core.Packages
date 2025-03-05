using Core.Packages.Domain.Comman;
using Core.Packages.Domain.Enums;

namespace Core.Packages.Domain.Entities
{
    public class UploadedFile : BaseEntity<int>
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public FileType FileType { get; set; }

    }
}
