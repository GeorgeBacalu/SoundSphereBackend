using SoundSphere.Database.Dtos.Common;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IRoleService
    {
        IList<RoleDto> GetAll();

        RoleDto GetById(Guid id);

        RoleDto Add(RoleDto roleDto);
    }
}