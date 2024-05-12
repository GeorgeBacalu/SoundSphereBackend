using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Repositories
{
    public class UserRepositoryTest
    {
        private readonly Mock<DbSet<User>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IUserRepository _userRepository;

        private readonly User _user1 = UserMock.GetMockedUser1();
        private readonly User _user2 = UserMock.GetMockedUser2();
        private readonly IList<User> _users = UserMock.GetMockedUsers();
        private readonly IList<User> _activeUsers = UserMock.GetMockedActiveUsers();
        private readonly IList<User> _paginatedUsers = UserMock.GetMockedPaginatedUsers();
        private readonly IList<User> _activePaginatedUsers = UserMock.GetMockedActivePaginatedUsers();
        private readonly UserPaginationRequest _paginationRequest = UserMock.GetMockedPaginationRequest();

        public UserRepositoryTest()
        {
            IQueryable<User> queryableUsers = _users.AsQueryable();
            _dbSetMock.As<IQueryable<User>>().Setup(mock => mock.Provider).Returns(queryableUsers.Provider);
            _dbSetMock.As<IQueryable<User>>().Setup(mock => mock.Expression).Returns(queryableUsers.Expression);
            _dbSetMock.As<IQueryable<User>>().Setup(mock => mock.ElementType).Returns(queryableUsers.ElementType);
            _dbSetMock.As<IQueryable<User>>().Setup(mock => mock.GetEnumerator()).Returns(queryableUsers.GetEnumerator());
            _dbContextMock.Setup(mock => mock.Users).Returns(_dbSetMock.Object);
            _userRepository = new UserRepository(_dbContextMock.Object);
        }

        [Fact] public void FindAll_Test() => _userRepository.FindAll().Should().BeEquivalentTo(_users);

        [Fact] public void FindAllActive_Test() => _userRepository.FindAllActive().Should().BeEquivalentTo(_activeUsers);

        [Fact] public void FindAllPagination_Test() => _userRepository.FindAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedUsers);

        [Fact] public void FindAllActivePagination_Test() => _userRepository.FindAllActivePagination(_paginationRequest).Should().BeEquivalentTo(_activePaginatedUsers);

        [Fact] public void FindById_ValidId_Test() => _userRepository.FindById(Constants.ValidUserGuid).Should().Be(_user1);

        [Fact] public void FindById_InvalidId_Test() => _userRepository
            .Invoking(repository => repository.FindById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.UserNotFound, Constants.InvalidGuid));

        [Fact] public void Save_Test()
        {
            _userRepository.Save(_user1).Should().Be(_user1);
            _dbSetMock.Verify(mock => mock.Add(It.IsAny<User>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            User updatedUser = GetUser(_user2, _user1.IsActive);
            _userRepository.UpdateById(_user2, Constants.ValidUserGuid).Should().Be(updatedUser);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _userRepository
            .Invoking(repository => repository.UpdateById(_user2, Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.UserNotFound, Constants.InvalidGuid));

        [Fact] public void DisableById_ValidId_Test()
        {
            User disabledUser = GetUser(_user1, false);
            _userRepository.DisableById(Constants.ValidUserGuid).Should().Be(disabledUser);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DisableById_InvalidId_Test() => _userRepository
            .Invoking(repository => repository.DisableById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.UserNotFound, Constants.InvalidGuid));

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