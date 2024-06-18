using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Common
{
    public class NotificationDto : BaseEntity
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Sender Id is required")]
        public Guid SenderId { get; set; }

        [Required(ErrorMessage = "Receiver Id is required")]
        public Guid ReceiverId { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public NotificationType Type { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, ErrorMessage = "Message can't be longer than 500 characters")]
        public string Message { get; set; } = null!;

        public bool IsRead { get; set; } = false;

        public override bool Equals(object? obj) => obj is NotificationDto notificationDto &&
            Id.Equals(notificationDto.Id) &&
            SenderId.Equals(notificationDto.SenderId) &&
            ReceiverId.Equals(notificationDto.ReceiverId) &&
            Type == notificationDto.Type &&
            Message.Equals(notificationDto.Message) &&
            IsRead == notificationDto.IsRead &&
            CreatedAt.Equals(notificationDto.CreatedAt) &&
            UpdatedAt.Equals(notificationDto.UpdatedAt) &&
            DeletedAt.Equals(notificationDto.DeletedAt);

        public override int GetHashCode() => HashCode.Combine(Id, SenderId, ReceiverId, Type, Message, IsRead, HashCode.Combine(CreatedAt, UpdatedAt, DeletedAt));
    }
}