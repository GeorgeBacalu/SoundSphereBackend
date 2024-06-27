using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Dtos.Request.Auth;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using SoundSphere.Database.Context;

namespace SoundSphere.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthorityRepository _authorityRepository;
        private readonly ISecurityService _securityService;
        private readonly SoundSphereDbContext _context;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IAuthorityRepository authorityRepository, ISecurityService securityService, SoundSphereDbContext context, IMapper mapper) => 
            (_userRepository, _roleRepository, _authorityRepository, _securityService, _context, _mapper) = (userRepository, roleRepository, authorityRepository, securityService, context, mapper);

        public IList<UserDto> GetAll(UserPaginationRequest? payload)
        {
            IList<UserDto> userDtos = _userRepository.GetAll(payload).ToDtos(_mapper);
            return userDtos;
        }

        public UserDto GetById(Guid id)
        {
            UserDto userDto = _userRepository.GetById(id).ToDto(_mapper);
            return userDto;
        }

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
            _userRepository.AddUserSong(user);
            _userRepository.AddUserArtist(user);
            UserDto createdUserDto = _userRepository.Add(user).ToDto(_mapper);
            return createdUserDto;
        }

        public string? Login(LoginRequest payload)
        {
            if (payload == null) return null;
            User user = _userRepository.GetByEmail(payload.Email);
            string hashedPassword = _securityService.HashPassword(payload.Password, Convert.FromBase64String(user.PasswordSalt));
            string? token = _securityService.GetToken(user, user.Role.Type.ToString());
            return hashedPassword == user.PasswordHash ? token : throw new InvalidRequestException(InvalidPassword);
        }

        public UserDto UpdateById(UserDto userDto, Guid id)
        {
            User userToUpdate = userDto.ToEntity(_roleRepository, _authorityRepository, _mapper);
            UserDto updatedUserDto = _userRepository.UpdateById(userToUpdate, id).ToDto(_mapper);
            updatedUserDto.AuthoritiesIds = _authorityRepository.GetByRole(userToUpdate.Role).Select(authority => authority.Id).ToList();
            return updatedUserDto;
        }

        public UserDto DeleteById(Guid id)
        {
            UserDto deletedUserDto = _userRepository.DeleteById(id).ToDto(_mapper);
            return deletedUserDto;
        }

        public UserDto? UpdatePreferences(UserPreferencesDto payload, Guid userId)
        {
            if (payload == null) return null;
            User user = _userRepository.GetById(userId);
            user.EmailNotifications = payload.EmailNotifications;
            user.Theme = payload.Theme;
            UserDto updatedUserDto = _userRepository.UpdateById(user, userId).ToDto(_mapper);
            return updatedUserDto;
        }

        public void ChangePassword(ChangePasswordRequest payload, Guid userId)
        {
            if (payload == null) return;
            User user = _userRepository.GetById(userId);
            string hashedOldPassword = _securityService.HashPassword(payload.OldPassword, Convert.FromBase64String(user.PasswordSalt));
            if (!user.PasswordHash.Equals(hashedOldPassword)) 
                throw new InvalidRequestException(InvalidOldPassword);
            if (!payload.NewPassword.Equals(payload.ConfirmPassword))
                throw new InvalidRequestException(PasswordMismatch);
            byte[] salt = _securityService.GenerateSalt();
            user.PasswordHash = _securityService.HashPassword(payload.NewPassword, salt);
            user.PasswordSalt = Convert.ToBase64String(salt);
            _userRepository.UpdateById(user, userId);
        }
    }
}