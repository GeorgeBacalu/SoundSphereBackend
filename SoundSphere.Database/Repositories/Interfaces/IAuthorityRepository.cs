using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IAuthorityRepository
    {
        IList<Authority> GetAll();

        Authority GetById(Guid id);

        Authority Add(Authority authority);
    }
}