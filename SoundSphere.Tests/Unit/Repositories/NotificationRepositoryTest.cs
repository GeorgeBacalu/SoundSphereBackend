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
using static SoundSphere.Tests.Mocks.NotificationMock;

namespace SoundSphere.Tests.Unit.Repositories
{
    public class NotificationRepositoryTest
    {
        private readonly Mock<DbSet<Notification>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly INotificationRepository _notificationRepository;

        private readonly Notification _notification1 = GetMockedNotification1();
        private readonly Notification _notification2 = GetMockedNotification2();
        private readonly IList<Notification> _notifications = GetMockedNotifications();
        private readonly IList<Notification> _paginatedNotifications = GetMockedPaginatedNotifications();
        private readonly NotificationPaginationRequest _paginationRequest = GetMockedNotificationsPaginationRequest();

        public NotificationRepositoryTest()
        {
            IQueryable<Notification> queryableNotifications = _notifications.AsQueryable();
            _dbSetMock.As<IQueryable<Notification>>().Setup(mock => mock.Provider).Returns(queryableNotifications.Provider);
            _dbSetMock.As<IQueryable<Notification>>().Setup(mock => mock.Expression).Returns(queryableNotifications.Expression);
            _dbSetMock.As<IQueryable<Notification>>().Setup(mock => mock.ElementType).Returns(queryableNotifications.ElementType);
            _dbSetMock.As<IQueryable<Notification>>().Setup(mock => mock.GetEnumerator()).Returns(queryableNotifications.GetEnumerator());
            _dbContextMock.Setup(mock => mock.Notifications).Returns(_dbSetMock.Object);
            _notificationRepository = new NotificationRepository(_dbContextMock.Object);
        }

        [Fact] public void GetAll_Test() => _notificationRepository.GetAll(_paginationRequest).Should().BeEquivalentTo(_paginatedNotifications);

        [Fact] public void GetById_ValidId_Test() => _notificationRepository.GetById(ValidNotificationGuid).Should().Be(_notification1);

        [Fact] public void GetById_InvalidId_Test() => _notificationRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(NotificationNotFound, InvalidGuid));

        [Fact] public void Add_Test()
        {
            _notificationRepository.Add(_notification1).Should().Be(_notification1);
            _dbSetMock.Verify(mock => mock.Add(It.IsAny<Notification>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Notification updatedNotification = new Notification
            {
                Id = ValidNotificationGuid,
                User = _notification1.User,
                Type = _notification2.Type,
                Message = _notification2.Message,
                IsRead = _notification2.IsRead,
                CreatedAt = _notification1.CreatedAt
            };
            _notificationRepository.UpdateById(_notification2, ValidNotificationGuid).Should().Be(updatedNotification);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _notificationRepository
            .Invoking(repository => repository.UpdateById(_notification2, InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(NotificationNotFound, InvalidGuid));

        [Fact] public void DeleteById_ValidId_Test()
        {
            _notificationRepository.DeleteById(ValidNotificationGuid);
            _dbSetMock.Verify(mock => mock.Remove(It.IsAny<Notification>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DeleteById_InvalidId_Test() => _notificationRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(NotificationNotFound, InvalidGuid));
    }
}