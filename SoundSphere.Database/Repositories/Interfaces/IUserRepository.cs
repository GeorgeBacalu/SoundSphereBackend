using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IList<User> FindAll();

        User FindById(Guid id);

        User Save(User user);

        User UpdateById(User user, Guid id);

        User DisableById(Guid id);

        void LinkUserToRole(User user);

        void LinkUserToAuthorities(User user);

        void AddUserSong(User user);

        void AddUserArtist(User user);
    }
}