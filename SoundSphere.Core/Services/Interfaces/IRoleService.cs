using SoundSphere.Database.Dtos.Common;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IRoleService
    {
        IList<RoleDto> FindAll();

        RoleDto FindById(Guid id);

        RoleDto Save(RoleDto roleDto);
    }
}