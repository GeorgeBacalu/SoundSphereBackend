using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Services;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.NotificationMock;
using static SoundSphere.Tests.Mocks.UserMock;

namespace SoundSphere.Tests.Unit.Services
{
    public class NotificationServiceTest
    {
        private readonly Mock<INotificationRepository> _notificationRepositoryMock = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly INotificationService _notificationService;

        private readonly Notification _notification1 = GetMockedNotification1();
        private readonly Notification _notification2 = GetMockedNotification2();
        private readonly IList<Notification> _notifications = GetMockedNotifications();
        private readonly IList<Notification> _paginatedNotifications = GetMockedPaginatedNotifications();
        private readonly NotificationDto _notificationDto1 = GetMockedNotificationDto1();
        private readonly NotificationDto _notificationDto2 = GetMockedNotificationDto2();
        private readonly IList<NotificationDto> _notificationDtos = GetMockedNotificationDtos();
        private readonly IList<NotificationDto> _paginatedNotificationDtos = GetMockedPaginatedNotificationDtos();
        private readonly NotificationPaginationRequest _paginationRequest = GetMockedNotificationsPaginationRequest();
        private readonly User _user1 = GetMockedUser1();

        public NotificationServiceTest()
        {
            _mapperMock.Setup(mock => mock.Map<NotificationDto>(_notification1)).Returns(_notificationDto1);
            _mapperMock.Setup(mock => mock.Map<NotificationDto>(_notification2)).Returns(_notificationDto2);
            _mapperMock.Setup(mock => mock.Map<Notification>(_notificationDto1)).Returns(_notification1);
            _mapperMock.Setup(mock => mock.Map<Notification>(_notificationDto2)).Returns(_notification2);
            _notificationService = new NotificationService(_notificationRepositoryMock.Object, _userRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact] public void GetAll_Test()
        {
            _notificationRepositoryMock.Setup(mock => mock.GetAll(_paginationRequest)).Returns(_paginatedNotifications);
            _notificationService.GetAll(_paginationRequest).Should().BeEquivalentTo(_paginatedNotificationDtos);
        }

        [Fact] public void GetById_Test()
        {
            _notificationRepositoryMock.Setup(mock => mock.GetById(ValidNotificationGuid)).Returns(_notification1);
            _notificationService.GetById(ValidNotificationGuid).Should().Be(_notificationDto1);
        }

        [Fact] public void Add_Test()
        {
            _userRepositoryMock.Setup(mock => mock.GetById(ValidUserGuid)).Returns(_user1);
            _notificationRepositoryMock.Setup(mock => mock.Add(_notification1)).Returns(_notification1);
            _notificationService.Add(_notificationDto1).Should().Be(_notificationDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            Notification updatedNotification = new Notification
            {
                Id = ValidNotificationGuid,
                User = _notification1.User,
                Type = _notification2.Type,
                Message = _notification2.Message,
                IsRead = _notification2.IsRead,
                CreatedAt = _notification1.CreatedAt,
            };
            NotificationDto updatedNotificationDto = ToDto(updatedNotification);
            _mapperMock.Setup(mock => mock.Map<NotificationDto>(updatedNotification)).Returns(updatedNotificationDto);
            _notificationRepositoryMock.Setup(mock => mock.UpdateById(_notification2, ValidNotificationGuid)).Returns(updatedNotification);
            _notificationService.UpdateById(_notificationDto2, ValidNotificationGuid).Should().Be(updatedNotificationDto);
        }

        [Fact] public void DeleteById_Test()
        {
            _notificationService.DeleteById(ValidNotificationGuid);
            _notificationRepositoryMock.Verify(mock => mock.DeleteById(ValidNotificationGuid));
        }

        private NotificationDto ToDto(Notification notification) => new NotificationDto
        {
            Id = notification.Id,
            UserId = notification.User.Id,
            Type = notification.Type,
            Message = notification.Message,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt,
            UpdatedAt = notification.UpdatedAt,
            DeletedAt = notification.DeletedAt
        };
    }
}