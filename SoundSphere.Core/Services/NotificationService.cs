using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository) => _notificationRepository = notificationRepository;

        public IList<Notification> FindAll() => _notificationRepository.FindAll();

        public Notification FindById(Guid id) => _notificationRepository.FindById(id);

        public Notification Save(Notification notification)
        {
            if (notification == null) throw new Exception("Can't persist null notification to DB!");
            if (notification.Id == Guid.Empty) notification.Id = Guid.NewGuid();
            notification.SentAt = DateTime.Now;
            notification.IsRead = false;

            _notificationRepository.LinkNotificationToUser(notification);

            return _notificationRepository.Save(notification);
        }

        public Notification UpdateById(Notification notification, Guid id)
        {
            if (notification == null) throw new Exception("Can't persist null notification to DB!");
            return _notificationRepository.UpdateById(notification, id);
        }

        public void DeleteById(Guid id) => _notificationRepository.DeleteById(id);
    }
}