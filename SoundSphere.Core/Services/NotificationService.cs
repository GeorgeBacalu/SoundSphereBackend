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

        public Notification Save(Notification notification) => _notificationRepository.Save(notification);

        public Notification UpdateById(Notification notification, Guid id) => _notificationRepository.UpdateById(notification, id);

        public void DeleteById(Guid id) => _notificationRepository.DeleteById(id);
    }
}