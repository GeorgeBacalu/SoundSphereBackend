using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;

namespace SoundSphere.Database.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly SoundSphereDbContext _context;

        public RoleRepository(SoundSphereDbContext context) => _context = context;

        public IList<Role> FindAll() => _context.Roles.ToList();

        public Role FindById(Guid id) => _context.Roles
            .FirstOrDefault(role => role.Id == id)
            ?? throw new ResourceNotFoundException(string.Format(Constants.RoleNotFound, id));

        public Role Save(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role;
        }
    }
}