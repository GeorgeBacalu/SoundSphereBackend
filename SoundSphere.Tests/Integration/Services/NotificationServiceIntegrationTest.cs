using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Integration.Services
{
    public class NotificationServiceIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly IMapper _mapper;

        private readonly Notification _notification1 = NotificationMock.GetMockedNotification1();
        private readonly Notification _notification2 = NotificationMock.GetMockedNotification2();
        private readonly IList<Notification> _notifications = NotificationMock.GetMockedNotifications();
        private readonly NotificationDto _notificationDto1 = NotificationMock.GetMockedNotificationDto1();
        private readonly NotificationDto _notificationDto2 = NotificationMock.GetMockedNotificationDto2();
        private readonly IList<NotificationDto> _notificationDtos = NotificationMock.GetMockedNotificationDtos();
        private readonly IList<NotificationDto> _paginatedNotificationDtos = NotificationMock.GetMockedPaginatedNotificationDtos();
        private readonly NotificationPaginationRequest _paginationRequest = NotificationMock.GetMockedPaginationRequest();

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

        [Fact] public void FindAll_Test() => Execute((notificationService, context) => notificationService.FindAll().Should().BeEquivalentTo(_notificationDtos));

        [Fact] public void FindAllPagination_Test() => Execute((notificationService, context) => notificationService.FindAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedNotificationDtos));
        
        [Fact] public void FindById_Test() => Execute((notificationService, context) => notificationService.FindById(Constants.ValidNotificationGuid).Should().Be(_notificationDto1));

        [Fact] public void Save_Test() => Execute((notificationService, context) =>
        {
            NotificationDto newNotificationDto = NotificationMock.GetMockedNotificationDto37();
            notificationService.Save(newNotificationDto);
            context.Notifications.Find(newNotificationDto.Id).Should().BeEquivalentTo(newNotificationDto, options => options.Excluding(notification => notification.SentAt));
        });

        [Fact] public void UpdateById_Test() => Execute((notificationService, context) =>
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
            NotificationDto updatedNotificationDto = updatedNotification.ToDto(_mapper);
            notificationService.UpdateById(_notificationDto2, Constants.ValidNotificationGuid);
            context.Notifications.Find(Constants.ValidNotificationGuid).Should().Be(updatedNotification);
        });

        [Fact] public void DeleteById_Test() => Execute((notificationService, context) =>
        {
            notificationService.DeleteById(Constants.ValidNotificationGuid);
            context.Notifications.Should().BeEquivalentTo(new List<Notification> { _notification2 });
        });
    }
}