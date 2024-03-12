using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class AuthorityRepository : IAuthorityRepository
    {
        private readonly SoundSphereContext _context;

        public AuthorityRepository(SoundSphereContext context) => _context = context;

        public IList<Authority> FindAll() => _context.Authorities.ToList();

        public Authority FindById(Guid id) => _context.Authorities.Find(id) ?? throw new Exception($"Authority with id {id} not found!");

        public Authority Save(Authority authority)
        {
            if (authority == null) throw new Exception("Can't persist null authority to DB!");
            if (authority.Id == Guid.Empty) authority.Id = Guid.NewGuid();
            _context.Authorities.Add(authority);
            _context.SaveChanges();
            return authority;
        }
    }
}