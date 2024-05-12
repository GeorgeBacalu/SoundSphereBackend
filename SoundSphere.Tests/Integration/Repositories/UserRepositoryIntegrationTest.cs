using FluentAssertions;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class UserRepositoryIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        private readonly User _user1 = UserMock.GetMockedUser1();
        private readonly User _user2 = UserMock.GetMockedUser2();
        private readonly IList<User> _users = UserMock.GetMockedUsers();
        private readonly IList<User> _activeUsers = UserMock.GetMockedActiveUsers();
        private readonly IList<User> _paginatedUsers = UserMock.GetMockedPaginatedUsers();
        private readonly IList<User> _activePaginatedUsers = UserMock.GetMockedActivePaginatedUsers();
        private readonly UserPaginationRequest _paginationRequest = UserMock.GetMockedPaginationRequest();

        public UserRepositoryIntegrationTest(DbFixture fixture) => _fixture = fixture;

        private void Execute(Action<UserRepository, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var userRepository = new UserRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Users.AddRange(_users);
            context.SaveChanges();
            action(userRepository, context);
            transaction.Rollback();
        }

        [Fact] public void FindAll_Test() => Execute((userRepository, context) => userRepository.FindAll().Should().BeEquivalentTo(_users));

        [Fact] public void FindAllActive_Test() => Execute((userRepository, context) => userRepository.FindAllActive().Should().BeEquivalentTo(_activeUsers));

        [Fact] public void FindAllPagination_Test() => Execute((userRepository, context) => userRepository.FindAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedUsers));

        [Fact] public void FindAllActivePagination_Test() => Execute((userRepository, context) => userRepository.FindAllActivePagination(_paginationRequest).Should().BeEquivalentTo(_activePaginatedUsers));
        
        [Fact] public void FindById_ValidId_Test() => Execute((userRepository, context) => userRepository.FindById(Constants.ValidUserGuid).Should().Be(_user1));

        [Fact] public void FindById_InvalidId_Test() => Execute((userRepository, context) => userRepository
            .Invoking(repository => repository.FindById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.UserNotFound, Constants.InvalidGuid)));

        [Fact] public void Save_Test() => Execute((userRepository, context) =>
        {
            User newUser = UserMock.GetMockedUser11();
            userRepository.Save(newUser);
            context.Users.Find(newUser.Id).Should().Be(newUser);
        });

        [Fact] public void UpdateById_ValidId_Test() => Execute((userRepository, context) =>
        {
            User updatedUser = GetUser(_user2, _user1.IsActive);
            userRepository.UpdateById(_user2, Constants.ValidUserGuid);
            context.Users.Find(Constants.ValidUserGuid).Should().Be(updatedUser);
        });

        [Fact] public void UpdateById_InvalidId_Test() => Execute((userRepository, context) => userRepository
            .Invoking(repository => repository.UpdateById(_user2, Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.UserNotFound, Constants.InvalidGuid)));

        [Fact] public void DisableById_ValidId_Test() => Execute((userRepository, context) =>
        {
            User disabledUser = GetUser(_user1, false);
            userRepository.DisableById(Constants.ValidUserGuid);
            context.Users.Find(Constants.ValidUserGuid).Should().Be(disabledUser);
        });

        [Fact] public void DisableById_InvalidId_Test() => Execute((userRepository, context) => userRepository
            .Invoking(repository => repository.DisableById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.UserNotFound, Constants.InvalidGuid)));

        private User GetUser(User user, bool isActive) => new User
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