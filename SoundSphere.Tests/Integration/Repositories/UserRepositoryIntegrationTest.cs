using FluentAssertions;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class UserRepositoryIntegrationTest
    {
        private readonly DbFixture _fixture;

        private readonly User _user1 = UserMock.GetMockedUser1();
        private readonly User _user2 = UserMock.GetMockedUser2();
        private readonly IList<User> _users = UserMock.GetMockedUsers();
        private readonly IList<User> _activeUsers = UserMock.GetMockedActiveUsers();

        public UserRepositoryIntegrationTest(DbFixture fixture) => _fixture = fixture;

        private void Execute(Action<UserRepository, SoundSphereContext> action)
        {
            using var context = _fixture.CreateContext();
            var userRepository = new UserRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Users.AddRange(_users);
            context.SaveChanges();
            action(userRepository, context);
        }

        [Fact] public void FindAll_Test() => Execute((userRepository, context) => userRepository.FindAll().Should().BeEquivalentTo(_users));

        [Fact] public void FindAllActive_Test() => Execute((userRepository, context) => userRepository.FindAllActive().Should().BeEquivalentTo(_activeUsers));

        [Fact] public void FindById_ValidId_Test() => Execute((userRepository, context) => userRepository.FindById(Constants.ValidUserGuid).Should().BeEquivalentTo(_user1));

        [Fact] public void FindById_InvalidId_Test() => Execute((userRepository, context) => 
            userRepository.Invoking(repository => repository.FindById(Constants.InvalidGuid))
                          .Should().Throw<ResourceNotFoundException>()
                          .WithMessage($"User with id {Constants.InvalidGuid} not found!"));

        [Fact] public void Save_Test() => Execute((userRepository, context) =>
        {
            User newUser = UserMock.GetMockedUser3();
            userRepository.Save(newUser);
            context.Users.Find(newUser.Id).Should().BeEquivalentTo(newUser);
        });

        [Fact] public void UpdateById_ValidId_Test() => Execute((userRepository, context) =>
        {
            User updatedUser = CreateTestUser(_user2, _user1.IsActive);
            userRepository.UpdateById(_user2, Constants.ValidUserGuid);
            context.Users.Find(Constants.ValidUserGuid).Should().BeEquivalentTo(updatedUser);
        });

        [Fact] public void UpdateById_InvalidId_Test() => Execute((userRepository, context) =>
            userRepository.Invoking(repository => repository.UpdateById(_user2, Constants.InvalidGuid))
                          .Should().Throw<ResourceNotFoundException>()
                          .WithMessage($"User with id {Constants.InvalidGuid} not found!"));

        [Fact] public void DisableById_ValidId_Test() => Execute((userRepository, context) =>
        {
            User disabledUser = CreateTestUser(_user1, false);
            userRepository.DisableById(Constants.ValidUserGuid);
            context.Users.Find(Constants.ValidUserGuid).Should().BeEquivalentTo(disabledUser);
        });

        [Fact] public void DisableById_InvalidId_Test() => Execute((userRepository, context) =>
            userRepository.Invoking(repository => repository.DisableById(Constants.InvalidGuid))
                          .Should().Throw<ResourceNotFoundException>()
                          .WithMessage($"User with id {Constants.InvalidGuid} not found!"));

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