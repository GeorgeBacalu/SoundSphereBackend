using AutoMapper;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper) => (_roleRepository, _mapper) = (roleRepository, mapper);

        public IList<RoleDto> FindAll() => ConvertToDtos(_roleRepository.FindAll());

        public RoleDto FindById(Guid id) => ConvertToDto(_roleRepository.FindById(id));

        public RoleDto Save(RoleDto roleDto)
        {
            Role role = ConvertToEntity(roleDto);
            if (role.Id == Guid.Empty) role.Id = Guid.NewGuid();
            return ConvertToDto(_roleRepository.Save(role));
        }

        public IList<RoleDto> ConvertToDtos(IList<Role> roles) => roles.Select(ConvertToDto).ToList();

        public IList<Role> ConvertToEntities(IList<RoleDto> roleDtos) => roleDtos.Select(ConvertToEntity).ToList();

        public RoleDto ConvertToDto(Role role) => _mapper.Map<RoleDto>(role);

        public Role ConvertToEntity(RoleDto roleDto) => _mapper.Map<Role>(roleDto);
    }
}