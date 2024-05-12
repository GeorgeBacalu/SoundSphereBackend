using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper) => (_roleRepository, _mapper) = (roleRepository, mapper);

        public IList<RoleDto> FindAll() => _roleRepository.FindAll().ToDtos(_mapper);

        public RoleDto FindById(Guid id) => _roleRepository.FindById(id).ToDto(_mapper);

        public RoleDto Save(RoleDto roleDto)
        {
            Role role = roleDto.ToEntity(_mapper);
            if (role.Id == Guid.Empty) role.Id = Guid.NewGuid();
            return _roleRepository.Save(role).ToDto(_mapper);
        }
    }
}