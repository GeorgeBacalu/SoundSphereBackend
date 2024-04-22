﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database;
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
        private readonly Mock<DbSet<Notification>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly INotificationRepository _notificationRepository;

        private readonly Notification _notification1 = NotificationMock.GetMockedNotification1();
        private readonly Notification _notification2 = NotificationMock.GetMockedNotification2();
        private readonly IList<Notification> _notifications = NotificationMock.GetMockedNotifications();

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

        [Fact] public void FindAll_Test() => _notificationRepository.FindAll().Should().BeEquivalentTo(_notifications);

        [Fact] public void FindById_ValidId_Test() => _notificationRepository.FindById(Constants.ValidNotificationGuid).Should().Be(_notification1);

        [Fact] public void FindById_InvalidId_Test() => _notificationRepository
            .Invoking(repository => repository.FindById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.NotificationNotFound, Constants.InvalidGuid));

        [Fact] public void Save_Test()
        {
            _notificationRepository.Save(_notification1).Should().Be(_notification1);
            _dbSetMock.Verify(mock => mock.Add(It.IsAny<Notification>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
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
            _notificationRepository.UpdateById(_notification2, Constants.ValidNotificationGuid).Should().Be(updatedNotification);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _notificationRepository
            .Invoking(repository => repository.UpdateById(_notification2, Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.NotificationNotFound, Constants.InvalidGuid));

        [Fact] public void DeleteById_ValidId_Test()
        {
            _notificationRepository.DeleteById(Constants.ValidNotificationGuid);
            _dbSetMock.Verify(mock => mock.Remove(It.IsAny<Notification>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DeleteById_InvalidId_Test() => _notificationRepository
            .Invoking(repository => repository.DeleteById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.NotificationNotFound, Constants.InvalidGuid));
    }
}