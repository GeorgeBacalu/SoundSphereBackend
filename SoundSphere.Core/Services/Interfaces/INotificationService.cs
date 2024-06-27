using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface INotificationService
    {
        IList<NotificationDto> GetAll(NotificationPaginationRequest? payload, Guid userId);

        NotificationDto GetById(Guid id);

        NotificationDto Send(NotificationDto notificationDto, Guid senderId, Guid receiverId);

        NotificationDto UpdateById(NotificationDto notificationDto, Guid id);

        NotificationDto DeleteById(Guid id);

        int CountUnread(Guid userId);

        void MarkAsRead(Guid notificationId, Guid userId);

        void MarkAllAsRead(Guid userId);
    }
}