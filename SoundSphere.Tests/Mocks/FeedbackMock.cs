using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;

namespace SoundSphere.Tests.Mocks
{
    public class FeedbackMock
    {
        private FeedbackMock() { }

        public static IList<Feedback> GetMockedFeedbacks() => new List<Feedback> { GetMockedFeedback1(), GetMockedFeedback2() };

        public static IList<FeedbackDto> GetMockedFeedbackDtos() => new List<FeedbackDto> { GetMockedFeedbackDto1(), GetMockedFeedbackDto2() };

        public static Feedback GetMockedFeedback1() => new Feedback
        {
            Id = Guid.Parse("83061e8c-3403-441a-8be5-867ed1f4a86b"),
            User = UserMock.GetMockedUser1(),
            Type = FeedbackType.Issue,
            Message = "feedback_message1",
            SentAt = new DateTime(2024, 1, 1),
        };

        public static Feedback GetMockedFeedback2() => new Feedback
        {
            Id = Guid.Parse("bf823996-d2ce-4616-a6b2-f7347f05c6aa"),
            User = UserMock.GetMockedUser2(),
            Type = FeedbackType.Optimization,
            Message = "feedback_message2",
            SentAt = new DateTime(2024, 1, 2),
        };

        public static FeedbackDto GetMockedFeedbackDto1() => new FeedbackDto
        {
            Id = Guid.Parse("83061e8c-3403-441a-8be5-867ed1f4a86b"),
            UserId = Guid.Parse("0a9e546f-38b4-4dbf-a482-24a82169890e"),
            Type = FeedbackType.Issue,
            Message = "feedback_message1",
            SentAt = new DateTime(2024, 1, 1),
        };

        public static FeedbackDto GetMockedFeedbackDto2() => new FeedbackDto
        {
            Id = Guid.Parse("bf823996-d2ce-4616-a6b2-f7347f05c6aa"),
            UserId = Guid.Parse("31a088bd-6fe8-4226-bd03-f4af698abe83"),
            Type = FeedbackType.Optimization,
            Message = "feedback_message2",
            SentAt = new DateTime(2024, 1, 2),
        };
    }
}