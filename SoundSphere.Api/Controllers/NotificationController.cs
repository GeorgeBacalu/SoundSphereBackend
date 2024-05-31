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

        /// <summary>Get all notifications</summary>
        /// <remarks>Return list with all notifications</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet] public IActionResult GetAll() => Ok(_notificationService.GetAll());

        /// <summary>Get notifications paginated, sorted and filtered</summary>
        /// <remarks>Return list with notifications paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with notifications pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("pagination")] public IActionResult GetAllPagination(NotificationPaginationRequest payload) => Ok(_notificationService.GetAllPagination(payload));

        /// <summary>Get notification by ID</summary>
        /// <remarks>Return notification with given ID</remarks>
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
            NotificationDto addedNotificationDto = _notificationService.Add(notificationDto);
            return CreatedAtAction(nameof(GetById), new { id = addedNotificationDto.Id }, addedNotificationDto);
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
        /// <remarks>Hard delete notification with given ID</remarks>
        /// <param name="id">Notification deleting ID</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id)
        {
            _notificationService.DeleteById(id);
            return NoContent();
        }
    }
}