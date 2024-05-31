using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface INotificationService
    {
        IList<NotificationDto> GetAll();

        IList<NotificationDto> GetAllPagination(NotificationPaginationRequest payload);

        NotificationDto GetById(Guid id);

        NotificationDto Add(NotificationDto notificationDto);

        NotificationDto UpdateById(NotificationDto notificationDto, Guid id);

        void DeleteById(Guid id);
    }
}