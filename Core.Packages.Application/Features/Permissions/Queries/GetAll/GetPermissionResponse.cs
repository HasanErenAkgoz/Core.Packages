using AutoMapper;
using Core.Packages.Application.Common.Attributies;
using Core.Packages.Application.Common.AutoMapper;
namespace Core.Packages.Application.Features.Permission.Queries.GetAll
{
    public sealed class GetPermissionResponse : IMapFrom<Core.Packages.Domain.Entities.Permission>
    {

        public GetPermissionResponse() { }  // 🔥 Default Constructor ekledik

        [Translate]
        public string Name { get; set; }
        [Translate]
        public string Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Core.Packages.Domain.Entities.Permission, GetPermissionResponse>();
        }
    }
}
