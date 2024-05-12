using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository, IUserRepository userRepository, IMapper mapper) => (_notificationRepository, _userRepository, _mapper) = (notificationRepository, userRepository, mapper);

        public IList<NotificationDto> FindAll() => _notificationRepository.FindAll().ToDtos(_mapper);

        public IList<NotificationDto> FindAllPagination(NotificationPaginationRequest payload) => _notificationRepository.FindAllPagination(payload).ToDtos(_mapper);

        public NotificationDto FindById(Guid id) => _notificationRepository.FindById(id).ToDto(_mapper);

        public NotificationDto Save(NotificationDto notificationDto)
        {
            Notification notification = notificationDto.ToEntity(_userRepository, _mapper);
            if (notification.Id == Guid.Empty) notification.Id = Guid.NewGuid();
            notification.SentAt = DateTime.Now;
            notification.IsRead = false;
            _notificationRepository.LinkNotificationToUser(notification);
            return _notificationRepository.Save(notification).ToDto(_mapper);
        }

        public NotificationDto UpdateById(NotificationDto notificationDto, Guid id) => _notificationRepository.UpdateById(notificationDto.ToEntity(_userRepository, _mapper), id).ToDto(_mapper);

        public void DeleteById(Guid id) => _notificationRepository.DeleteById(id);
    }
}