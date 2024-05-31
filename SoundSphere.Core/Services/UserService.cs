using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthorityRepository _authorityRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IAuthorityRepository authorityRepository, IMapper mapper) => 
            (_userRepository, _roleRepository, _authorityRepository, _mapper) = (userRepository, roleRepository, authorityRepository, mapper);

        public IList<UserDto> GetAll() => _userRepository.GetAll().ToDtos(_mapper);

        public IList<UserDto> GetAllActive() => _userRepository.GetAllActive().ToDtos(_mapper);

        public IList<UserDto> GetAllPagination(UserPaginationRequest payload) => _userRepository.GetAllPagination(payload).ToDtos(_mapper);

        public IList<UserDto> GetAllActivePagination(UserPaginationRequest payload) => _userRepository.GetAllActivePagination(payload).ToDtos(_mapper);

        public UserDto GetById(Guid id) => _userRepository.GetById(id).ToDto(_mapper);

        public UserDto Add(UserDto userDto)
        {
            User user = userDto.ToEntity(_roleRepository, _authorityRepository, _mapper);
            _userRepository.LinkUserToRole(user);
            _userRepository.LinkUserToAuthorities(user);
            _userRepository.AddUserSong(user);
            _userRepository.AddUserArtist(user);
            return _userRepository.Add(user).ToDto(_mapper);
        }

        public UserDto UpdateById(UserDto userDto, Guid id) => _userRepository.UpdateById(userDto.ToEntity(_roleRepository, _authorityRepository, _mapper), id).ToDto(_mapper);

        public UserDto DeleteById(Guid id) => _userRepository.DeleteById(id).ToDto(_mapper);
    }
}