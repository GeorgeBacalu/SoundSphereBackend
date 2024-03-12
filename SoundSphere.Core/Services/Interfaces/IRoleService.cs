using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IRoleService
    {
        IList<Role> FindAll();

        Role FindById(Guid id);

        Role Save(Role role);
    }
}