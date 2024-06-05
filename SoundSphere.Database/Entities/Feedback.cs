namespace SoundSphere.Database.Entities
{
    public class Feedback : BaseEntity
    {
        public Guid Id { get; set; }
        
        public User User { get; set; } = null!;
        
        public FeedbackType Type { get; set; }
        
        public string Message { get; set; } = null!;

        public override bool Equals(object? obj) => obj is Feedback feedback &&
            Id.Equals(feedback.Id) &&
            User.Equals(feedback.User) &&
            Type == feedback.Type &&
            Message.Equals(feedback.Message) &&
            CreatedAt.Equals(feedback.CreatedAt) &&
            UpdatedAt.Equals(feedback.UpdatedAt) &&
            DeletedAt.Equals(feedback.DeletedAt);

        public override int GetHashCode() => HashCode.Combine(Id, User, Type, Message, HashCode.Combine(CreatedAt, UpdatedAt, DeletedAt));
    }

    public enum FeedbackType { Issue, Optimization, Improvement }
}