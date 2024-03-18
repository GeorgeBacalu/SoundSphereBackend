using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;

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

        [HttpPost] public IActionResult Save(FeedbackDto feedbackDto)
        {
            FeedbackDto savedFeedbackDto = _feedbackService.Save(feedbackDto);
            return CreatedAtAction(nameof(FindById), new { id = savedFeedbackDto.Id }, savedFeedbackDto);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(FeedbackDto feedbackDto, Guid id) => Ok(_feedbackService.UpdateById(feedbackDto, id));

        [HttpDelete("{id}")]
        public IActionResult DeleteById(Guid id)
        {
            _feedbackService.DeleteById(id);
            return NoContent();
        }
    }
}