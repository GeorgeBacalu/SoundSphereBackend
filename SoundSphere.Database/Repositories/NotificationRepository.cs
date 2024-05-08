using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Extensions;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;

namespace SoundSphere.Database.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly SoundSphereDbContext _context;

        public NotificationRepository(SoundSphereDbContext context) => _context = context;

        public IList<Notification> FindAll() => _context.Notifications
            .Include(notification => notification.User)
            .ToList();

        public IList<Notification> FindAllPagination(NotificationPaginationRequest payload) => _context.Notifications
            .Include(notification => notification.User)
            .Filter(payload)
            .Sort(payload)
            .Paginate(payload)
            .ToList();

        public Notification FindById(Guid id) => _context.Notifications
            .Include(notification => notification.User)
            .FirstOrDefault(notification => notification.Id == id)
            ?? throw new ResourceNotFoundException(string.Format(Constants.NotificationNotFound, id));

        public Notification Save(Notification notification)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();
            return notification;
        }

        public Notification UpdateById(Notification notification, Guid id)
        {
            Notification notificationToUpdate = FindById(id);
            notificationToUpdate.Type = notification.Type;
            notificationToUpdate.Message = notification.Message;
            notificationToUpdate.IsRead = notification.IsRead;
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