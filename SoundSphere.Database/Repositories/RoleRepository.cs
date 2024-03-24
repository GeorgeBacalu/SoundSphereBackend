using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;

namespace SoundSphere.Database.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly SoundSphereContext _context;

        public RoleRepository(SoundSphereContext context) => _context = context;

        public IList<Role> FindAll() => _context.Roles.ToList();

        public Role FindById(Guid id) => _context.Roles
            .FirstOrDefault(role => role.Id == id) 
            ?? throw new ResourceNotFoundException($"Role with id {id} not found!");

        public Role Save(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role;
        }
    }
}