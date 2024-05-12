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

        public IList<UserDto> FindAll() => _userRepository.FindAll().ToDtos(_mapper);

        public IList<UserDto> FindAllActive() => _userRepository.FindAllActive().ToDtos(_mapper);

        public IList<UserDto> FindAllPagination(UserPaginationRequest payload) => _userRepository.FindAllPagination(payload).ToDtos(_mapper);

        public IList<UserDto> FindAllActivePagination(UserPaginationRequest payload) => _userRepository.FindAllActivePagination(payload).ToDtos(_mapper);

        public UserDto FindById(Guid id) => _userRepository.FindById(id).ToDto(_mapper);

        public UserDto Save(UserDto userDto)
        {
            User user = userDto.ToEntity(_roleRepository, _authorityRepository, _mapper);
            if (user.Id == Guid.Empty) user.Id = Guid.NewGuid();
            user.IsActive = true;
            user.Password = "password";
            _userRepository.LinkUserToRole(user);
            _userRepository.LinkUserToAuthorities(user);
            _userRepository.AddUserSong(user);
            _userRepository.AddUserArtist(user);
            return _userRepository.Save(user).ToDto(_mapper);
        }

        public UserDto UpdateById(UserDto userDto, Guid id) => _userRepository.UpdateById(userDto.ToEntity(_roleRepository, _authorityRepository, _mapper), id).ToDto(_mapper);

        public UserDto DisableById(Guid id) => _userRepository.DisableById(id).ToDto(_mapper);
    }
}