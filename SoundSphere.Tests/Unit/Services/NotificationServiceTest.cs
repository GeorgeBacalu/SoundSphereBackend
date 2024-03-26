﻿using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Services;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Services
{
    public class NotificationServiceTest
    {
        private readonly Mock<INotificationRepository> _notificationRepository = new();
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly INotificationService _notificationService;

        private readonly Notification _notification1 = NotificationMock.GetMockedNotification1();
        private readonly Notification _notification2 = NotificationMock.GetMockedNotification2();
        private readonly IList<Notification> _notifications = NotificationMock.GetMockedNotifications();
        private readonly NotificationDto _notificationDto1 = NotificationMock.GetMockedNotificationDto1();
        private readonly NotificationDto _notificationDto2 = NotificationMock.GetMockedNotificationDto2();
        private readonly IList<NotificationDto> _notificationDtos = NotificationMock.GetMockedNotificationDtos();
        private readonly User _user1 = UserMock.GetMockedUser1();

        public NotificationServiceTest()
        {
            _mapper.Setup(mock => mock.Map<NotificationDto>(_notification1)).Returns(_notificationDto1);
            _mapper.Setup(mock => mock.Map<NotificationDto>(_notification2)).Returns(_notificationDto2);
            _mapper.Setup(mock => mock.Map<Notification>(_notificationDto1)).Returns(_notification1);
            _mapper.Setup(mock => mock.Map<Notification>(_notificationDto2)).Returns(_notification2);
            _notificationService = new NotificationService(_notificationRepository.Object, _userRepository.Object, _mapper.Object);
        }

        [Fact] public void FindAll_Test()
        {
            _notificationRepository.Setup(mock => mock.FindAll()).Returns(_notifications);
            _notificationService.FindAll().Should().BeEquivalentTo(_notificationDtos);
        }

        [Fact] public void FindById_Test()
        {
            _notificationRepository.Setup(mock => mock.FindById(Constants.ValidNotificationGuid)).Returns(_notification1);
            _notificationService.FindById(Constants.ValidNotificationGuid).Should().BeEquivalentTo(_notificationDto1);
        }

        [Fact] public void Save_Test()
        {
            _userRepository.Setup(mock => mock.FindById(Constants.ValidUserGuid)).Returns(_user1);
            _notificationRepository.Setup(mock => mock.Save(_notification1)).Returns(_notification1);
            _notificationService.Save(_notificationDto1).Should().BeEquivalentTo(_notificationDto1);
        }

        [Fact] public void UpdateById_Test()
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
            NotificationDto updatedNotificationDto = ConvertToDto(updatedNotification);
            _mapper.Setup(mock => mock.Map<NotificationDto>(updatedNotification)).Returns(updatedNotificationDto);
            _notificationRepository.Setup(mock => mock.UpdateById(_notification2, Constants.ValidNotificationGuid)).Returns(updatedNotification);
            _notificationService.UpdateById(_notificationDto2, Constants.ValidNotificationGuid).Should().BeEquivalentTo(updatedNotificationDto);
        }

        [Fact] public void DeleteById_Test()
        {
            _notificationService.DeleteById(Constants.ValidNotificationGuid);
            _notificationRepository.Verify(mock => mock.DeleteById(Constants.ValidNotificationGuid));
        }

        private NotificationDto ConvertToDto(Notification notification) => new NotificationDto
        {
            Id = notification.Id,
            UserId = notification.User.Id,
            Type = notification.Type,
            Message = notification.Message,
            SentAt = notification.SentAt,
            IsRead = notification.IsRead
        };
    }
}