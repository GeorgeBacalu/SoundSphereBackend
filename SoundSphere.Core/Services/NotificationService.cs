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

        public IList<NotificationDto> GetAll(NotificationPaginationRequest payload)
        {
            IList<NotificationDto> notificationDtos = _notificationRepository.GetAll(payload).ToDtos(_mapper);
            return notificationDtos;
        }

        public NotificationDto GetById(Guid id)
        {
            NotificationDto notificationDto = _notificationRepository.GetById(id).ToDto(_mapper);
            return notificationDto;
        }

        public NotificationDto Add(NotificationDto notificationDto)
        {
            Notification notificationToCreate = notificationDto.ToEntity(_userRepository, _mapper);
            _notificationRepository.LinkNotificationToUser(notificationToCreate);
            NotificationDto createdNotificationDto = _notificationRepository.Add(notificationToCreate).ToDto(_mapper);
            return createdNotificationDto;
        }

        public NotificationDto UpdateById(NotificationDto notificationDto, Guid id)
        {
            Notification notificationToUpdate = notificationDto.ToEntity(_userRepository, _mapper);
            NotificationDto updatedNotificationDto = _notificationRepository.UpdateById(notificationToUpdate, id).ToDto(_mapper);
            return updatedNotificationDto;
        }

        public NotificationDto DeleteById(Guid id)
        {
            NotificationDto deletedNotifcationDto = _notificationRepository.DeleteById(id).ToDto(_mapper);
            return deletedNotifcationDto;
        }
    }
}