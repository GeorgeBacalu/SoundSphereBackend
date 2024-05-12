using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Dtos.Request.Models;
using SoundSphere.Database.Entities;

namespace SoundSphere.Tests.Mocks
{
    public class NotificationMock
    {
        private NotificationMock() { }

        public static IList<Notification> GetMockedNotifications() => new List<Notification>
        {
            GetMockedNotification1(), GetMockedNotification2(), GetMockedNotification3(), GetMockedNotification4(), GetMockedNotification5(), GetMockedNotification6(), GetMockedNotification7(), GetMockedNotification8(), GetMockedNotification9(), GetMockedNotification10(),
            GetMockedNotification11(), GetMockedNotification12(), GetMockedNotification13(), GetMockedNotification14(), GetMockedNotification15(), GetMockedNotification16(), GetMockedNotification17(), GetMockedNotification18(), GetMockedNotification19(), GetMockedNotification20(),
            GetMockedNotification21(), GetMockedNotification22(), GetMockedNotification23(), GetMockedNotification24(), GetMockedNotification25(), GetMockedNotification26(), GetMockedNotification27(), GetMockedNotification28(), GetMockedNotification29(), GetMockedNotification30(),
            GetMockedNotification31(), GetMockedNotification32(), GetMockedNotification33(), GetMockedNotification34(), GetMockedNotification35(), GetMockedNotification36()
        };

        public static IList<NotificationDto> GetMockedNotificationDtos() => GetMockedNotifications().Select(ToDto).ToList();

        public static IList<Notification> GetMockedPaginatedNotifications() => new List<Notification> { GetMockedNotification31(), GetMockedNotification32(), GetMockedNotification33(), GetMockedNotification34(), GetMockedNotification35(), GetMockedNotification36() };

        public static IList<NotificationDto> GetMockedPaginatedNotificationDtos() => new List<NotificationDto> { GetMockedNotificationDto31(), GetMockedNotificationDto32(), GetMockedNotificationDto33(), GetMockedNotificationDto34(), GetMockedNotificationDto35(), GetMockedNotificationDto36() };

        public static NotificationPaginationRequest GetMockedPaginationRequest() => new NotificationPaginationRequest
        {
            SortCriteria = new Dictionary<NotificationSortCriterion, SortOrder> { { NotificationSortCriterion.BySendDate, SortOrder.Ascending }, { NotificationSortCriterion.ByMessage, SortOrder.Ascending }, { NotificationSortCriterion.ByUserName, SortOrder.Ascending } },
            SearchCriteria = new List<NotificationSearchCriterion> { NotificationSearchCriterion.BySendDateRange, NotificationSearchCriterion.ByMessage, NotificationSearchCriterion.ByUserName, NotificationSearchCriterion.ByIsRead },
            Message = "A",
            DateRange = new DateTimeRange { StartDate = new DateTime(1950, 1, 1), EndDate = new DateTime(2024, 1, 1) },
            UserName = "A",
            IsRead = false
        };

