using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Dtos.Request.Models;
using SoundSphere.Database.Entities;
using static SoundSphere.Tests.Mocks.UserMock;

namespace SoundSphere.Tests.Mocks
{
    public class FeedbackMock
    {
        private FeedbackMock() { }

        public static IList<Feedback> GetMockedFeedbacks() => new List<Feedback>
        {
            GetMockedFeedback1(), GetMockedFeedback2(), GetMockedFeedback3(), GetMockedFeedback4(), GetMockedFeedback5(), GetMockedFeedback6(), GetMockedFeedback7(), GetMockedFeedback8(), GetMockedFeedback9(), GetMockedFeedback10(),
            GetMockedFeedback11(), GetMockedFeedback12(), GetMockedFeedback13(), GetMockedFeedback14(), GetMockedFeedback15(), GetMockedFeedback16(), GetMockedFeedback17(), GetMockedFeedback18(), GetMockedFeedback19(), GetMockedFeedback20(),
            GetMockedFeedback21(), GetMockedFeedback22(), GetMockedFeedback23(), GetMockedFeedback24(), GetMockedFeedback25(), GetMockedFeedback26(), GetMockedFeedback27(), GetMockedFeedback28(), GetMockedFeedback29(), GetMockedFeedback30(),
            GetMockedFeedback31(), GetMockedFeedback32(), GetMockedFeedback33(), GetMockedFeedback34(), GetMockedFeedback35(), GetMockedFeedback36()
        };

        public static IList<FeedbackDto> GetMockedFeedbackDtos() => GetMockedFeedbacks().Select(ToDto).ToList();

        public static IList<Feedback> GetMockedPaginatedFeedbacks() => GetMockedFeedbacks().Where(feedback => feedback.DeletedAt == null).Take(10).ToList();

        public static IList<FeedbackDto> GetMockedPaginatedFeedbackDtos() => GetMockedPaginatedFeedbacks().Select(ToDto).ToList();

        public static FeedbackPaginationRequest GetMockedFeedbacksPaginationRequest() => new FeedbackPaginationRequest(
            SortCriteria: new Dictionary<FeedbackSortCriterion, SortOrder> { { FeedbackSortCriterion.ByMessage, SortOrder.Ascending }, { FeedbackSortCriterion.ByUserName, SortOrder.Ascending } },
            SearchCriteria: new List<FeedbackSearchCriterion> { FeedbackSearchCriterion.ByCreateDateRange, FeedbackSearchCriterion.ByMessage, FeedbackSearchCriterion.ByUserName, FeedbackSearchCriterion.ByType },
            DateRange: new DateTimeRange(new DateTime(1950, 1, 1), new DateTime(2024, 5, 31)),
            Message: "A",
            UserName: "A",
            Type: FeedbackType.Issue);

