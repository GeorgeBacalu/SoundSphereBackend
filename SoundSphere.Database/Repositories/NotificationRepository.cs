using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;

namespace SoundSphere.Database.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly SoundSphereContext _context;

        public NotificationRepository(SoundSphereContext context) => _context = context;

        public IList<Notification> FindAll() => _context.Notifications
            .Include(notification => notification.User)
            .ToList();

        public Notification FindById(Guid id) => _context.Notifications
            .Include(notification => notification.User)
            .FirstOrDefault(notification => notification.Id == id)
            ?? throw new ResourceNotFoundException($"Notification with id {id} not found!");

        public Notification Save(Notification notification)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();
            return notification;
        }

        public Notification UpdateById(Notification notification, Guid id)
        {
            Notification notificationToUpdate = FindById(id);
            DateTime SentAt = notificationToUpdate.SentAt;
            _context.Entry(notificationToUpdate).CurrentValues.SetValues(notification);
            notificationToUpdate.SentAt = SentAt;
            _context.SaveChanges();
            return notificationToUpdate;
        }

        public void DeleteById(Guid id)
        {
            Notification notificationToDelete = FindById(id);
            _context.Notifications.Remove(notificationToDelete);
            _context.SaveChanges();
        }

        public void LinkNotificationToUser(Notification notification)
        {
            User existingUser = _context.Users.Find(notification.User.Id);
            if (existingUser != null)
            {
                _context.Entry(existingUser).State = EntityState.Unchanged;
                notification.User = existingUser;
            }
        }
    }
}