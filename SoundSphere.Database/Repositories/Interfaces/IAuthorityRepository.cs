using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IAuthorityRepository
    {
        IList<Authority> GetAll();

        Authority GetById(Guid id);

        IList<Authority> GetByRole(Role role);

        Authority Add(Authority authority);
    }
}