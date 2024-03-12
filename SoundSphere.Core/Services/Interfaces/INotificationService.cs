using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface INotificationService
    {
        IList<Notification> FindAll();

        Notification FindById(Guid id);

        Notification Save(Notification notification);

        Notification UpdateById(Notification notification, Guid id);

        void DeleteById(Guid id);
    }
}