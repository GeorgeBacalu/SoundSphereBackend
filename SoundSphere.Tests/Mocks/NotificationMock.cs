using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;

namespace SoundSphere.Tests.Mocks
{
    public class NotificationMock
    {
        private NotificationMock() { }

        public static IList<Notification> GetMockedNotifications() => new List<Notification> { GetMockedNotification1(), GetMockedNotification2() };

        public static IList<NotificationDto> GetMockedNotificationDtos() => new List<NotificationDto> { GetMockedNotificationDto1(), GetMockedNotificationDto2() };

        public static Notification GetMockedNotification1() => new Notification
        {
            Id = Guid.Parse("39eb6228-682d-4418-85f4-9aaf6e4c698f"),
            User = UserMock.GetMockedUser1(),
            Type = NotificationType.Music,
            Message = "notification_message1",
            SentAt = new DateTime(2024, 1, 1),
            IsRead = false
        };

        public static Notification GetMockedNotification2() => new Notification
        {
            Id = Guid.Parse("a1201396-af0f-4e9b-b108-caac99afef44"),
            User = UserMock.GetMockedUser2(),
            Type = NotificationType.Social,
            Message = "notification_message2",
            SentAt = new DateTime(2024, 1, 2),
            IsRead = false
        };

        public static Notification GetMockedNotification3() => new Notification
        {
            Id = Guid.Parse("5e53bb96-cc40-48c6-96a4-02d31df5cbab"),
            User = UserMock.GetMockedUser3(),
            Type = NotificationType.System,
            Message = "notificaiton_message3",
            SentAt = new DateTime(2024, 1, 3),
            IsRead = false
        };

        public static NotificationDto GetMockedNotificationDto1() => new NotificationDto
        {
            Id = Guid.Parse("39eb6228-682d-4418-85f4-9aaf6e4c698f"),
            UserId = Guid.Parse("0a9e546f-38b4-4dbf-a482-24a82169890e"),
            Type = NotificationType.Music,
            Message = "notification_message1",
            SentAt = new DateTime(2024, 1, 1),
            IsRead = false
        };

        public static NotificationDto GetMockedNotificationDto2() => new NotificationDto
        {
            Id = Guid.Parse("a1201396-af0f-4e9b-b108-caac99afef44"),
            UserId = Guid.Parse("31a088bd-6fe8-4226-bd03-f4af698abe83"),
            Type = NotificationType.Social,
            Message = "notification_message2",
            SentAt = new DateTime(2024, 1, 2),
            IsRead = false
        };

        public static NotificationDto GetMockedNotificationDto3() => new NotificationDto
        {
            Id = Guid.Parse("5e53bb96-cc40-48c6-96a4-02d31df5cbab"),
            UserId = Guid.Parse("b3692c1c-384a-47ef-a258-106bceb73f0c"),
            Type = NotificationType.System,
            Message = "notificaiton_message3",
            SentAt = new DateTime(2024, 1, 3),
            IsRead = false
        };
    }
}