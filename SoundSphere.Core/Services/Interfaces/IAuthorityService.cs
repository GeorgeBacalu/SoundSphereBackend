using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IAuthorityService
    {
        IList<Authority> FindAll();

        Authority FindById(Guid id);

        Authority Save(Authority authority);
    }
}