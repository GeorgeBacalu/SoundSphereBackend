using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Extensions;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;

namespace SoundSphere.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SoundSphereDbContext _context;

        public UserRepository(SoundSphereDbContext context) => _context = context;

        public IList<User> GetAll() => _context.Users
            .Include(user => user.Role)
            .Include(user => user.Authorities)
            .ToList();

        public IList<User> GetAllActive() => _context.Users
            .Include(user => user.Role)
            .Include(user => user.Authorities)
            .Where(user => user.IsActive)
            .ToList();

        public IList<User> GetAllPagination(UserPaginationRequest payload) => _context.Users
            .Include(user => user.Role)
            .Include(user => user.Authorities)
            .Filter(payload)
            .Sort(payload)
            .Paginate(payload)
            .ToList();

        public IList<User> GetAllActivePagination(UserPaginationRequest payload) => _context.Users
            .Include(user => user.Role)
            .Include(user => user.Authorities)
            .Where(user => user.IsActive)
            .Filter(payload)
            .Sort(payload)
            .Paginate(payload)
            .ToList();

        public User GetById(Guid id) => _context.Users
            .Include(user => user.Role)
            .Include(user => user.Authorities)
            .Where(user => user.IsActive)
            .FirstOrDefault(user => user.Id == id)
            ?? throw new ResourceNotFoundException(string.Format(UserNotFound, id));

        public User Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User UpdateById(User user, Guid id)
        {
            User userToUpdate = GetById(id);
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

        public User DeleteById(Guid id)
        {
            User userToDelete = GetById(id);
            userToDelete.IsActive = false;
            _context.SaveChanges();
            return userToDelete;
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