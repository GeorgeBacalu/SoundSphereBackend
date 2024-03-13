using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly SoundSphereContext _context;

        public RoleRepository(SoundSphereContext context) => _context = context;

        public IList<Role> FindAll() => _context.Roles.ToList();

        public Role FindById(Guid id) => _context.Roles.Find(id) ?? throw new Exception($"Role with id {id} not found!");

        public Role Save(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role;
        }
    }
}