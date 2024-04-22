using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;

namespace SoundSphere.Database.Repositories
{
    public class AuthorityRepository : IAuthorityRepository
    {
        private readonly SoundSphereDbContext _context;

        public AuthorityRepository(SoundSphereDbContext context) => _context = context;

        public IList<Authority> FindAll() => _context.Authorities.ToList();

        public Authority FindById(Guid id) => _context.Authorities
            .FirstOrDefault(authority => authority.Id == id)
            ?? throw new ResourceNotFoundException(string.Format(Constants.AuthorityNotFound, id));

        public Authority Save(Authority authority)
        {
            _context.Authorities.Add(authority);
            _context.SaveChanges();
            return authority;
        }
    }
}