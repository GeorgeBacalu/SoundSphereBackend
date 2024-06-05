using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Common
{
    public class NotificationDto : BaseEntity
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

        public bool IsRead { get; set; } = false;

        public override bool Equals(object? obj) => obj is NotificationDto notificationDto &&
            Id.Equals(notificationDto.Id) &&
            UserId.Equals(notificationDto.UserId) &&
            Type == notificationDto.Type &&
            Message.Equals(notificationDto.Message) &&
            IsRead == notificationDto.IsRead &&
            CreatedAt.Equals(notificationDto.CreatedAt) &&
            UpdatedAt.Equals(notificationDto.UpdatedAt) &&
            DeletedAt.Equals(notificationDto.DeletedAt);

        public override int GetHashCode() => HashCode.Combine(Id, UserId, Type, Message, IsRead, HashCode.Combine(CreatedAt, UpdatedAt, DeletedAt));
    }
}