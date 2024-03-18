using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public NotificationType Type { get; set; }
        public string Message { get; set; } = null!;
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; } = false;
    }
}