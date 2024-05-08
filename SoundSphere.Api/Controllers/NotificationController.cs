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

        /// <summary>Find all notifications</summary>
        /// <remarks>Return list with all notifications</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet] public IActionResult FindAll() => Ok(_notificationService.FindAll());

        /// <summary>Find notifications paginated, sorted and filtered</summary>
        /// <remarks>Return list with notifications paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with notifications pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("pagination")] public IActionResult FindAllPagination(NotificationPaginationRequest payload) => Ok(_notificationService.FindAllPagination(payload));

        /// <summary>Find notification by ID</summary>
        /// <remarks>Return notification with given ID</remarks>
        /// <param name="id">Notification fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult FindById(Guid id) => Ok(_notificationService.FindById(id));

        /// <summary>Save notification</summary>
        /// <remarks>Save new notification</remarks>
        /// <param name="notificationDto">Notification to save</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Save(NotificationDto notificationDto)
        {
            NotificationDto savedNotificationDto = _notificationService.Save(notificationDto);
            return CreatedAtAction(nameof(FindById), new { id = savedNotificationDto.Id }, savedNotificationDto);
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
        /// <remarks>Delete notification with given ID</remarks>
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