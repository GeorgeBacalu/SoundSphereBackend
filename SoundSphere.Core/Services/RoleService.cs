using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository) => _roleRepository = roleRepository;

        public IList<Role> FindAll() => _roleRepository.FindAll();

        public Role FindById(Guid id) => _roleRepository.FindById(id);

        public Role Save(Role role)
        {
            if (role == null) throw new Exception($"Can't persist null role to DB!");
            if (role.Id == Guid.Empty) role.Id = Guid.NewGuid();
            return _roleRepository.Save(role);
        }
    }
}