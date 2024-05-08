using AutoMapper;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Mappings
{
    public static class RoleMappingExtensions
    {
        public static IList<RoleDto> ToDtos(this IList<Role> roles, IMapper mapper) => roles.Select(role => role.ToDto(mapper)).ToList();

        public static IList<Role> ToEntities(this IList<RoleDto> roleDtos, IMapper mapper) => roleDtos.Select(roleDto => roleDto.ToEntity(mapper)).ToList();

        public static RoleDto ToDto(this Role role, IMapper mapper) => mapper.Map<RoleDto>(role);
        
        public static Role ToEntity(this RoleDto roleDto, IMapper mapper) => mapper.Map<Role>(roleDto);
    }
}