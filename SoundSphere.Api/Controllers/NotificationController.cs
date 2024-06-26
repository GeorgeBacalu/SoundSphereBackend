using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using System.Net.Mime;

namespace SoundSphere.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService) => _notificationService = notificationService;

        /// <summary>Get active notifications paginated, sorted and filtered</summary>
        /// <remarks>Return list with active notifications paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with notifications pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("get")] public IActionResult GetAll(NotificationPaginationRequest? payload)
        {
            IList<NotificationDto> notificationDtos = _notificationService.GetAll(payload, GetUserId());
            return Ok(new { userId = GetUserId(), notificationDtos });
        }

        /// <summary>Get active notification by ID</summary>
        /// <remarks>Return active notification with given ID</remarks>
        /// <param name="id">Notification fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id)
        {
            NotificationDto notificationDto = _notificationService.GetById(id);
            return Ok(new { userId = GetUserId(), notificationDto });
        }

        /// <summary>Send notification</summary>
        /// <remarks>Send notification to another user</remarks>
        /// <param name="notificationDto">Notification to send</param>
        /// <param name="receiverId">The user to receive the notification</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("{receiverId}")] public IActionResult Send(NotificationDto notificationDto, Guid receiverId)
        {
            Guid senderId = GetUserId();
            NotificationDto createdNotificationDto = _notificationService.Send(notificationDto, senderId, receiverId);
            return CreatedAtAction(nameof(GetById), new { createdNotificationDto.Id }, createdNotificationDto);
        }

        /// <summary>Update notification by ID</summary>
        /// <remarks>Update notification with given ID</remarks>
        /// <param name="notificationDto">Updated notification</param>
        /// <param name="id">Notification updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Moderator,Admin")]
        [HttpPut("{id}")] public IActionResult UpdateById(NotificationDto notificationDto, Guid id)
        {
            NotificationDto updatedNotificationDto = _notificationService.UpdateById(notificationDto, id);
            return Ok(new { userId = GetUserId(), updatedNotificationDto });
        }

        /// <summary>Delete notification by ID</summary>
        /// <remarks>Soft delete notification with given ID</remarks>
        /// <param name="id">Notification deleting ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id)
        {
            NotificationDto deletedNotificationDto = _notificationService.DeleteById(id);
            return Ok(new { userId = GetUserId(), deletedNotificationDto });
        }

        /// <summary>Counts unread notifications</summary>
        /// <remarks>Return number of unread notifications for the logged in user</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("unread")] public IActionResult CountUnread()
        {
            int nrUnread = _notificationService.CountUnread(GetUserId());
            return Ok(new { userId = GetUserId(), nrUnread });
        }

        /// <summary>Mark notification as read</summary>
        /// <remarks>Mark notification with given ID as read</remarks>
        /// <param name="id">Notification ID to mark as read</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPut("read")] public IActionResult MarkAsRead(Guid id)
        {
            _notificationService.MarkAsRead(id, GetUserId());
            return Ok(new { userId = GetUserId(), message = "Notification was read" });
        }

        /// <summary>Mark all notifications as read</summary>
        /// <remarks>Mark all notifications as read for the logged in user</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("read-all")] public IActionResult MarkAllAsRead()
        {
            _notificationService.MarkAllAsRead(GetUserId());
            return Ok(new { userId = GetUserId(), message = "All notifications were read" });
        }
    }
}