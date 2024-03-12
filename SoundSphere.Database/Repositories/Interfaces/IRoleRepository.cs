using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        IList<Role> FindAll();

        Role FindById(Guid id);

        Role Save(Role role);
    }
}