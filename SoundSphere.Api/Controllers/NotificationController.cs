using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;

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

        [HttpPost] public IActionResult Save(NotificationDto notificationDto)
        {
            NotificationDto savedNotificationDto = _notificationService.Save(notificationDto);
            return CreatedAtAction(nameof(FindById), new { id = savedNotificationDto.Id }, savedNotificationDto);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(NotificationDto notificationDto, Guid id) => Ok(_notificationService.UpdateById(notificationDto, id));

        [HttpDelete("{id}")]
        public IActionResult DeleteById(Guid id)
        {
            _notificationService.DeleteById(id);
            return NoContent();
        }
    }
}