        public static Feedback GetMockedFeedback1() => new Feedback
        {
            Id = Guid.Parse("83061e8c-3403-441a-8be5-867ed1f4a86b"),
            User = GetMockedUser1(),
            Type = FeedbackType.Optimization,
            Message = "Improve page load time for the dashboard.",
            CreatedAt = new DateTime(2024, 3, 1, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback2() => new Feedback
        {
            Id = Guid.Parse("bf823996-d2ce-4616-a6b2-f7347f05c6aa"),
            User = GetMockedUser2(),
            Type = FeedbackType.Optimization,
            Message = "Optimize search functionality for better results.",
            CreatedAt = new DateTime(2024, 3, 2, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback3() => new Feedback
        {
            Id = Guid.Parse("48c8b307-d4b5-4c58-a3ad-1a2affea3b36"),
            User = GetMockedUser3(),
            Type = FeedbackType.Optimization,
            Message = "Compress images to improve page load time.",
            CreatedAt = new DateTime(2024, 3, 3, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback4() => new Feedback
        {
            Id = Guid.Parse("7acaaed6-1bbb-4591-948c-ff5b6fbdad89"),
            User = GetMockedUser4(),
            Type = FeedbackType.Optimization,
            Message = "Implement lazy loading for faster initial load.",
            CreatedAt = new DateTime(2024, 3, 4, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback5() => new Feedback
        {
            Id = Guid.Parse("9dd5b2dc-3c7b-42b6-b6f4-241541087b5b"),
            User = GetMockedUser5(),
            Type = FeedbackType.Issue,
            Message = "App crashes when submitting a form.",
            CreatedAt = new DateTime(2024, 3, 5, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback6() => new Feedback
        {
            Id = Guid.Parse("25d06c66-b0cd-44a6-8ede-5a6003d58bcb"),
            User = GetMockedUser6(),
            Type = FeedbackType.Issue,
            Message = "Error message appears when uploading an image.",
            CreatedAt = new DateTime(2024, 3, 6, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback7() => new Feedback
        {
            Id = Guid.Parse("506f63c5-5a15-46cb-89c7-e034bbe18b7d"),
            User = GetMockedUser7(),
            Type = FeedbackType.Issue,
            Message = "Login issues after resetting the password.",
            CreatedAt = new DateTime(2024, 3, 7, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback8() => new Feedback
        {
            Id = Guid.Parse("e242f701-23aa-4b89-8eb6-ceaaf9c47f74"),
            User = GetMockedUser8(),
            Type = FeedbackType.Issue,
            Message = "Notifications not appearing on the mobile app.",
            CreatedAt = new DateTime(2024, 3, 8, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback9() => new Feedback
        {
            Id = Guid.Parse("1a0e8dd4-70fd-4644-9c70-18b1e3d13e3d"),
            User = GetMockedUser9(),
            Type = FeedbackType.Improvement,
            Message = "Add a dark mode for better user experience.",
            CreatedAt = new DateTime(2024, 3, 9, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback10() => new Feedback
        {
            Id = Guid.Parse("136183be-e45d-412f-a938-694db72b64f0"),
            User = GetMockedUser10(),
            Type = FeedbackType.Improvement,
            Message = "Add more filtering options in the search bar.",
            CreatedAt = new DateTime(2024, 3, 10, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback11() => new Feedback
        {
            Id = Guid.Parse("5729f7df-ac1f-4b0d-ab2f-e7d993ddeebe"),
            User = GetMockedUser1(),
            Type = FeedbackType.Improvement,
            Message = "Include an option to save items to a wishlist.",
            CreatedAt = new DateTime(2024, 3, 11, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback12() => new Feedback
        {
            Id = Guid.Parse("777cfbdd-5330-44ab-848d-e2d61b46dcf7"),
            User = GetMockedUser2(),
            Type = FeedbackType.Improvement,
            Message = "Allow users to customize their profile layout.",
            CreatedAt = new DateTime(2024, 3, 12, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback13() => new Feedback
        {
            Id = Guid.Parse("4e9c1ada-0cb2-4bfe-8d99-f82d06d39092"),
            User = GetMockedUser3(),
            Type = FeedbackType.Optimization,
            Message = "The loading time for the main page needs improvement.",
            CreatedAt = new DateTime(2024, 3, 13, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback14() => new Feedback
        {
            Id = Guid.Parse("367daa3c-3b36-49b7-af37-29ca8bf55c69"),
            User = GetMockedUser4(),
            Type = FeedbackType.Optimization,
            Message = "Consider using a CDN for faster content delivery.",
            CreatedAt = new DateTime(2024, 3, 14, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback15() => new Feedback
        {
            Id = Guid.Parse("d901d2a3-3a96-496d-8f80-33d43a3b30da"),
            User = GetMockedUser5(),
            Type = FeedbackType.Optimization,
            Message = "Reduce the memory usage on mobile devices.",
            CreatedAt = new DateTime(2024, 3, 15, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback16() => new Feedback
        {
            Id = Guid.Parse("405b71fc-cd19-442b-b70b-c6fc1ef28eac"),
            User = GetMockedUser6(),
            Type = FeedbackType.Optimization,
            Message = "Improve server response time by optimizing queries.",
            CreatedAt = new DateTime(2024, 3, 16, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback17() => new Feedback
        {
            Id = Guid.Parse("ccef4154-7a43-45d8-a7f0-d581a4dbd680"),
            User = GetMockedUser7(),
            Type = FeedbackType.Issue,
            Message = "The application crashed on startup.",
            CreatedAt = new DateTime(2024, 3, 17, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback18() => new Feedback
        {
            Id = Guid.Parse("1fb89f40-7b40-4438-8be5-0d770658ffe3"),
            User = GetMockedUser8(),
            Type = FeedbackType.Issue,
            Message = "There is a bug in the payment gateway integration.",
            CreatedAt = new DateTime(2024, 3, 18, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback19() => new Feedback
        {
            Id = Guid.Parse("15b305b2-f585-4cca-8039-5d76ad537104"),
            User = GetMockedUser9(),
            Type = FeedbackType.Issue,
            Message = "Email notifications are sometimes delayed.",
            CreatedAt = new DateTime(2024, 3, 19, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback20() => new Feedback
        {
            Id = Guid.Parse("10ff528a-0490-4eaa-a80f-d04c279c5a63"),
            User = GetMockedUser10(),
            Type = FeedbackType.Issue,
            Message = "Fix alignment issues on the settings page.",
            CreatedAt = new DateTime(2024, 3, 20, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback21() => new Feedback
        {
            Id = Guid.Parse("819ece95-0f7e-4ad6-aa95-4dcafff97345"),
            User = GetMockedUser1(),
            Type = FeedbackType.Improvement,
            Message = "Add more analytics features for users.",
            CreatedAt = new DateTime(2024, 3, 21, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback22() => new Feedback
        {
            Id = Guid.Parse("67583c71-93f3-422c-8f8d-a00ca634373b"),
            User = GetMockedUser2(),
            Type = FeedbackType.Improvement,
            Message = "Implement dark mode in the user interface.",
            CreatedAt = new DateTime(2024, 3, 22, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback23() => new Feedback
        {
            Id = Guid.Parse("31f51cb2-a487-457b-9ad3-507e48a5f2b1"),
            User = GetMockedUser3(),
            Type = FeedbackType.Improvement,
            Message = "Update the search functionality to include filters.",
            CreatedAt = new DateTime(2024, 3, 23, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback24() => new Feedback
        {
            Id = Guid.Parse("871103f4-b5c9-4788-9f41-265c72c41b9f"),
            User = GetMockedUser4(),
            Type = FeedbackType.Improvement,
            Message = "Add more detailed logs for debugging purposes.",
            CreatedAt = new DateTime(2024, 3, 24, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback25() => new Feedback
        {
            Id = Guid.Parse("6abc0b49-b2de-46a5-bfa6-5b05a9573e73"),
            User = GetMockedUser5(),
            Type = FeedbackType.Optimization,
            Message = "Database cleanup operations should be scheduled.",
            CreatedAt = new DateTime(2024, 3, 25, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback26() => new Feedback
        {
            Id = Guid.Parse("ad8e35cb-bf09-48e1-8257-bb866c84821d"),
            User = GetMockedUser6(),
            Type = FeedbackType.Optimization,
            Message = "Use a more robust authentication method.",
            CreatedAt = new DateTime(2024, 3, 26, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback27() => new Feedback
        {
            Id = Guid.Parse("70fec855-d30a-4a8d-9db5-1e8c40afbd27"),
            User = GetMockedUser7(),
            Type = FeedbackType.Optimization,
            Message = "Optimize data handling to prevent memory leaks.",
            CreatedAt = new DateTime(2024, 3, 27, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback28() => new Feedback
        {
            Id = Guid.Parse("55973ad0-5169-48c5-bc79-e2f24a03db36"),
            User = GetMockedUser8(),
            Type = FeedbackType.Optimization,
            Message = "Improve email delivery system reliability.",
            CreatedAt = new DateTime(2024, 3, 28, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback29() => new Feedback
        {
            Id = Guid.Parse("5594ecd8-b0ca-49b9-8e96-ea4980b10178"),
            User = GetMockedUser9(),
            Type = FeedbackType.Issue,
            Message = "Users are experiencing logouts randomly.",
            CreatedAt = new DateTime(2024, 3, 29, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback30() => new Feedback
        {
            Id = Guid.Parse("4c9684b7-ad0a-475b-a499-d66dccde6730"),
            User = GetMockedUser10(),
            Type = FeedbackType.Issue,
            Message = "The report generation feature fails to load.",
            CreatedAt = new DateTime(2024, 3, 30, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback31() => new Feedback
        {
            Id = Guid.Parse("9e592479-4c9f-444f-a1c3-f888f38ab008"),
            User = GetMockedUser1(),
            Type = FeedbackType.Issue,
            Message = "Notification sounds are not working on some devices.",
            CreatedAt = new DateTime(2024, 3, 31, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetMockedFeedback32() => new Feedback
        {
            Id = Guid.Parse("2abf1f0c-6201-4eeb-96df-810a7fcdde47"),
            User = GetMockedUser2(),
            Type = FeedbackType.Issue,
            Message = "Application crashes when uploading large files.",
            CreatedAt = new DateTime(2024, 4, 1, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = new DateTime(2024, 4, 1, 12, 0, 0)
        };

        public static Feedback GetMockedFeedback33() => new Feedback
        {
            Id = Guid.Parse("fc1ae2d2-1234-4b5b-9a1e-39df1b636f38"),
            User = GetMockedUser3(),
            Type = FeedbackType.Improvement,
            Message = "Enhance the search function with advanced filters.",
            CreatedAt = new DateTime(2024, 4, 2, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = new DateTime(2024, 4, 2, 12, 0, 0)
        };

        public static Feedback GetMockedFeedback34() => new Feedback
        {
            Id = Guid.Parse("1e790ccd-7777-4bd7-a721-779f3b718b04"),
            User = GetMockedUser4(),
            Type = FeedbackType.Improvement,
            Message = "Add multi-language support to the interface.",
            CreatedAt = new DateTime(2024, 4, 3, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = new DateTime(2024, 4, 3, 12, 0, 0)
        };

        public static Feedback GetMockedFeedback35() => new Feedback
        {
            Id = Guid.Parse("bc5b2a5c-3333-47b4-bdac-4b504f1f7473"),
            User = GetMockedUser5(),
            Type = FeedbackType.Improvement,
            Message = "Provide customizable dashboard widgets.",
            CreatedAt = new DateTime(2024, 4, 4, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = new DateTime(2024, 4, 4, 12, 0, 0)
        };

        public static Feedback GetMockedFeedback36() => new Feedback
        {
            Id = Guid.Parse("c20369a1-8080-4f0b-9a3d-07af2fde560b"),
            User = GetMockedUser6(),
            Type = FeedbackType.Improvement,
            Message = "Implement two-factor authentication for enhanced security.",
            CreatedAt = new DateTime(2024, 4, 5, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = new DateTime(2024, 4, 5, 12, 0, 0)
        };

        public static Feedback GetMockedFeedback37() => new Feedback
        {
            Id = Guid.Parse("84ab304c-c3ce-4db7-b451-77fd593f2cfe"),
            User = GetMockedUser7(),
            Type = FeedbackType.Improvement,
            Message = "Increase file storage limits for users on all plans.",
            CreatedAt = new DateTime(2024, 4, 6, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static FeedbackDto GetMockedFeedbackDto1() => ToDto(GetMockedFeedback1());

        public static FeedbackDto GetMockedFeedbackDto2() => ToDto(GetMockedFeedback2());

        public static FeedbackDto GetMockedFeedbackDto3() => ToDto(GetMockedFeedback3());

        public static FeedbackDto GetMockedFeedbackDto4() => ToDto(GetMockedFeedback4());

        public static FeedbackDto GetMockedFeedbackDto5() => ToDto(GetMockedFeedback5());

        public static FeedbackDto GetMockedFeedbackDto6() => ToDto(GetMockedFeedback6());

        public static FeedbackDto GetMockedFeedbackDto7() => ToDto(GetMockedFeedback7());

        public static FeedbackDto GetMockedFeedbackDto8() => ToDto(GetMockedFeedback8());

        public static FeedbackDto GetMockedFeedbackDto9() => ToDto(GetMockedFeedback9());

        public static FeedbackDto GetMockedFeedbackDto10() => ToDto(GetMockedFeedback10());

        public static FeedbackDto GetMockedFeedbackDto11() => ToDto(GetMockedFeedback11());

        public static FeedbackDto GetMockedFeedbackDto12() => ToDto(GetMockedFeedback12());

        public static FeedbackDto GetMockedFeedbackDto13() => ToDto(GetMockedFeedback13());

        public static FeedbackDto GetMockedFeedbackDto14() => ToDto(GetMockedFeedback14());

        public static FeedbackDto GetMockedFeedbackDto15() => ToDto(GetMockedFeedback15());

        public static FeedbackDto GetMockedFeedbackDto16() => ToDto(GetMockedFeedback16());

        public static FeedbackDto GetMockedFeedbackDto17() => ToDto(GetMockedFeedback17());

        public static FeedbackDto GetMockedFeedbackDto18() => ToDto(GetMockedFeedback18());

        public static FeedbackDto GetMockedFeedbackDto19() => ToDto(GetMockedFeedback19());

        public static FeedbackDto GetMockedFeedbackDto20() => ToDto(GetMockedFeedback20());

        public static FeedbackDto GetMockedFeedbackDto21() => ToDto(GetMockedFeedback21());

        public static FeedbackDto GetMockedFeedbackDto22() => ToDto(GetMockedFeedback22());

        public static FeedbackDto GetMockedFeedbackDto23() => ToDto(GetMockedFeedback23());

        public static FeedbackDto GetMockedFeedbackDto24() => ToDto(GetMockedFeedback24());

        public static FeedbackDto GetMockedFeedbackDto25() => ToDto(GetMockedFeedback25());

        public static FeedbackDto GetMockedFeedbackDto26() => ToDto(GetMockedFeedback26());

        public static FeedbackDto GetMockedFeedbackDto27() => ToDto(GetMockedFeedback27());

        public static FeedbackDto GetMockedFeedbackDto28() => ToDto(GetMockedFeedback28());

        public static FeedbackDto GetMockedFeedbackDto29() => ToDto(GetMockedFeedback29());

        public static FeedbackDto GetMockedFeedbackDto30() => ToDto(GetMockedFeedback30());

        public static FeedbackDto GetMockedFeedbackDto31() => ToDto(GetMockedFeedback31());

        public static FeedbackDto GetMockedFeedbackDto32() => ToDto(GetMockedFeedback32());

        public static FeedbackDto GetMockedFeedbackDto33() => ToDto(GetMockedFeedback33());

        public static FeedbackDto GetMockedFeedbackDto34() => ToDto(GetMockedFeedback34());

        public static FeedbackDto GetMockedFeedbackDto35() => ToDto(GetMockedFeedback35());

        public static FeedbackDto GetMockedFeedbackDto36() => ToDto(GetMockedFeedback36());

        public static FeedbackDto GetMockedFeedbackDto37() => ToDto(GetMockedFeedback37());

        private static FeedbackDto ToDto(Feedback feedback) => new FeedbackDto
        {
            Id = feedback.Id,
            UserId = feedback.User.Id,
            Type = feedback.Type,
            Message = feedback.Message,
            CreatedAt = feedback.CreatedAt,
            UpdatedAt = feedback.UpdatedAt,
            DeletedAt = feedback.DeletedAt
        };
    }
}