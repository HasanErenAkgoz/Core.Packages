using AutoMapper;
using Core.Packages.Application.Common.AutoMapper;
namespace Core.Packages.Application.Features.Permission.Queries.GetAll
{
    public sealed class GetPermissionResponse : IMapFrom<Core.Packages.Domain.Entities.Permission>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Core.Packages.Domain.Entities.Permission, GetPermissionResponse>();
        }
    }
}
