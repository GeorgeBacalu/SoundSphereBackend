using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Services;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.UserMock;
using static SoundSphere.Tests.Mocks.RoleMock;
using static SoundSphere.Tests.Mocks.AuthorityMock;

namespace SoundSphere.Tests.Unit.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IRoleRepository> _roleRepositoryMock = new();
        private readonly Mock<IAuthorityRepository> _authorityRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly IUserService _userService;

        private readonly User _user1 = GetMockedUser1();
        private readonly User _user2 = GetMockedUser2();
        private readonly IList<User> _users = GetMockedUsers();
        private readonly IList<User> _paginatedUsers = GetMockedPaginatedUsers();
        private readonly UserDto _userDto1 = GetMockedUserDto1();
        private readonly UserDto _userDto2 = GetMockedUserDto2();
        private readonly IList<UserDto> _userDtos = GetMockedUserDtos();
        private readonly IList<UserDto> _paginatedUserDtos = GetMockedPaginatedUserDtos();
        private readonly UserPaginationRequest _paginationRequest = GetMockedUsersPaginationRequest();
        private readonly Role _role1 = GetMockedRole1();
        private readonly IList<Authority> _authorities1 = GetMockedAuthorities1();

        public UserServiceTest()
        {
            _mapperMock.Setup(mock => mock.Map<UserDto>(_user1)).Returns(_userDto1);
            _mapperMock.Setup(mock => mock.Map<UserDto>(_user2)).Returns(_userDto2);
            _mapperMock.Setup(mock => mock.Map<User>(_userDto1)).Returns(_user1);
            _mapperMock.Setup(mock => mock.Map<User>(_userDto2)).Returns(_user2);
            _userService = new UserService(_userRepositoryMock.Object, _roleRepositoryMock.Object, _authorityRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact] public void GetAll_Test()
        {
            _userRepositoryMock.Setup(mock => mock.GetAll(_paginationRequest)).Returns(_paginatedUsers);
            _userService.GetAll(_paginationRequest).Should().BeEquivalentTo(_paginatedUserDtos);
        }

        [Fact] public void GetById_Test()
        {
            _userRepositoryMock.Setup(mock => mock.GetById(ValidUserGuid)).Returns(_user1);
            _userService.GetById(ValidUserGuid).Should().Be(_userDto1);
        }

        [Fact] public void Add_Test()
        {
            _userDto1.AuthoritiesIds.ToList().ForEach(id => _authorityRepositoryMock.Setup(mock => mock.GetById(id)).Returns(_authorities1.First(authority => authority.Id == id)));
            _roleRepositoryMock.Setup(mock => mock.GetById(ValidRoleGuid)).Returns(_role1);
            _userRepositoryMock.Setup(mock => mock.Add(_user1)).Returns(_user1);
            _userService.Add(_userDto1).Should().Be(_userDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            User updatedUser = GetUser(_user2, true);
            UserDto updatedUserDto = ToDto(updatedUser);
            _mapperMock.Setup(mock => mock.Map<UserDto>(updatedUser)).Returns(updatedUserDto);
            _userRepositoryMock.Setup(mock => mock.UpdateById(_user2, ValidUserGuid)).Returns(updatedUser);
            _userService.UpdateById(_userDto2, ValidUserGuid).Should().Be(updatedUserDto);
        }

        [Fact] public void DeleteById_Test()
        {
            User deletedUser = GetUser(_user1, false);
            UserDto deletedUserDto = ToDto(deletedUser);
            _mapperMock.Setup(mock => mock.Map<UserDto>(deletedUser)).Returns(deletedUserDto);
            _userRepositoryMock.Setup(mock => mock.DeleteById(ValidUserGuid)).Returns(deletedUser);
            _userService.DeleteById(ValidUserGuid).Should().Be(deletedUserDto);
        }

        private User GetUser(User user, bool isActive) => new User
        {
            Id = ValidUserGuid,
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Mobile = user.Mobile,
            Address = user.Address,
            Birthday = user.Birthday,
            Avatar = user.Avatar,
            Role = user.Role,
            Authorities = user.Authorities
        };

        private UserDto ToDto(User user) => new UserDto
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
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            DeletedAt = user.DeletedAt
        };
    }
}