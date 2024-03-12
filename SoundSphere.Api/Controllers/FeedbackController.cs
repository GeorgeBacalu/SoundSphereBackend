using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;

namespace SoundSphereV2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService) => _feedbackService = feedbackService;

        [HttpGet] public IActionResult FindAll() => Ok(_feedbackService.FindAll());

        [HttpGet("{id}")] public IActionResult FindById(Guid id) => Ok(_feedbackService.FindById(id));

        [HttpPost] public IActionResult Save(Feedback feedback)
        {
            Feedback savedFeedback = _feedbackService.Save(feedback);
            return CreatedAtAction(nameof(FindById), new { id = savedFeedback.Id }, savedFeedback);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(Feedback feedback, Guid id) => Ok(_feedbackService.UpdateById(feedback, id));

        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id)
        {
            _feedbackService.DeleteById(id);
            return NoContent();
        }
    }
}