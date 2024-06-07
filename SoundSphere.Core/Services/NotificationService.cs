using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
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

        public IList<NotificationDto> GetAll(NotificationPaginationRequest payload) => _notificationRepository.GetAll(payload).ToDtos(_mapper);

        public NotificationDto GetById(Guid id) => _notificationRepository.GetById(id).ToDto(_mapper);

        public NotificationDto Add(NotificationDto notificationDto)
        {
            Notification notification = notificationDto.ToEntity(_userRepository, _mapper);
            _notificationRepository.LinkNotificationToUser(notification);
            return _notificationRepository.Add(notification).ToDto(_mapper);
        }

        public NotificationDto UpdateById(NotificationDto notificationDto, Guid id) => _notificationRepository.UpdateById(notificationDto.ToEntity(_userRepository, _mapper), id).ToDto(_mapper);

        public NotificationDto DeleteById(Guid id) => _notificationRepository.DeleteById(id).ToDto(_mapper);
    }
}