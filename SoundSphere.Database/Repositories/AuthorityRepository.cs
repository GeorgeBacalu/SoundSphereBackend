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

        public IList<Authority> GetAll() => _context.Authorities.ToList();

        public Authority GetById(Guid id) => _context.Authorities
            .FirstOrDefault(authority => authority.Id == id)
            ?? throw new ResourceNotFoundException(string.Format(AuthorityNotFound, id));

        public IList<Authority> GetByRole(Role role) => role.Type switch
        {
            RoleType.Listener => _context.Authorities
                .Where(authority => authority.Type == AuthorityType.Read)
                .ToList(),
            RoleType.Moderator => _context.Authorities
                .Where(authority => authority.Type != AuthorityType.Delete)
                .ToList(),
            RoleType.Administrator => _context.Authorities.ToList(),
            _ => throw new ResourceNotFoundException(string.Format(RoleTypeNotFound, role.Type))
        };

        public Authority Add(Authority authority)
        {
            if (authority.Id == Guid.Empty) authority.Id = Guid.NewGuid();
            _context.Authorities.Add(authority);
            _context.SaveChanges();
            return authority;
        }
    }
}