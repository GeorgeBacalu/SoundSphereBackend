using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;

namespace SoundSphere.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly SoundSphereDbContext _context;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository, IUserRepository userRepository, SoundSphereDbContext context, IMapper mapper) => 
            (_notificationRepository, _userRepository, _context, _mapper) = (notificationRepository, userRepository, context, mapper);

        public IList<NotificationDto> GetAll(NotificationPaginationRequest? payload, Guid userId)
        {
            IList<NotificationDto> notificationDtos = _notificationRepository.GetAll(payload, userId).ToDtos(_mapper);
            return notificationDtos;
        }

        public NotificationDto GetById(Guid id)
        {
            NotificationDto notificationDto = _notificationRepository.GetById(id).ToDto(_mapper);
            return notificationDto;
        }

        public NotificationDto Send(NotificationDto notificationDto, Guid senderId, Guid receiverId)
        {
            if (senderId.Equals(receiverId))
                throw new InvalidRequestException(SendNotificationToSelfDenied);
            notificationDto.SenderId = senderId;
            notificationDto.ReceiverId = receiverId;
            Notification notificationToSend = notificationDto.ToEntity(_userRepository, _mapper);
            _notificationRepository.LinkNotificationToSender(notificationToSend);
            _notificationRepository.LinkNotificationToReceiver(notificationToSend);
            NotificationDto sentNotificationDto = _notificationRepository.Add(notificationToSend).ToDto(_mapper);
            return sentNotificationDto;
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

        public int CountUnread(Guid userId)
        {
            int nrUnread = _context.Notifications.Count(notification => notification.ReceiverId.Equals(userId) && !notification.IsRead);
            return nrUnread;
        }

        public void MarkAsRead(Guid notificationId, Guid userId)
        {
            Notification notification = _notificationRepository.GetById(notificationId);
            if (!notification.ReceiverId.Equals(userId))
                throw new ForbiddenAccessException(ReadNotificationDenied);
            notification.IsRead = true;
            _context.SaveChanges();
        }

        public void MarkAllAsRead(Guid userId)
        {
            _context.Notifications
                .Where(notification => notification.ReceiverId.Equals(userId))
                .ToList().ForEach(notification => notification.IsRead = true);
            _context.SaveChanges();
        }
    }
}