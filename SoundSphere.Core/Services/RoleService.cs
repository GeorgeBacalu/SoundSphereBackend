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

        public IList<RoleDto> GetAll()
        {
            IList<RoleDto> roleDtos = _roleRepository.GetAll().ToDtos(_mapper);
            return roleDtos;
        }

        public RoleDto GetById(Guid id)
        {
            RoleDto roleDto = _roleRepository.GetById(id).ToDto(_mapper);
            return roleDto;
        }

        public RoleDto Add(RoleDto roleDto)
        {
            Role roleToCreate = roleDto.ToEntity(_mapper);
            RoleDto createdRoleDto = _roleRepository.Add(roleToCreate).ToDto(_mapper);
            return createdRoleDto;
        }
    }
}