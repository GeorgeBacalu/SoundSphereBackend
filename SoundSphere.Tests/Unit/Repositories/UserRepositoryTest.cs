using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Repositories
{
    public class UserRepositoryTest
    {
        private readonly Mock<DbSet<User>> _setMock = new();
        private readonly Mock<SoundSphereContext> _contextMock = new();
        private readonly IUserRepository _userRepository;

        private readonly User _user1 = UserMock.GetMockedUser1();
        private readonly User _user2 = UserMock.GetMockedUser2();
        private readonly IList<User> _users = UserMock.GetMockedUsers();
        private readonly IList<User> _activeUsers = UserMock.GetMockedActiveUsers();

        public UserRepositoryTest()
        {
            var queryableUsers = _users.AsQueryable();
            _setMock.As<IQueryable<User>>().Setup(mock => mock.Provider).Returns(queryableUsers.Provider);
            _setMock.As<IQueryable<User>>().Setup(mock => mock.Expression).Returns(queryableUsers.Expression);
            _setMock.As<IQueryable<User>>().Setup(mock => mock.ElementType).Returns(queryableUsers.ElementType);
            _setMock.As<IQueryable<User>>().Setup(mock => mock.GetEnumerator()).Returns(queryableUsers.GetEnumerator());
            _contextMock.Setup(mock => mock.Users).Returns(_setMock.Object);
            _userRepository = new UserRepository(_contextMock.Object);
        }

        [Fact] public void FindAll_Test() => _userRepository.FindAll().Should().BeEquivalentTo(_users);

        [Fact] public void FindAllActive_Test() => _userRepository.FindAllActive().Should().BeEquivalentTo(_activeUsers);

        [Fact] public void FindById_ValidId_Test() => _userRepository.FindById(Constants.ValidUserGuid).Should().BeEquivalentTo(_user1);

        [Fact] public void FindById_InvalidId_Test() =>
            _userRepository.Invoking(repository => repository.FindById(Constants.InvalidGuid))
                           .Should().Throw<ResourceNotFoundException>()
                           .WithMessage($"User with id {Constants.InvalidGuid} not found!");

        [Fact] public void Save_Test()
        {
            _userRepository.Save(_user1).Should().BeEquivalentTo(_user1);
            _setMock.Verify(mock => mock.Add(It.IsAny<User>()));
            _contextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            User updatedUser = CreateTestUser(_user2, _user1.IsActive);
            _userRepository.UpdateById(_user2, Constants.ValidUserGuid).Should().BeEquivalentTo(updatedUser);
            _contextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() =>
            _userRepository.Invoking(repository => repository.UpdateById(_user2, Constants.InvalidGuid))
                           .Should().Throw<ResourceNotFoundException>()
                           .WithMessage($"User with id {Constants.InvalidGuid} not found!");

        [Fact] public void DisableById_ValidId_Test()
        {
            User disabledUser = CreateTestUser(_user1, false);
            _userRepository.DisableById(Constants.ValidUserGuid).Should().BeEquivalentTo(disabledUser);
            _contextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DisableById_InvalidId_Test() =>
            _userRepository.Invoking(repository => repository.DisableById(Constants.InvalidGuid))
                           .Should().Throw<ResourceNotFoundException>()
                           .WithMessage($"User with id {Constants.InvalidGuid} not found!");

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