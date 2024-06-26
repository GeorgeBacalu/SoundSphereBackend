using FluentAssertions;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.NotificationMock;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class NotificationRepositoryIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        private readonly Notification _notification1 = GetMockedNotification1();
        private readonly Notification _notification2 = GetMockedNotification2();
        private readonly IList<Notification> _notifications = GetMockedNotifications();
        private readonly IList<Notification> _paginatedNotifications = GetMockedPaginatedNotifications();
        private readonly NotificationPaginationRequest _paginationRequest = GetMockedNotificationsPaginationRequest();

        public NotificationRepositoryIntegrationTest(DbFixture fixture) => _fixture = fixture;

        private void Execute(Action<NotificationRepository, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var notificationRepository = new NotificationRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Notifications.AddRange(_notifications);
            context.SaveChanges();
            action(notificationRepository, context);
            transaction.Rollback();
        }

        [Fact] public void GetAll_Test() => Execute((notificationRepository, context) => notificationRepository.GetAll(_paginationRequest, ValidUserGuid).Should().BeEquivalentTo(_paginatedNotifications));

        [Fact] public void GetById_ValidId_Test() => Execute((notificationRepository, context) => notificationRepository.GetById(ValidNotificationGuid).Should().Be(_notification1));

        [Fact]
        public void GetById_InvalidId_Test() => Execute((notificationRepository, context) => notificationRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(NotificationNotFound, InvalidGuid)));

        [Fact]
        public void Add_Test() => Execute((notificationRepository, context) =>
        {
            Notification newNotification = GetMockedNotification37();
            notificationRepository.Add(newNotification);
            context.Notifications.Find(newNotification.Id).Should().BeEquivalentTo(newNotification, options => options.Excluding(notification => notification.CreatedAt));
        });

        [Fact]
        public void UodateById_ValidId_Test() => Execute((notificationRepository, context) =>
        {
            Notification updatedNotification = new Notification
            {
                Id = ValidNotificationGuid,
                Sender = _notification1.Sender,
                Receiver = _notification1.Receiver,
                Type = _notification2.Type,
                Message = _notification2.Message,
                IsRead = _notification2.IsRead,
                CreatedAt = _notification1.CreatedAt
            };
            notificationRepository.UpdateById(_notification2, ValidNotificationGuid);
            context.Notifications.Find(ValidNotificationGuid).Should().Be(updatedNotification);
        });

        [Fact]
        public void UpdateById_InvalidId_Test() => Execute((notificationRepository, cotext) => notificationRepository
            .Invoking(repository => repository.UpdateById(_notification2, InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(NotificationNotFound, InvalidGuid)));

        [Fact]
        public void DeleteById_ValidId_Test() => Execute((notificationRepository, context) =>
        {
            notificationRepository.DeleteById(ValidNotificationGuid);
            IList<Notification> newNotifications = new List<Notification>(_notifications);
            newNotifications.Remove(_notification1);
            context.Notifications.Should().BeEquivalentTo(newNotifications);
        });

        [Fact]
        public void DeleteById_InvalidId_Test() => Execute((notificationReposiotry, context) => notificationReposiotry
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(NotificationNotFound, InvalidGuid)));
    }
}