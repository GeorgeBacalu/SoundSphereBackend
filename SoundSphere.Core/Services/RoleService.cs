using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper) => (_roleRepository, _mapper) = (roleRepository, mapper);

        public IList<RoleDto> GetAll() => _roleRepository.GetAll().ToDtos(_mapper);

        public RoleDto GetById(Guid id) => _roleRepository.GetById(id).ToDto(_mapper);

        public RoleDto Add(RoleDto roleDto) => _roleRepository.Add(roleDto.ToEntity(_mapper)).ToDto(_mapper);
    }
}