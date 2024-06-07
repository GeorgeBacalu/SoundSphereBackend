using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface INotificationService
    {
        IList<NotificationDto> GetAll(NotificationPaginationRequest payload);

        NotificationDto GetById(Guid id);

        NotificationDto Add(NotificationDto notificationDto);

        NotificationDto UpdateById(NotificationDto notificationDto, Guid id);

        NotificationDto DeleteById(Guid id);
    }
}