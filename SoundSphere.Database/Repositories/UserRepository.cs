using Microsoft.EntityFrameworkCore;
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
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User UpdateById(User user, Guid id)
        {
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


        public void LinkUserToRole(User user)
        {
            Role existingRole = _context.Roles.Find(user.Role.Id);
            if (existingRole != null)
            {
                _context.Entry(existingRole).State = EntityState.Unchanged;
                user.Role = existingRole;
            }
        }

        public void LinkUserToAuthorities(User user)
        {
            user.Authorities = user.Authorities
                            .Select(authority => _context.Authorities.Find(authority.Id))
                            .Where(authority => authority != null)
                            .ToList();

            foreach (Authority authority in user.Authorities)
            {
                _context.Entry(authority).State = EntityState.Unchanged;
            }
        }

        public void AddUserSong(User user)
        {
            _context.AddRange(_context.Songs
                            .Select(song => new UserSong
                            {
                                User = user,
                                Song = song,
                                PlayCount = 0
                            })
                            .ToList());
        }

        public void AddUserArtist(User user)
        {
            _context.AddRange(_context.Artists
                            .Select(artist => new UserArtist
                            {
                                User = user,
                                Artist = artist,
                                IsFollowing = false
                            })
                            .ToList());
        }
    }
}