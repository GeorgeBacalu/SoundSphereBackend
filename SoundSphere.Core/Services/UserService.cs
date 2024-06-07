using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Dtos.Request.Auth;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthorityRepository _authorityRepository;
        private readonly ISecurityService _securityService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IAuthorityRepository authorityRepository, ISecurityService securityService, IMapper mapper) => 
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
                PasswordHash = _securityService.HashPassword(payload.Password, salt),
                PasswordSalt = Convert.ToBase64String(salt),
                Mobile = payload.Mobile,
                Address = payload.Address,
                Birthday = payload.Birthday,
                Avatar = payload.Avatar,
                Role = _roleRepository.GetById(payload.RoleId),
                CreatedAt = DateTime.Now
            };
            user.Authorities = _authorityRepository.GetByRole(user.Role);
            _userRepository.LinkUserToRole(user);
            _userRepository.LinkUserToAuthorities(user);
            return _userRepository.Add(user).ToDto(_mapper);
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