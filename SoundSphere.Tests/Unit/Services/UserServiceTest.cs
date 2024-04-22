using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Services;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IRoleRepository> _roleRepositoryMock = new();
        private readonly Mock<IAuthorityRepository> _authorityRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly IUserService _userService;

        private readonly User _user1 = UserMock.GetMockedUser1();
        private readonly User _user2 = UserMock.GetMockedUser2();
        private readonly IList<User> _users = UserMock.GetMockedUsers();
        private readonly IList<User> _activeUsers = UserMock.GetMockedActiveUsers();
        private readonly UserDto _userDto1 = UserMock.GetMockedUserDto1();
        private readonly UserDto _userDto2 = UserMock.GetMockedUserDto2();
        private readonly IList<UserDto> _userDtos = UserMock.GetMockedUserDtos();
        private readonly IList<UserDto> _activeUserDtos = UserMock.GetMockedActiveUserDtos();
        private readonly Role _role1 = RoleMock.GetMockedRole1();
        private readonly IList<Authority> _authorities1 = AuthorityMock.GetMockedAuthorities1();

        public UserServiceTest()
        {
            _mapperMock.Setup(mock => mock.Map<UserDto>(_user1)).Returns(_userDto1);
            _mapperMock.Setup(mock => mock.Map<UserDto>(_user2)).Returns(_userDto2);
            _mapperMock.Setup(mock => mock.Map<User>(_userDto1)).Returns(_user1);
            _mapperMock.Setup(mock => mock.Map<User>(_userDto2)).Returns(_user2);
            _userService = new UserService(_userRepositoryMock.Object, _roleRepositoryMock.Object, _authorityRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact] public void FindAll_Test()
        {
            _userRepositoryMock.Setup(mock => mock.FindAll()).Returns(_users);
            _userService.FindAll().Should().BeEquivalentTo(_userDtos);
        }

        [Fact] public void FindAllActive_Test()
        {
            _userRepositoryMock.Setup(mock => mock.FindAllActive()).Returns(_activeUsers);
            _userService.FindAllActive().Should().BeEquivalentTo(_activeUserDtos);
        }

        [Fact] public void FindById_Test()
        {
            _userRepositoryMock.Setup(mock => mock.FindById(Constants.ValidUserGuid)).Returns(_user1);
            _userService.FindById(Constants.ValidUserGuid).Should().Be(_userDto1);
        }

        [Fact] public void Save_Test()
        {
            _userDto1.AuthoritiesIds.ToList().ForEach(id => _authorityRepositoryMock.Setup(mock => mock.FindById(id)).Returns(_authorities1.First(authority => authority.Id == id)));
            _roleRepositoryMock.Setup(mock => mock.FindById(Constants.ValidRoleGuid)).Returns(_role1);
            _userRepositoryMock.Setup(mock => mock.Save(_user1)).Returns(_user1);
            _userService.Save(_userDto1).Should().Be(_userDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            User updatedUser = CreateTestUser(_user2, _user1.IsActive);
            UserDto updatedUserDto = ConvertToDto(updatedUser);
            _mapperMock.Setup(mock => mock.Map<UserDto>(updatedUser)).Returns(updatedUserDto);
            _userRepositoryMock.Setup(mock => mock.UpdateById(_user2, Constants.ValidUserGuid)).Returns(updatedUser);
            _userService.UpdateById(_userDto2, Constants.ValidUserGuid).Should().Be(updatedUserDto);
        }

        [Fact] public void DisableById_Test()
        {
            User disabledUser = CreateTestUser(_user1, false);
            UserDto disabledUserDto = ConvertToDto(disabledUser);
            _mapperMock.Setup(mock => mock.Map<UserDto>(disabledUser)).Returns(disabledUserDto);
            _userRepositoryMock.Setup(mock => mock.DisableById(Constants.ValidUserGuid)).Returns(disabledUser);
            _userService.DisableById(Constants.ValidUserGuid).Should().Be(disabledUserDto);
        }

        private User CreateTestUser(User user, bool isActive) => new User
        {
            Id = Constants.ValidUserGuid,
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Mobile = user.Mobile,
            Address = user.Address,
            Birthday = user.Birthday,
            Avatar = user.Avatar,
            Role = user.Role,
            Authorities = user.Authorities,
            IsActive = isActive
        };

        private UserDto ConvertToDto(User user) => new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Mobile = user.Mobile,
            Address = user.Address,
            Birthday = user.Birthday,
            Avatar = user.Avatar,
            RoleId = user.Role.Id,
            AuthoritiesIds = user.Authorities.Select(authority => authority.Id).ToList(),
            IsActive = user.IsActive
        };
    }
}