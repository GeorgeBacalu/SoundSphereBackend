using FluentAssertions;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.UserMock;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class UserRepositoryIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        private readonly User _user1 = GetMockedUser1();
        private readonly User _user2 = GetMockedUser2();
        private readonly IList<User> _users = GetMockedUsers();
        private readonly IList<User> _paginatedUsers = GetMockedPaginatedUsers();
        private readonly UserPaginationRequest _paginationRequest = GetMockedUsersPaginationRequest();

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

        [Fact] public void GetAll_Test() => Execute((userRepository, context) => userRepository.GetAll(_paginationRequest).Should().BeEquivalentTo(_paginatedUsers));
        
        [Fact] public void GetById_ValidId_Test() => Execute((userRepository, context) => userRepository.GetById(ValidUserGuid).Should().Be(_user1));

        [Fact] public void GetById_InvalidId_Test() => Execute((userRepository, context) => userRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(UserNotFound, InvalidGuid)));

        [Fact] public void Add_Test() => Execute((userRepository, context) =>
        {
            User newUser = GetMockedUser11();
            userRepository.Add(newUser);
            context.Users.Find(newUser.Id).Should().Be(newUser);
        });

        [Fact] public void UpdateById_ValidId_Test() => Execute((userRepository, context) =>
        {
            User updatedUser = GetUser(_user2, true);
            userRepository.UpdateById(_user2, ValidUserGuid);
            context.Users.Find(ValidUserGuid).Should().Be(updatedUser);
        });

        [Fact] public void UpdateById_InvalidId_Test() => Execute((userRepository, context) => userRepository
            .Invoking(repository => repository.UpdateById(_user2, InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(UserNotFound, InvalidGuid)));

        [Fact] public void DeleteById_ValidId_Test() => Execute((userRepository, context) =>
        {
            User deletedUser = GetUser(_user1, false);
            userRepository.DeleteById(ValidUserGuid);
            context.Users.Find(ValidUserGuid).Should().Be(deletedUser);
        });

        [Fact] public void DeleteById_InvalidId_Test() => Execute((userRepository, context) => userRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(UserNotFound, InvalidGuid)));

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