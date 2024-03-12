using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IUserService
    {
        IList<User> FindAll();

        User FindById(Guid id);

        User Save(User user);

        User UpdateById(User user, Guid id);

        User DisableById(Guid id);
    }
}