using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) => _userRepository = userRepository;

        public IList<User> FindAll() => _userRepository.FindAll();

        public User FindById(Guid id) => _userRepository.FindById(id);

        public User Save(User user)
        {
            if (user == null) throw new Exception("Can't persist null user to DB!");
            if (user.Id == Guid.Empty) user.Id = Guid.NewGuid();
            user.IsActive = true;
            return _userRepository.Save(user);
        }

        public User UpdateById(User user, Guid id)
        {
            if (user == null) throw new Exception("Can't persist null user to DB!");
            return _userRepository.UpdateById(user, id);
        }

        public User DisableById(Guid id) => _userRepository.DisableById(id);
    }
}