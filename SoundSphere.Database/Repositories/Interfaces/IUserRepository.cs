using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IList<User> GetAll(UserPaginationRequest payload);

        User GetById(Guid id);

        User GetByEmail(string email);

        User Add(User user);

        User UpdateById(User user, Guid id);

        User DeleteById(Guid id);

        void LinkUserToRole(User user);

        void LinkUserToAuthorities(User user);

        void AddUserSong(User user);

        void AddUserArtist(User user);
    }
}