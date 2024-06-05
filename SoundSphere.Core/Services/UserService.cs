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
        private readonly SecurityService _securityService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IAuthorityRepository authorityRepository, SecurityService securityService, IMapper mapper) => 
            (_userRepository, _roleRepository, _authorityRepository, _securityService, _mapper) = (userRepository, roleRepository, authorityRepository, securityService, mapper);

        public IList<UserDto> GetAll(UserPaginationRequest payload) => _userRepository.GetAll(payload).ToDtos(_mapper);

        public UserDto GetById(Guid id) => _userRepository.GetById(id).ToDto(_mapper);

        public UserDto? Register(RegisterRequest payload)
        {
            if (payload == null) return null;
            byte[] salt = _securityService.GenerateSalt();
            User user = new User
            {
                Name = payload.Name,
                Email = payload.Email,
                Role = _roleRepository.GetById(payload.RoleId),
                PasswordHash = _securityService.HashPassword(payload.Password, salt),
                PasswordSalt = Convert.ToBase64String(salt),
                CreatedAt = DateTime.Now
            };
            user.Authorities = _authorityRepository.GetByRole(user.Role);
            _userRepository.LinkUserToRole(user);
            _userRepository.LinkUserToAuthorities(user);
            UserDto registeredUserDto = _userRepository.Add(user).ToDto(_mapper);
            return registeredUserDto;
        }

        public string? Login(LoginRequest payload)
        {
            if (payload == null) return null;
            User user = _userRepository.GetByEmail(payload.Email);
            string hashedPassword = _securityService.HashPassword(payload.Password, Convert.FromBase64String(user.PasswordSalt));
            string? token = _securityService.GetToken(user, user.Role.Type.ToString());
            return hashedPassword == user.PasswordHash ? token : throw new Exception("Invalid password");
        }

        public UserDto UpdateById(UserDto userDto, Guid id) => _userRepository.UpdateById(userDto.ToEntity(_roleRepository, _authorityRepository, _mapper), id).ToDto(_mapper);

        public UserDto DeleteById(Guid id) => _userRepository.DeleteById(id).ToDto(_mapper);
    }
}