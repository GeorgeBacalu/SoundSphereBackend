namespace SoundSphere.Database.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public User User { get; set; } = null!; // ManyToOne with User
        public NotificationType Type { get; set; }
        public string Message { get; set; } = null!;
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; } = false;
    }

    public enum NotificationType { Music, Social, Account, System }
}