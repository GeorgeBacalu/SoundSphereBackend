using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly SoundSphereContext _context;

        public NotificationRepository(SoundSphereContext context) => _context = context;

        public IList<Notification> FindAll() => _context.Notifications.ToList();

        public Notification FindById(Guid id) => _context.Notifications.Find(id) ?? throw new Exception($"Notification with id {id} not found!");

        public Notification Save(Notification notification)
        {
            if (notification == null) throw new Exception("Can't persist null notification to DB!");
            if (notification.Id == Guid.Empty) notification.Id = Guid.NewGuid();
            notification.SentAt = DateTime.Now;
            notification.IsRead = false;
            _context.Notifications.Add(notification);
            _context.SaveChanges();
            return notification;
        }

        public Notification UpdateById(Notification notification, Guid id)
        {
            if (notification == null) throw new Exception("Can't persist null notification to DB!");
            Notification notificationToUpdate = FindById(id);
            _context.Entry(notificationToUpdate).CurrentValues.SetValues(notification);
            _context.SaveChanges();
            return notificationToUpdate;
        }

        public void DeleteById(Guid id)
        {
            Notification notificationToDelete = FindById(id);
            _context.Notifications.Remove(notificationToDelete);
            _context.SaveChanges();
        }
    }
}