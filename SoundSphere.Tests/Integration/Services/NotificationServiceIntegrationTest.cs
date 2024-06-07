using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.NotificationMock;

namespace SoundSphere.Tests.Integration.Services
{
    public class NotificationServiceIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly IMapper _mapper;

        private readonly Notification _notification1 = GetMockedNotification1();
        private readonly Notification _notification2 = GetMockedNotification2();
        private readonly IList<Notification> _notifications = GetMockedNotifications();
        private readonly NotificationDto _notificationDto1 = GetMockedNotificationDto1();
        private readonly NotificationDto _notificationDto2 = GetMockedNotificationDto2();
        private readonly IList<NotificationDto> _notificationDtos = GetMockedNotificationDtos();
        private readonly IList<NotificationDto> _paginatedNotificationDtos = GetMockedPaginatedNotificationDtos();
        private readonly NotificationPaginationRequest _paginationRequest = GetMockedNotificationsPaginationRequest();

        public NotificationServiceIntegrationTest(DbFixture fixture) => (_fixture, _mapper) = (fixture, new MapperConfiguration(config => { config.CreateMap<Notification, NotificationDto>(); config.CreateMap<NotificationDto, Notification>(); }).CreateMapper());

        private void Execute(Action<NotificationService, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var notificationService = new NotificationService(new NotificationRepository(context), new UserRepository(context), _mapper);
            using var transaction = context.Database.BeginTransaction();
            context.AddRange(_notifications);
            context.SaveChanges();
            action(notificationService, context);
            transaction.Rollback();
        }

        [Fact] public void GetAll_Test() => Execute((notificationService, context) => notificationService.GetAll(_paginationRequest).Should().BeEquivalentTo(_paginatedNotificationDtos));
        
        [Fact] public void GetById_Test() => Execute((notificationService, context) => notificationService.GetById(ValidNotificationGuid).Should().Be(_notificationDto1));

        [Fact] public void Add_Test() => Execute((notificationService, context) =>
        {
            NotificationDto newNotificationDto = GetMockedNotificationDto37();
            NotificationDto result = notificationService.Add(newNotificationDto);
            context.Notifications.Find(newNotificationDto.Id).Should().BeEquivalentTo(newNotificationDto, options => options.Excluding(notification => notification.CreatedAt));
            result.Should().Be(newNotificationDto);
        });

        [Fact] public void UpdateById_Test() => Execute((notificationService, context) =>
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
            NotificationDto updatedNotificationDto = updatedNotification.ToDto(_mapper);
            NotificationDto result = notificationService.UpdateById(_notificationDto2, ValidNotificationGuid);
            context.Notifications.Find(ValidNotificationGuid).Should().Be(updatedNotification);
            result.Should().Be(updatedNotificationDto);
        });

        [Fact] public void DeleteById_Test() => Execute((notificationService, context) =>
        {
            notificationService.DeleteById(ValidNotificationGuid);
            IList<Notification> newNotifications = new List<Notification>(_notifications);
            newNotifications.Remove(_notification1);
            context.Notifications.Should().BeEquivalentTo(newNotifications);
        });
    }
}