using FluentAssertions;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class NotificationRepositoryIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        private readonly Notification _notification1 = NotificationMock.GetMockedNotification1();
        private readonly Notification _notification2 = NotificationMock.GetMockedNotification2();
        private readonly IList<Notification> _notifications = NotificationMock.GetMockedNotifications();

        public NotificationRepositoryIntegrationTest(DbFixture fixture) => _fixture = fixture;

        private void Execute(Action<NotificationRepository, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var notificationRepository = new NotificationRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Notifications.AddRange(_notifications);
            context.SaveChanges();
            action(notificationRepository, context);
        }

        [Fact] public void FindAll_Test() => Execute((notificationRepository, context) => notificationRepository.FindAll().Should().BeEquivalentTo(_notifications));

        [Fact] public void FindById_ValidId_Test() => Execute((notificationRepository, context) => notificationRepository.FindById(Constants.ValidNotificationGuid).Should().Be(_notification1));

        [Fact] public void FindById_InvalidId_Test() => Execute((notificationRepository, context) => notificationRepository
            .Invoking(repository => repository.FindById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.NotificationNotFound, Constants.InvalidGuid)));

        [Fact] public void Save_Test() => Execute((notificationRepository, context) =>
        {
            Notification newNotification = NotificationMock.GetMockedNotification3();
            notificationRepository.Save(newNotification);
            context.Notifications.Find(newNotification.Id).Should().BeEquivalentTo(newNotification, options => options.Excluding(notification => notification.SentAt));
        });

        [Fact] public void UodateById_ValidId_Test() => Execute((notificationRepository, context) =>
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
            notificationRepository.UpdateById(_notification2, Constants.ValidNotificationGuid);
            context.Notifications.Find(Constants.ValidNotificationGuid).Should().Be(updatedNotification);
        });

        [Fact] public void UpdateById_InvalidId_Test() => Execute((notificationRepository, cotext) => notificationRepository
            .Invoking(repository => repository.UpdateById(_notification2, Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.NotificationNotFound, Constants.InvalidGuid)));

        [Fact] public void DeleteById_ValidId_Test() => Execute((notificationRepository, context) =>
        {
            notificationRepository.DeleteById(Constants.ValidNotificationGuid);
            context.Notifications.Should().BeEquivalentTo(new List<Notification> { _notification2 });
        });

        [Fact] public void DeleteById_InvalidId_Test() => Execute((notificationReposiotry, context) => notificationReposiotry
            .Invoking(repository => repository.DeleteById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.NotificationNotFound, Constants.InvalidGuid)));
    }
}