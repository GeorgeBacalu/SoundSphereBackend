using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;

namespace SoundSphere.Database.Repositories
{
    public class AuthorityRepository : IAuthorityRepository
    {
        private readonly SoundSphereDbContext _context;

        public AuthorityRepository(SoundSphereDbContext context) => _context = context;

        public IList<Authority> GetAll()
        {
            IList<Authority> authorities = _context.Authorities
                .OrderBy(authority => authority.CreatedAt)
                .ToList();
            return authorities;
        }

        public Authority GetById(Guid id)
        {
            Authority? authority = _context.Authorities.FirstOrDefault(authority => authority.Id == id);
            if (authority == null)
                throw new ResourceNotFoundException(string.Format(AuthorityNotFound, id));
            return authority;
        }

        public IList<Authority> GetByRole(Role role)
        {
            IList<Authority> authorities = role.Type switch
            {
                RoleType.Listener => _context.Authorities.Where(authority => authority.Type == AuthorityType.Read).ToList(),
                RoleType.Moderator => _context.Authorities.Where(authority => authority.Type != AuthorityType.Delete).ToList(),
                RoleType.Admin => _context.Authorities.ToList(),
                _ => throw new ResourceNotFoundException(string.Format(RoleTypeNotFound, role.Type.ToString()))
            };
            return authorities;
        }

        public Authority Add(Authority authority)
        {
            if (authority.Id == Guid.Empty)
                authority.Id = Guid.NewGuid();
            authority.CreatedAt = DateTime.Now;
            _context.Authorities.Add(authority);
            _context.SaveChanges();
            return authority;
        }
    }
}