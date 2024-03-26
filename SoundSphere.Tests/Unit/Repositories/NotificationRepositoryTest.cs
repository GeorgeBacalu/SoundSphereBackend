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
    public class NotificationRepositoryTest
    {
        private readonly Mock<DbSet<Notification>> _setMock = new();
        private readonly Mock<SoundSphereContext> _contextMock = new();
        private readonly INotificationRepository _notificationRepository;

        private readonly Notification _notification1 = NotificationMock.GetMockedNotification1();
        private readonly Notification _notification2 = NotificationMock.GetMockedNotification2();
        private readonly IList<Notification> _notifications = NotificationMock.GetMockedNotifications();

        public NotificationRepositoryTest()
        {
            var queryableNotifications = _notifications.AsQueryable();
            _setMock.As<IQueryable<Notification>>().Setup(mock => mock.Provider).Returns(queryableNotifications.Provider);
            _setMock.As<IQueryable<Notification>>().Setup(mock => mock.Expression).Returns(queryableNotifications.Expression);
            _setMock.As<IQueryable<Notification>>().Setup(mock => mock.ElementType).Returns(queryableNotifications.ElementType);
            _setMock.As<IQueryable<Notification>>().Setup(mock => mock.GetEnumerator()).Returns(queryableNotifications.GetEnumerator());
            _contextMock.Setup(mock => mock.Notifications).Returns(_setMock.Object);
            _notificationRepository = new NotificationRepository(_contextMock.Object);
        }

        [Fact] public void FindAll_Test() => _notificationRepository.FindAll().Should().BeEquivalentTo(_notifications);

        [Fact] public void FindById_ValidId_Test() => _notificationRepository.FindById(Constants.ValidNotificationGuid).Should().BeEquivalentTo(_notification1);

        [Fact] public void FindById_InvalidId_Test() =>
            _notificationRepository.Invoking(repository => repository.FindById(Constants.InvalidGuid))
                                   .Should().Throw<ResourceNotFoundException>()
                                   .WithMessage($"Notification with id {Constants.InvalidGuid} not found!");

        [Fact] public void Save_Test()
        {
            _notificationRepository.Save(_notification1).Should().BeEquivalentTo(_notification1);
            _setMock.Verify(mock => mock.Add(It.IsAny<Notification>()));
            _contextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Notification updatedNotification = new Notification
            {
                Id = Constants.ValidNotificationGuid,
                User = _notification1.User,
                Type = _notification2.Type,
                Message = _notification2.Message,
                SentAt = _notification1.SentAt,
                IsRead = _notification2.IsRead
            };
            _notificationRepository.UpdateById(_notification2, Constants.ValidNotificationGuid).Should().BeEquivalentTo(updatedNotification);
            _contextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() =>
            _notificationRepository.Invoking(repository => repository.UpdateById(_notification2, Constants.InvalidGuid))
                                   .Should().Throw<ResourceNotFoundException>()
                                   .WithMessage($"Notification with id {Constants.InvalidGuid} not found!");

        [Fact] public void DeleteById_ValidId_Test()
        {
            _notificationRepository.DeleteById(Constants.ValidNotificationGuid);
            _setMock.Verify(mock => mock.Remove(It.IsAny<Notification>()));
            _contextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DeleteById_InvalidId_Test() =>
            _notificationRepository.Invoking(repository => repository.DeleteById(Constants.InvalidGuid))
                                   .Should().Throw<ResourceNotFoundException>()
                                   .WithMessage($"Notification with id {Constants.InvalidGuid} not found!");
    }
}