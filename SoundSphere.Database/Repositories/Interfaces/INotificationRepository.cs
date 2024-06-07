using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        IList<Notification> GetAll(NotificationPaginationRequest payload);

        Notification GetById(Guid id);

        Notification Add(Notification notification);

        Notification UpdateById(Notification notification, Guid id);

        Notification DeleteById(Guid id);

        void LinkNotificationToUser(Notification notification);
    }
}