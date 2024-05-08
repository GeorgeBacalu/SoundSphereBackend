using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        IList<Notification> FindAll();

        IList<Notification> FindAllPagination(NotificationPaginationRequest payload);

        Notification FindById(Guid id);

        Notification Save(Notification notification);

        Notification UpdateById(Notification notification, Guid id);

        void DeleteById(Guid id);

        void LinkNotificationToUser(Notification notification);
    }
}