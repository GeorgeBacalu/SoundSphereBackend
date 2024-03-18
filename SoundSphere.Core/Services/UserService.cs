using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthorityRepository _authorityRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IAuthorityRepository authorityRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _authorityRepository = authorityRepository;
        }

        public IList<UserDto> FindAll() => ConvertToDtos(_userRepository.FindAll());

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

        public UserDto ConvertToDto(User user) => new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Mobile = user.Mobile,
            Address = user.Address,
            Birthday = user.Birthday,
            Avatar = user.Avatar,
            RoleId = user.Role.Id,
            AuthoritiesIds = user.Authorities
                .Select(authority => authority.Id)
                .ToList(),
            IsActive = user.IsActive
        };

        public User ConvertToEntity(UserDto userDto) => new User
        {
            Id = userDto.Id,
            Name = userDto.Name,
            Email = userDto.Email,
            Mobile = userDto.Mobile,
            Address = userDto.Address,
            Birthday = userDto.Birthday,
            Avatar = userDto.Avatar,
            Role = _roleRepository.FindById(userDto.RoleId),
            Authorities = userDto.AuthoritiesIds
                .Select(_authorityRepository.FindById)
                .ToList(),
            IsActive = userDto.IsActive
        };
    }
}