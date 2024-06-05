using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using System.Net.Mime;

namespace SoundSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService) => _notificationService = notificationService;

        /// <summary>Get active notifications paginated, sorted and filtered</summary>
        /// <remarks>Return list with active notifications paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with notifications pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("get")] public IActionResult GetAll(NotificationPaginationRequest payload) => Ok(_notificationService.GetAll(payload));

        /// <summary>Get active notification by ID</summary>
        /// <remarks>Return active notification with given ID</remarks>
        /// <param name="id">Notification fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id) => Ok(_notificationService.GetById(id));

        /// <summary>Add notification</summary>
        /// <remarks>Add new notification</remarks>
        /// <param name="notificationDto">Notification to add</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [HttpPut("{id}")] public IActionResult UpdateById(NotificationDto notificationDto, Guid id) => Ok(_notificationService.UpdateById(notificationDto, id));

        /// <summary>Delete notification by ID</summary>
        /// <remarks>Soft delete notification with given ID</remarks>
        /// <param name="id">Notification deleting ID</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_notificationService.DeleteById(id));
    }
}