using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.UserMock;

namespace SoundSphere.Tests.Integration.Services
{
    public class UserServiceIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly IMapper _mapper;

        private readonly User _user1 = GetMockedUser1();
        private readonly User _user2 = GetMockedUser2();
        private readonly IList<User> _users = GetMockedUsers();
        private readonly UserDto _userDto1 = GetMockedUserDto1();
        private readonly UserDto _userDto2 = GetMockedUserDto2();
        private readonly IList<UserDto> _userDtos = GetMockedUserDtos();
        private readonly IList<UserDto> _paginatedUserDtos = GetMockedPaginatedUserDtos();
        private readonly UserPaginationRequest _paginationRequest = GetMockedUsersPaginationRequest();

        public UserServiceIntegrationTest(DbFixture fixture) => (_fixture, _mapper) = (fixture, new MapperConfiguration(config => { config.CreateMap<User, UserDto>(); config.CreateMap<UserDto, User>(); }).CreateMapper());

        private void Execute(Action<UserService, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var userService = new UserService(new UserRepository(context), new RoleRepository(context), new AuthorityRepository(context), _mapper);
            using var transaction = context.Database.BeginTransaction();
            context.AddRange(_users);
            context.SaveChanges();
            action(userService, context);
            transaction.Rollback();
        }

        [Fact] public void GetAll_Test() => Execute((userService, context) => userService.GetAll(_paginationRequest).Should().BeEquivalentTo(_paginatedUserDtos));
        
        [Fact] public void GetById_Test() => Execute((userService, context) => userService.GetById(ValidUserGuid).Should().Be(_userDto1));

        [Fact] public void Add_Test() => Execute((userService, context) =>
        {
            UserDto newUserDto = GetMockedUserDto11();
            UserDto result = userService.Add(newUserDto);
            context.Users.Find(newUserDto.Id).Should().Be(newUserDto);
            result.Should().Be(newUserDto);
        });

        [Fact] public void UpdateById_Test() => Execute((userService, context) =>
        {
            User updatedUser = GetUser(_user2, true);
            UserDto updatedUserDto = updatedUser.ToDto(_mapper);
            UserDto result = userService.UpdateById(_userDto2, ValidUserGuid);
            context.Users.Find(ValidUserGuid).Should().Be(updatedUser);
            result.Should().Be(updatedUserDto);
        });

        [Fact] public void DeleteById_Test() => Execute((userService, context) =>
        {
            User deletedUser = GetUser(_user1, false);
            UserDto deletedUserDto = deletedUser.ToDto(_mapper);
            UserDto result = userService.DeleteById(ValidUserGuid);
            context.Users.Find(ValidUserGuid).Should().Be(deletedUser);
            result.Should().Be(deletedUserDto);
        });

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
            Authorities = user.Authorities,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            DeletedAt = user.DeletedAt
        };
    }
}