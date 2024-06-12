using AutoMapper;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Mappings
{
    public static class RoleMappingExtensions
    {
        public static IList<RoleDto> ToDtos(this IList<Role> roles, IMapper mapper)
        {
            IList<RoleDto> roleDtos = roles.Select(role => role.ToDto(mapper)).ToList();
            return roleDtos;
        }

        public static IList<Role> ToEntities(this IList<RoleDto> roleDtos, IMapper mapper)
        {
            IList<Role> roles = roleDtos.Select(roleDto => roleDto.ToEntity(mapper)).ToList();
            return roles;
        }

        public static RoleDto ToDto(this Role role, IMapper mapper)
        {
            RoleDto roleDto = mapper.Map<RoleDto>(role);
            return roleDto;
        }
        
        public static Role ToEntity(this RoleDto roleDto, IMapper mapper)
        {
            Role role = mapper.Map<Role>(roleDto);
            return role;
        }
    }
}