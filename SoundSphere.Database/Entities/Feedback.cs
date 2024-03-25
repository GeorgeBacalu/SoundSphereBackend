namespace SoundSphere.Database.Entities
{
    public class Feedback
    {
        public Guid Id { get; set; }
        
        public User User { get; set; } = null!;
        
        public FeedbackType Type { get; set; }
        
        public string Message { get; set; } = null!;
        
        public DateTime SentAt { get; set; }

        public override bool Equals(object? obj) => obj is Feedback feedback &&
            Id.Equals(feedback.Id) &&
            User.Equals(feedback.User) &&
            Type == feedback.Type &&
            Message == feedback.Message &&
            SentAt == feedback.SentAt;

        public override int GetHashCode() => HashCode.Combine(Id, User, Type, Message, SentAt);
    }

    public enum FeedbackType { Issue, Optimization, Improvement }
}