using AutoMapper;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository, IUserRepository userRepository, IMapper mapper) => 
            (_notificationRepository, _userRepository, _mapper) = (notificationRepository, userRepository, mapper);

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

        public NotificationDto ConvertToDto(Notification notification) => _mapper.Map<NotificationDto>(notification);

        public Notification ConvertToEntity(NotificationDto notificationDto)
        {
            Notification notification = _mapper.Map<Notification>(notificationDto);
            notification.User = _userRepository.FindById(notificationDto.UserId);
            return notification;
        }
    }
}