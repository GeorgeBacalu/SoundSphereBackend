using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IAuthorityRepository
    {
        IList<Authority> FindAll();

        Authority FindById(Guid id);

        Authority Save(Authority authority);
    }
}