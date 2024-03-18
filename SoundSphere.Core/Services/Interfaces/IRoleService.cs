using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IRoleService
    {
        IList<RoleDto> FindAll();

        RoleDto FindById(Guid id);

        RoleDto Save(RoleDto roleDto);

        IList<RoleDto> ConvertToDtos(IList<Role> roles);

        IList<Role> ConvertToEntities(IList<RoleDto> roleDtos);

        RoleDto ConvertToDto(Role role);

        Role ConvertToEntity(RoleDto roleDto);
    }
}