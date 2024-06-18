namespace SoundSphere.Database.Entities
{
    public class Notification : BaseEntity
    {
        public Guid Id { get; set; }

        public Guid SenderId { get; set; }

        public Guid ReceiverId { get; set; }
        
        public User Sender { get; set; } = null!;

        public User Receiver { get; set; } = null!;
        
        public NotificationType Type { get; set; }
        
        public string Message { get; set; } = null!;
        
        public bool IsRead { get; set; } = false;

        public override bool Equals(object? obj) => obj is Notification notification &&
            Id.Equals(notification.Id) &&
            Sender.Equals(notification.Sender) &&
            Receiver.Equals(notification.Receiver) &&
            Type == notification.Type &&
            Message.Equals(notification.Message) &&
            IsRead == notification.IsRead &&
            CreatedAt.Equals(notification.CreatedAt) &&
            UpdatedAt.Equals(notification.UpdatedAt) &&
            DeletedAt.Equals(notification.DeletedAt);

        public override int GetHashCode() => HashCode.Combine(Id, Sender, Receiver, Type, Message, IsRead, HashCode.Combine(CreatedAt, UpdatedAt, DeletedAt));
    }

    public enum NotificationType { InvalidNotificationType, Music = 10, Social = 20, Account = 30, System = 40 }
}