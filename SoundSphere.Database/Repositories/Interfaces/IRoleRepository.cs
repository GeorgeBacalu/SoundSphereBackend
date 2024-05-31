using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        IList<Role> GetAll();

        Role GetById(Guid id);

        Role Add(Role role);
    }
}