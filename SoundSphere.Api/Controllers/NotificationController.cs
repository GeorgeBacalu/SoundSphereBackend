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
        [HttpPost("get")] public IActionResult GetAll(NotificationPaginationRequest payload)
        {
            IList<NotificationDto> result = _notificationService.GetAll(payload);
            return Ok(new { userId = GetUserId(), notifications = result });
        }

        /// <summary>Get active notification by ID</summary>
        /// <remarks>Return active notification with given ID</remarks>
        /// <param name="id">Notification fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id)
        {
            NotificationDto result = _notificationService.GetById(id);
            return Ok(new { userId = GetUserId(), notification = result });
        }

        /// <summary>Add notification</summary>
        /// <remarks>Add new notification</remarks>
        /// <param name="notificationDto">Notification to add</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Moderator,Admin")]
        [HttpPost] public IActionResult Add(NotificationDto notificationDto)
        {
            NotificationDto createdNotificationDto = _notificationService.Add(notificationDto);
            return CreatedAtAction(nameof(GetById), new { id = createdNotificationDto.Id }, createdNotificationDto);
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
            NotificationDto result = _notificationService.UpdateById(notificationDto, id);
            return Ok(new { userId = GetUserId(), updatedNotification = result });
        }

        /// <summary>Delete notification by ID</summary>
        /// <remarks>Soft delete notification with given ID</remarks>
        /// <param name="id">Notification deleting ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id)
        {
            NotificationDto result = _notificationService.DeleteById(id);
            return Ok(new { userId = GetUserId(), deletedNotification = result });
        }
    }
}