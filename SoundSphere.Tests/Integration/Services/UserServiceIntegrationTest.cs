using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Services;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Integration.Services
{
    public class UserServiceIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly IMapper _mapper;

        private readonly User _user1 = UserMock.GetMockedUser1();
        private readonly User _user2 = UserMock.GetMockedUser2();
        private readonly IList<User> _users = UserMock.GetMockedUsers();
        private readonly UserDto _userDto1 = UserMock.GetMockedUserDto1();
        private readonly UserDto _userDto2 = UserMock.GetMockedUserDto2();
        private readonly IList<UserDto> _userDtos = UserMock.GetMockedUserDtos();
        private readonly IList<UserDto> _activeUserDtos = UserMock.GetMockedActiveUserDtos();

        public UserServiceIntegrationTest(DbFixture fixture) => (_fixture, _mapper) = (fixture, new MapperConfiguration(config => { config.CreateMap<User, UserDto>(); config.CreateMap<UserDto, User>(); }).CreateMapper());

        private void Execute(Action<UserService, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var userService = new UserService(new UserRepository(context), new RoleRepository(context), new AuthorityRepository(context), _mapper);
            using var transaction = context.Database.BeginTransaction();
            context.AddRange(_users);
            context.SaveChanges();
            action(userService, context);
        }

        [Fact] public void FindAll_Test() => Execute((userService, context) => userService.FindAll().Should().BeEquivalentTo(_userDtos));

        [Fact] public void FindAllActive_Test() => Execute((userService, context) => userService.FindAllActive().Should().BeEquivalentTo(_activeUserDtos));

        [Fact] public void FindById_Test() => Execute((userService, context) => userService.FindById(Constants.ValidUserGuid).Should().Be(_userDto1));

        [Fact] public void Save_Test() => Execute((userService, context) =>
        {
            UserDto newUserDto = UserMock.GetMockedUserDto3();
            userService.Save(newUserDto);
            context.Users.Find(newUserDto.Id).Should().Be(newUserDto);
        });

        [Fact] public void UpdateById_Test() => Execute((userService, context) =>
        {
            User updatedUser = CreateTestUser(_user2, _user1.IsActive);
            UserDto updatedUserDto = userService.ConvertToDto(updatedUser);
            userService.UpdateById(_userDto2, Constants.ValidUserGuid);
            context.Users.Find(Constants.ValidUserGuid).Should().Be(updatedUser);
        });

        [Fact] public void DisableById_Test() => Execute((userService, context) =>
        {
            User disabledUser = CreateTestUser(_user1, false);
            UserDto disabledUserDto = userService.ConvertToDto(disabledUser);
            userService.DisableById(Constants.ValidUserGuid);
            context.Users.Find(Constants.ValidUserGuid).Should().Be(disabledUser);
        });

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
    }
}