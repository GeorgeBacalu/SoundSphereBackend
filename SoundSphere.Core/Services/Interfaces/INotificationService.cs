using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface INotificationService
    {
        IList<NotificationDto> FindAll();

        NotificationDto FindById(Guid id);

        NotificationDto Save(NotificationDto notificationDto);

        NotificationDto UpdateById(NotificationDto notificationDto, Guid id);

        void DeleteById(Guid id);

        IList<NotificationDto> ConvertToDtos(IList<Notification> notifications);

        IList<Notification> ConvertToEntities(IList<NotificationDto> notificationDtos);

        NotificationDto ConvertToDto(Notification notification);

        Notification ConvertToEntity(NotificationDto notificationDto);
    }
}