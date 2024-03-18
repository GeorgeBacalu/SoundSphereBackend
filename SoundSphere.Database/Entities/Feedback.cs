namespace SoundSphere.Database.Entities
{
    public class Feedback
    {
        public Guid Id { get; set; }
        public User User { get; set; } = null!;
        public FeedbackType Type { get; set; }
        public string Message { get; set; } = null!;
        public DateTime SentAt { get; set; }
    }

    public enum FeedbackType { Issue, Optimization, Improvement }
}