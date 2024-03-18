using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;

        public NotificationService(INotificationRepository notificationRepository, IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
        }

        public IList<NotificationDto> FindAll() => ConvertToDtos(_notificationRepository.FindAll());

        public NotificationDto FindById(Guid id) => ConvertToDto(_notificationRepository.FindById(id));

        public NotificationDto Save(NotificationDto notificationDto)
        {
            Notification notification = ConvertToEntity(notificationDto);
            if (notification.Id == Guid.Empty) notification.Id = Guid.NewGuid();
            notification.SentAt = DateTime.Now;
            notification.IsRead = false;
            _notificationRepository.LinkNotificationToUser(notification);
            return ConvertToDto(_notificationRepository.Save(notification));
        }

        public NotificationDto UpdateById(NotificationDto notificationDto, Guid id) => ConvertToDto(_notificationRepository.UpdateById(ConvertToEntity(notificationDto), id));

        public void DeleteById(Guid id) => _notificationRepository.DeleteById(id);

        public IList<NotificationDto> ConvertToDtos(IList<Notification> notifications) => notifications.Select(ConvertToDto).ToList();

        public IList<Notification> ConvertToEntities(IList<NotificationDto> notificationDtos) => notificationDtos.Select(ConvertToEntity).ToList();

        public NotificationDto ConvertToDto(Notification notification) => new NotificationDto
        {
            Id = notification.Id,
            UserId = notification.User.Id,
            Type = notification.Type,
            Message = notification.Message,
            SentAt = notification.SentAt,
            IsRead = notification.IsRead
        };

        public Notification ConvertToEntity(NotificationDto notificationDto) => new Notification
        {
            Id = notificationDto.Id,
            User = _userRepository.FindById(notificationDto.UserId),
            Type = notificationDto.Type,
            Message = notificationDto.Message,
            SentAt = notificationDto.SentAt,
            IsRead = notificationDto.IsRead
        };
    }
}