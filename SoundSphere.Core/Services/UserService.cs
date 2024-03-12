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

        public User Save(User user) => _userRepository.Save(user);

        public User UpdateById(User user, Guid id) => _userRepository.UpdateById(user, id);

        public User DisableById(Guid id) => _userRepository.DisableById(id);
    }
}