namespace SoundSphere.Database.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        
        public User User { get; set; } = null!;
        
        public NotificationType Type { get; set; }
        
        public string Message { get; set; } = null!;
        
        public DateTime SentAt { get; set; }
        
        public bool IsRead { get; set; } = false;

        public override bool Equals(object? obj) => obj is Notification notification &&
            Id.Equals(notification.Id) &&
            User.Equals(notification.User) &&
            Type == notification.Type &&
            Message.Equals(notification.Message) &&
            SentAt.Equals(notification.SentAt) &&
            IsRead == notification.IsRead;

        public override int GetHashCode() => HashCode.Combine(Id, User, Type, Message, SentAt, IsRead);
    }

    public enum NotificationType { Music, Social, Account, System }
}