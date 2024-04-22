using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos
{
    public class NotificationDto
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public NotificationType Type { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, ErrorMessage = "Message can't be longer than 500 characters")]
        public string Message { get; set; } = null!;
        
        public DateTime SentAt { get; set; }
        
        public bool IsRead { get; set; } = false;

        public override bool Equals(object? obj) => obj is NotificationDto notificationDto &&
            Id.Equals(notificationDto.Id) &&
            UserId.Equals(notificationDto.UserId) &&
            Type == notificationDto.Type &&
            Message.Equals(notificationDto.Message) &&
            SentAt.Equals(notificationDto.SentAt) &&
            IsRead == notificationDto.IsRead;

        public override int GetHashCode() => HashCode.Combine(Id, UserId, Type, Message, SentAt, IsRead);
    }
}