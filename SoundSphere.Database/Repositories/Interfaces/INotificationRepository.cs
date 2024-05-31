using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        IList<Notification> GetAll();

        IList<Notification> GetAllPagination(NotificationPaginationRequest payload);

        Notification GetById(Guid id);

        Notification Add(Notification notification);

        Notification UpdateById(Notification notification, Guid id);

        void DeleteById(Guid id);

        void LinkNotificationToUser(Notification notification);
    }
}