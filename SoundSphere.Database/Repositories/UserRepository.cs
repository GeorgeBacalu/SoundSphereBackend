using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Extensions;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;

namespace SoundSphere.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SoundSphereDbContext _context;

        public UserRepository(SoundSphereDbContext context) => _context = context;

        public IList<User> FindAll() => _context.Users
            .Include(user => user.Role)
            .Include(user => user.Authorities)
            .ToList();

        public IList<User> FindAllActive() => _context.Users
            .Include(user => user.Role)
            .Include(user => user.Authorities)
            .Where(user => user.IsActive)
            .ToList();

        public IList<User> FindAllPagination(UserPaginationRequest payload) => _context.Users
            .Include(user => user.Role)
            .Include(user => user.Authorities)
            .Filter(payload)
            .Sort(payload)
            .Paginate(payload)
            .ToList();

        public IList<User> FindAllActivePagination(UserPaginationRequest payload) => _context.Users
            .Include(user => user.Role)
            .Include(user => user.Authorities)
            .Where(user => user.IsActive)
            .Filter(payload)
            .Sort(payload)
            .Paginate(payload)
            .ToList();

        public User FindById(Guid id) => _context.Users
            .Include(user => user.Role)
            .Include(user => user.Authorities)
            .Where(user => user.IsActive)
            .FirstOrDefault(user => user.Id == id)
            ?? throw new ResourceNotFoundException(string.Format(Constants.UserNotFound, id));

        public User Save(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User UpdateById(User user, Guid id)
        {
            User userToUpdate = FindById(id);
            userToUpdate.Name = user.Name;
            userToUpdate.Email = user.Email;
            userToUpdate.Password = user.Password;
            userToUpdate.Mobile = user.Mobile;
            userToUpdate.Address = user.Address;
            userToUpdate.Birthday = user.Birthday;
            userToUpdate.Avatar = user.Avatar;
            userToUpdate.Role = user.Role;
            userToUpdate.Authorities = user.Authorities;
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

        public void LinkUserToAuthorities(User user) => user.Authorities = user.Authorities
            .Select(authority => _context.Authorities.Find(authority.Id))
            .Where(authority => authority != null)
            .Select(authority => { _context.Entry(authority).State = EntityState.Unchanged; return authority; })
            .ToList();

        public void AddUserSong(User user) => _context.AddRange(_context.Songs
            .Select(song => new UserSong { User = user, Song = song, PlayCount = 0 })
            .ToList());

        public void AddUserArtist(User user) => _context.AddRange(_context.Artists
            .Select(artist => new UserArtist { User = user, Artist = artist, IsFollowing = false })
            .ToList());
    }
}