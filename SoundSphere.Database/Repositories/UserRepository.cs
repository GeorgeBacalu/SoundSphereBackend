using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SoundSphereContext _context;

        public UserRepository(SoundSphereContext context) => _context = context;

        public IList<User> FindAll() => _context.Users.ToList();

        public User FindById(Guid id) => _context.Users.Find(id) ?? throw new Exception($"User with id {id} not found!");

        public User Save(User user)
        {
            if (user == null) throw new Exception("Can't persist null user to DB!");
            if (user.Id == Guid.Empty) user.Id = Guid.NewGuid();
            user.IsActive = true;
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User UpdateById(User user, Guid id)
        {
            if (user == null) throw new Exception("Can't persist null user to DB!");
            User userToUpdate = FindById(id);
            _context.Entry(userToUpdate).CurrentValues.SetValues(user);
            _context.SaveChanges();
            return userToUpdate;
        }

        public User DisableById(Guid id)
        {
            User userToDisable = FindById(id);
            userToDisable.IsActive = false;
            _context.SaveChanges();
            return userToDisable;
        }
    }
}