        public static Notification GetMockedNotification1() => new Notification
        {
            Id = Guid.Parse("7e221fa3-2c22-4573-bf21-cd1d6696b576"),
            User = UserMock.GetMockedUser1(),
            Type = NotificationType.Music,
            Message = "Your favorite artist just released a new album!",
            SentAt = new DateTime(2024, 5, 7, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification2() => new Notification
        {
            Id = Guid.Parse("1d23fa22-3455-407b-9371-c42d56001de7"),
            User = UserMock.GetMockedUser2(),
            Type = NotificationType.Music,
            Message = "Exclusive concert tickets available now for followers.",
            SentAt = new DateTime(2024, 5, 6, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification3() => new Notification
        {
            Id = Guid.Parse("a88fda83-356b-46b2-a1fd-041ef5b98270"),
            User = UserMock.GetMockedUser3(),
            Type = NotificationType.Music,
            Message = "Updated playlist based on your recent listens.",
            SentAt = new DateTime(2024, 5, 5, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification4() => new Notification
        {
            Id = Guid.Parse("8f6c5a67-369c-402b-88a7-0c0535e67411"),
            User = UserMock.GetMockedUser4(),
            Type = NotificationType.Social,
            Message = "New friend suggestions are available.",
            SentAt = new DateTime(2024, 5, 4, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification5() => new Notification
        {
            Id = Guid.Parse("2ba6fa59-3c3b-4599-8a2c-bf34b6aac71b"),
            User = UserMock.GetMockedUser5(),
            Type = NotificationType.Social,
            Message = "You have new followers waiting for your approval.",
            SentAt = new DateTime(2024, 5, 3, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification6() => new Notification
        {
            Id = Guid.Parse("49f1caba-5d7e-4f3b-9217-49fefa6c4f3b"),
            User = UserMock.GetMockedUser6(),
            Type = NotificationType.Social,
            Message = "Your post has been shared over 100 times!",
            SentAt = new DateTime(2024, 5, 2, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification7() => new Notification
        {
            Id = Guid.Parse("17de3d40-3f7a-45f1-a25e-2538c09e4d22"),
            User = UserMock.GetMockedUser7(),
            Type = NotificationType.Account,
            Message = "Security alert: New sign-in from an unrecognized device.",
            SentAt = new DateTime(2024, 5, 1, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification8() => new Notification
        {
            Id = Guid.Parse("6ec9550d-073c-4a17-8d1a-20a54dfd15f1"),
            User = UserMock.GetMockedUser8(),
            Type = NotificationType.Account,
            Message = "Your account details have been successfully updated.",
            SentAt = new DateTime(2024, 4, 30, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification9() => new Notification
        {
            Id = Guid.Parse("5f14e45d-e702-47ed-bb98-dd1f5d556f47"),
            User = UserMock.GetMockedUser9(),
            Type = NotificationType.Account,
            Message = "Password change confirmation.",
            SentAt = new DateTime(2024, 4, 29, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification10() => new Notification
        {
            Id = Guid.Parse("67b832f0-4883-4df4-9a89-70c8bdd6023c"),
            User = UserMock.GetMockedUser10(),
            Type = NotificationType.System,
            Message = "Scheduled maintenance will occur this Sunday at 3 AM.",
            SentAt = new DateTime(2024, 4, 28, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification11() => new Notification
        {
            Id = Guid.Parse("1dc0d6c1-33a2-49f4-87a2-a2df00df00a1"),
            User = UserMock.GetMockedUser1(),
            Type = NotificationType.System,
            Message = "System update completed successfully.",
            SentAt = new DateTime(2024, 4, 27, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification12() => new Notification
        {
            Id = Guid.Parse("22d0d3b1-33a3-49f5-87a3-a2df01df01a1"),
            User = UserMock.GetMockedUser2(),
            Type = NotificationType.System,
            Message = "New features available in the latest system update.",
            SentAt = new DateTime(2024, 4, 26, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification13() => new Notification
        {
            Id = Guid.Parse("33d0d4c2-34a4-4af6-88a4-a3ef02df02a2"),
            User = UserMock.GetMockedUser3(),
            Type = NotificationType.Music,
            Message = "Live session alert: Join your favorite band's Q&A today!",
            SentAt = new DateTime(2024, 4, 25, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification14() => new Notification
        {
            Id = Guid.Parse("44e0d5d3-35a5-4bf7-99a5-a4ff03df03a3"),
            User = UserMock.GetMockedUser4(),
            Type = NotificationType.Music,
            Message = "Your music subscription has been upgraded.",
            SentAt = new DateTime(2024, 4, 24, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification15() => new Notification
        {
            Id = Guid.Parse("55f0e6e4-36a6-4cf8-aaa6-b50004df04a4"),
            User = UserMock.GetMockedUser5(),
            Type = NotificationType.Music,
            Message = "Playlist update: New tracks added to your favorite genre.",
            SentAt = new DateTime(2024, 4, 23, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification16() => new Notification
        {
            Id = Guid.Parse("fe62d1cb-797b-47b8-b61d-2fb4bc55ce20"),
            User = UserMock.GetMockedUser6(),
            Type = NotificationType.Social,
            Message = "You have been tagged in 10 new photos.",
            SentAt = new DateTime(2024, 4, 22, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification17() => new Notification
        {
            Id = Guid.Parse("c8afa171-ee36-424e-a67d-de6d465861f7"),
            User = UserMock.GetMockedUser7(),
            Type = NotificationType.Social,
            Message = "Your story received over 200 views!",
            SentAt = new DateTime(2024, 4, 21, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification18() => new Notification
        {
            Id = Guid.Parse("984df627-a5b3-4541-9987-b54e9c1576c0"),
            User = UserMock.GetMockedUser8(),
            Type = NotificationType.Social,
            Message = "You have new comments to respond to.",
            SentAt = new DateTime(2024, 4, 20, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification19() => new Notification
        {
            Id = Guid.Parse("b4c57e93-4f26-4868-bfd6-f511fe63d7e3"),
            User = UserMock.GetMockedUser9(),
            Type = NotificationType.Account,
            Message = "Account notification: You've earned a new loyalty badge.",
            SentAt = new DateTime(2024, 4, 19, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification20() => new Notification
        {
            Id = Guid.Parse("376659d1-fa63-48b3-956b-f178e7750a04"),
            User = UserMock.GetMockedUser10(),
            Type = NotificationType.Account,
            Message = "Reminder: Your subscription renewal is due next week.",
            SentAt = new DateTime(2024, 4, 18, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification21() => new Notification
        {
            Id = Guid.Parse("31a74b33-5530-4fd9-9530-c5b31adddba9"),
            User = UserMock.GetMockedUser1(),
            Type = NotificationType.Account,
            Message = "Security alert: Unusual activity detected on your account.",
            SentAt = new DateTime(2024, 4, 17, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification22() => new Notification
        {
            Id = Guid.Parse("938732f5-3bff-426d-a838-657a577c87b1"),
            User = UserMock.GetMockedUser2(),
            Type = NotificationType.System,
            Message = "A new update is available for your app.",
            SentAt = new DateTime(2024, 4, 16, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification23() => new Notification
        {
            Id = Guid.Parse("b7954386-cacc-476c-bfad-01267d262f69"),
            User = UserMock.GetMockedUser3(),
            Type = NotificationType.System,
            Message = "Security notice: Please update your password.",
            SentAt = new DateTime(2024, 4, 15, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification24() => new Notification
        {
            Id = Guid.Parse("f562d55f-281b-4b48-92fa-e2476b3abf74"),
            User = UserMock.GetMockedUser4(),
            Type = NotificationType.System,
            Message = "Performance enhancements have been applied successfully.",
            SentAt = new DateTime(2024, 4, 14, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification25() => new Notification
        {
            Id = Guid.Parse("25dbcfe4-b6aa-44c0-aa2d-e95cca4df9ff"),
            User = UserMock.GetMockedUser5(),
            Type = NotificationType.Music,
            Message = "Check out the newest albums released this week!",
            SentAt = new DateTime(2024, 4, 13, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification26() => new Notification
        {
            Id = Guid.Parse("0f96131e-74c2-4acb-82a3-5564c1049086"),
            User = UserMock.GetMockedUser6(),
            Type = NotificationType.Music,
            Message = "Your favorite artist just released a new single!",
            SentAt = new DateTime(2024, 4, 12, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification27() => new Notification
        {
            Id = Guid.Parse("2ea232f6-b229-4846-b028-ecdaa214ef7f"),
            User = UserMock.GetMockedUser7(),
            Type = NotificationType.Music,
            Message = "Live concert alert: Your favorite band is touring near you! Get your tickets now.",
            SentAt = new DateTime(2024, 4, 11, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification28() => new Notification
        {
            Id = Guid.Parse("dc41888e-78f4-4fe4-a437-3fd27401d9ea"),
            User = UserMock.GetMockedUser8(),
            Type = NotificationType.Social,
            Message = "You have a new follower!",
            SentAt = new DateTime(2024, 4, 10, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification29() => new Notification
        {
            Id = Guid.Parse("707e8678-89f3-4670-9fd0-aba89da589ff"),
            User = UserMock.GetMockedUser9(),
            Type = NotificationType.Social,
            Message = "Your recent post has gone viral!",
            SentAt = new DateTime(2024, 4, 9, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification30() => new Notification
        {
            Id = Guid.Parse("ec4f53d0-5554-4ad9-a761-8f2811add21f"),
            User = UserMock.GetMockedUser10(),
            Type = NotificationType.Social,
            Message = "Thank you for your feedback on social features! We have implemented your suggestions.",
            SentAt = new DateTime(2024, 4, 8, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification31() => new Notification
        {
            Id = Guid.Parse("a492110f-a4ba-4f6e-9565-d0e902b91f36"),
            User = UserMock.GetMockedUser1(),
            Type = NotificationType.Account,
            Message = "Performance enhancements have been applied successfully.",
            SentAt = new DateTime(2024, 4, 7, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification32() => new Notification
        {
            Id = Guid.Parse("2b76e979-7def-44c8-9b3c-f6ac0253b86b"),
            User = UserMock.GetMockedUser2(),
            Type = NotificationType.Account,
            Message = "Scheduled maintenance will occur this Sunday.",
            SentAt = new DateTime(2024, 4, 6, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification33() => new Notification
        {
            Id = Guid.Parse("0d1d4015-a33d-4f2c-b947-39cf988b4b1b"),
            User = UserMock.GetMockedUser3(),
            Type = NotificationType.Account,
            Message = "Feature update: New tools available in your account management.",
            SentAt = new DateTime(2024, 4, 5, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification34() => new Notification
        {
            Id = Guid.Parse("c9b81833-485b-4d0e-91b9-dbc65a299c55"),
            User = UserMock.GetMockedUser4(),
            Type = NotificationType.System,
            Message = "Update: Your requested features are now live!",
            SentAt = new DateTime(2024, 4, 4, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification35() => new Notification
        {
            Id = Guid.Parse("f245bed0-ab5b-4470-8c0c-373ef2a10c3b"),
            User = UserMock.GetMockedUser5(),
            Type = NotificationType.System,
            Message = "Your app interface has been updated for a better experience.",
            SentAt = new DateTime(2024, 4, 3, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification36() => new Notification
        {
            Id = Guid.Parse("0960f07f-ce1f-485b-83f2-55f33a7bbd92"),
            User = UserMock.GetMockedUser6(),
            Type = NotificationType.System,
            Message = "We've upgraded your service to include new benefits.",
            SentAt = new DateTime(2024, 4, 2, 9, 0, 0),
            IsRead = false
        };

        public static Notification GetMockedNotification37() => new Notification
        {
            Id = Guid.Parse("4ad89c50-e58c-4d4c-aadf-1e80519a84d7"),
            User = UserMock.GetMockedUser7(),
            Type = NotificationType.System,
            Message = "The latest update includes several performance enhancements.",
            SentAt = new DateTime(2024, 4, 1, 9, 0, 0),
            IsRead = false
        };

        public static NotificationDto GetMockedNotificationDto1() => ToDto(GetMockedNotification1());

        public static NotificationDto GetMockedNotificationDto2() => ToDto(GetMockedNotification2());

        public static NotificationDto GetMockedNotificationDto3() => ToDto(GetMockedNotification3());

        public static NotificationDto GetMockedNotificationDto4() => ToDto(GetMockedNotification4());

        public static NotificationDto GetMockedNotificationDto5() => ToDto(GetMockedNotification5());

        public static NotificationDto GetMockedNotificationDto6() => ToDto(GetMockedNotification6());

        public static NotificationDto GetMockedNotificationDto7() => ToDto(GetMockedNotification7());

        public static NotificationDto GetMockedNotificationDto8() => ToDto(GetMockedNotification8());

        public static NotificationDto GetMockedNotificationDto9() => ToDto(GetMockedNotification9());

        public static NotificationDto GetMockedNotificationDto10() => ToDto(GetMockedNotification10());

        public static NotificationDto GetMockedNotificationDto11() => ToDto(GetMockedNotification11());

        public static NotificationDto GetMockedNotificationDto12() => ToDto(GetMockedNotification12());

        public static NotificationDto GetMockedNotificationDto13() => ToDto(GetMockedNotification13());

        public static NotificationDto GetMockedNotificationDto14() => ToDto(GetMockedNotification14());

        public static NotificationDto GetMockedNotificationDto15() => ToDto(GetMockedNotification15());

        public static NotificationDto GetMockedNotificationDto16() => ToDto(GetMockedNotification16());

        public static NotificationDto GetMockedNotificationDto17() => ToDto(GetMockedNotification17());

        public static NotificationDto GetMockedNotificationDto18() => ToDto(GetMockedNotification18());

        public static NotificationDto GetMockedNotificationDto19() => ToDto(GetMockedNotification19());

        public static NotificationDto GetMockedNotificationDto20() => ToDto(GetMockedNotification20());

        public static NotificationDto GetMockedNotificationDto21() => ToDto(GetMockedNotification21());

        public static NotificationDto GetMockedNotificationDto22() => ToDto(GetMockedNotification22());

        public static NotificationDto GetMockedNotificationDto23() => ToDto(GetMockedNotification23());

        public static NotificationDto GetMockedNotificationDto24() => ToDto(GetMockedNotification24());

        public static NotificationDto GetMockedNotificationDto25() => ToDto(GetMockedNotification25());

        public static NotificationDto GetMockedNotificationDto26() => ToDto(GetMockedNotification26());

        public static NotificationDto GetMockedNotificationDto27() => ToDto(GetMockedNotification27());

        public static NotificationDto GetMockedNotificationDto28() => ToDto(GetMockedNotification28());

        public static NotificationDto GetMockedNotificationDto29() => ToDto(GetMockedNotification29());

        public static NotificationDto GetMockedNotificationDto30() => ToDto(GetMockedNotification30());

        public static NotificationDto GetMockedNotificationDto31() => ToDto(GetMockedNotification31());

        public static NotificationDto GetMockedNotificationDto32() => ToDto(GetMockedNotification32());

        public static NotificationDto GetMockedNotificationDto33() => ToDto(GetMockedNotification33());

        public static NotificationDto GetMockedNotificationDto34() => ToDto(GetMockedNotification34());

        public static NotificationDto GetMockedNotificationDto35() => ToDto(GetMockedNotification35());

        public static NotificationDto GetMockedNotificationDto36() => ToDto(GetMockedNotification36());

        public static NotificationDto GetMockedNotificationDto37() => ToDto(GetMockedNotification37());

        private static NotificationDto ToDto(Notification notification) => new NotificationDto
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