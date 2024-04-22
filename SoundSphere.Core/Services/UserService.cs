using AutoMapper;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;
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

        public IList<UserDto> FindAll() => ConvertToDtos(_userRepository.FindAll());

        public IList<UserDto> FindAllActive() => ConvertToDtos(_userRepository.FindAllActive());

        public UserDto FindById(Guid id) => ConvertToDto(_userRepository.FindById(id));

        public UserDto Save(UserDto userDto)
        {
            User user = ConvertToEntity(userDto);
            if (user.Id == Guid.Empty) user.Id = Guid.NewGuid();
            user.IsActive = true;
            user.Password = "password";
            _userRepository.LinkUserToRole(user);
            _userRepository.LinkUserToAuthorities(user);
            _userRepository.AddUserSong(user);
            _userRepository.AddUserArtist(user);
            return ConvertToDto(_userRepository.Save(user));
        }

        public UserDto UpdateById(UserDto userDto, Guid id) => ConvertToDto(_userRepository.UpdateById(ConvertToEntity(userDto), id));

        public UserDto DisableById(Guid id) => ConvertToDto(_userRepository.DisableById(id));

        public IList<UserDto> ConvertToDtos(IList<User> users) => users.Select(ConvertToDto).ToList();

        public IList<User> ConvertToEntities(IList<UserDto> userDtos) => userDtos.Select(ConvertToEntity).ToList();

        public UserDto ConvertToDto(User user)
        {
            UserDto userDto = _mapper.Map<UserDto>(user);
            userDto.AuthoritiesIds = user.Authorities
                .Select(authority => authority.Id)
                .ToList();
            return userDto;
        }

        public User ConvertToEntity(UserDto userDto)
        {
            User user = _mapper.Map<User>(userDto);
            user.Role = _roleRepository.FindById(userDto.RoleId);
            user.Authorities = userDto.AuthoritiesIds
                .Select(_authorityRepository.FindById)
                .ToList();
            return user;
        }
    }
}