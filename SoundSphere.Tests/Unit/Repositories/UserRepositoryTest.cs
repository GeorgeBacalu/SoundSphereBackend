using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.UserMock;

namespace SoundSphere.Tests.Unit.Repositories
{
    public class UserRepositoryTest
    {
        private readonly Mock<DbSet<User>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IUserRepository _userRepository;

        private readonly User _user1 = GetMockedUser1();
        private readonly User _user2 = GetMockedUser2();
        private readonly IList<User> _users = GetMockedUsers();
        private readonly IList<User> _activeUsers = GetMockedActiveUsers();
        private readonly IList<User> _paginatedUsers = GetMockedPaginatedUsers();
        private readonly IList<User> _activePaginatedUsers = GetMockedActivePaginatedUsers();
        private readonly UserPaginationRequest _paginationRequest = GetMockedUsersPaginationRequest();

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

        [Fact] public void GetAll_Test() => _userRepository.GetAll().Should().BeEquivalentTo(_users);

        [Fact] public void GetAllActive_Test() => _userRepository.GetAllActive().Should().BeEquivalentTo(_activeUsers);

        [Fact] public void GetAllPagination_Test() => _userRepository.GetAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedUsers);

        [Fact] public void GetAllActivePagination_Test() => _userRepository.GetAllActivePagination(_paginationRequest).Should().BeEquivalentTo(_activePaginatedUsers);

        [Fact] public void GetById_ValidId_Test() => _userRepository.GetById(ValidUserGuid).Should().Be(_user1);

        [Fact] public void GetById_InvalidId_Test() => _userRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(UserNotFound, InvalidGuid));

        [Fact] public void Add_Test()
        {
            _userRepository.Add(_user1).Should().Be(_user1);
            _dbSetMock.Verify(mock => mock.Add(It.IsAny<User>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            User updatedUser = GetUser(_user2, _user1.IsActive);
            _userRepository.UpdateById(_user2, ValidUserGuid).Should().Be(updatedUser);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _userRepository
            .Invoking(repository => repository.UpdateById(_user2, InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(UserNotFound, InvalidGuid));

        [Fact] public void DeleteById_ValidId_Test()
        {
            User deletedUser = GetUser(_user1, false);
            _userRepository.DeleteById(ValidUserGuid).Should().Be(deletedUser);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DeleteById_InvalidId_Test() => _userRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(UserNotFound, InvalidGuid));

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
            IsActive = isActive
        };
    }
}