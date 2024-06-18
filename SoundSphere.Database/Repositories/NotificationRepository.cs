using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Extensions;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;

namespace SoundSphere.Database.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly SoundSphereDbContext _context;

        public NotificationRepository(SoundSphereDbContext context) => _context = context;

        public IList<Notification> GetAll(NotificationPaginationRequest payload, Guid userId)
        {
            IList<Notification> notifications = _context.Notifications
                .Include(notification => notification.Sender)
                .Include(notification => notification.Receiver)
                .Where(notification => notification.DeletedAt == null && notification.Receiver.Id.Equals(userId))
                .Filter(payload)
                .Sort(payload)
                .Paginate(payload)
                .ToList();
            return notifications;
        }

        public Notification GetById(Guid id)
        {
            Notification? notification = _context.Notifications
                .Include(notification => notification.Sender)
                .Include(notification => notification.Receiver)
                .Where(notification => notification.DeletedAt == null)
                .FirstOrDefault(notification => notification.Id == id);
            if (notification == null)
                throw new ResourceNotFoundException(string.Format(NotificationNotFound, id));
            return notification;
        }

        public Notification Add(Notification notification)
        {
            if (notification.Id == Guid.Empty)
                notification.Id = Guid.NewGuid();
            notification.CreatedAt = DateTime.Now;
            notification.IsRead = false;
            _context.Notifications.Add(notification);
            _context.SaveChanges();
            return notification;
        }

        public Notification UpdateById(Notification notification, Guid id)
        {
            Notification notificationToUpdate = GetById(id);
            notificationToUpdate.Type = notification.Type;
            notificationToUpdate.Message = notification.Message;
            notificationToUpdate.IsRead = notification.IsRead;
            if (_context.Entry(notificationToUpdate).State == EntityState.Modified)
                notificationToUpdate.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return notificationToUpdate;
        }

        public Notification DeleteById(Guid id)
        {
            Notification notificationToDelete = GetById(id);
            notificationToDelete.DeletedAt = DateTime.Now;
            _context.SaveChanges();
            return notificationToDelete;
        }

        public void LinkNotificationToSender(Notification notification)
        {
            User? existingSender = _context.Users.Find(notification.Sender.Id);
            if (existingSender != null)
            {
                _context.Entry(existingSender).State = EntityState.Unchanged;
                notification.Sender = existingSender;
            }
        }

        public void LinkNotificationToReceiver(Notification notification)
        {
            User? existingReceiver = _context.Users.Find(notification.Receiver.Id);
            if (existingReceiver != null)
            {
                _context.Entry(existingReceiver).State = EntityState.Unchanged;
                notification.Receiver = existingReceiver;
            }
        }
    }
}