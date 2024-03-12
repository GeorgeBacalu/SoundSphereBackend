using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;

namespace SoundSphereV2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService) => _notificationService = notificationService;

        [HttpGet] public IActionResult FindAll() => Ok(_notificationService.FindAll());

        [HttpGet("{id}")] public IActionResult FindById(Guid id) => Ok(_notificationService.FindById(id));

        [HttpPost] public IActionResult Save(Notification notification)
        {
            Notification savedNotification = _notificationService.Save(notification);
            return CreatedAtAction(nameof(FindById), new { id = savedNotification.Id }, savedNotification);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(Notification notification, Guid id) => Ok(_notificationService.UpdateById(notification, id));

        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id)
        {
            _notificationService.DeleteById(id);
            return NoContent();
        }
    }
}