using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface INotificationService
    {
        IList<NotificationDto> FindAll();

        IList<NotificationDto> FindAllPagination(NotificationPaginationRequest payload);

        NotificationDto FindById(Guid id);

        NotificationDto Save(NotificationDto notificationDto);

        NotificationDto UpdateById(NotificationDto notificationDto, Guid id);

        void DeleteById(Guid id);
    }
}