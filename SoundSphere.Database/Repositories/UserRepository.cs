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

        public IList<User> GetAll(UserPaginationRequest payload) => _context.Users
            .Include(user => user.Role)
            .Include(user => user.Authorities)
            .Where(user => user.DeletedAt == null)
            .Filter(payload)
            .Sort(payload)
            .Paginate(payload)
            .ToList();

        public User GetById(Guid id) => _context.Users
            .Include(user => user.Role)
            .Include(user => user.Authorities)
            .Where(user => user.DeletedAt == null)
            .FirstOrDefault(user => user.Id == id)
            ?? throw new ResourceNotFoundException(string.Format(UserNotFound, id));

        public User GetByEmail(string email) => _context.Users
            .Include(user => user.Role)
            .Include(user => user.Authorities)
            .Where(user => user.DeletedAt == null)
            .FirstOrDefault(user => user.Email.Equals(email))
            ?? throw new ResourceNotFoundException(string.Format(UserEmailNotFound, email));

        public User Add(User user)
        {
            if (user.Id == Guid.Empty) user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.Now;
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User UpdateById(User user, Guid id)
        {
            User userToUpdate = GetById(id);
            userToUpdate.Name = user.Name;
            userToUpdate.Email = user.Email;
            userToUpdate.PasswordHash = user.PasswordHash;
            userToUpdate.Mobile = user.Mobile;
            userToUpdate.Address = user.Address;
            userToUpdate.Birthday = user.Birthday;
            userToUpdate.Avatar = user.Avatar;
            userToUpdate.Role = user.Role;
            userToUpdate.Authorities = user.Authorities;
            if (_context.Entry(userToUpdate).State == EntityState.Modified)
                userToUpdate.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return userToUpdate;
        }

        public User DeleteById(Guid id)
        {
            User userToDelete = GetById(id);
            userToDelete.DeletedAt = DateTime.Now;
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