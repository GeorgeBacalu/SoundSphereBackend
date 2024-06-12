using AutoMapper;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Mappings
{
    public static class NotificationMappingExtensions
    {
        public static IList<NotificationDto> ToDtos(this IList<Notification> notifications, IMapper mapper)
        {
            IList<NotificationDto> notificationDtos = notifications.Select(notification => notification.ToDto(mapper)).ToList();
            return notificationDtos;
        }

        public static IList<Notification> ToEntities(this IList<NotificationDto> notificationDtos, IUserRepository userRepository, IMapper mapper)
        {
            IList<Notification> notifications = notificationDtos.Select(notificationDto => notificationDto.ToEntity(userRepository, mapper)).ToList();
            return notifications;
        }

        public static NotificationDto ToDto(this Notification notification, IMapper mapper)
        {
            NotificationDto notificationDto = mapper.Map<NotificationDto>(notification);
            return notificationDto;
        }

        public static Notification ToEntity(this NotificationDto notificationDto, IUserRepository userRepository, IMapper mapper)
        {
            Notification notification = mapper.Map<Notification>(notificationDto);
            notification.User = userRepository.GetById(notificationDto.UserId);
            return notification;
        }
    }
}