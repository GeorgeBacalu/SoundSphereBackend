using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;
using System.Net.Mime;

namespace SoundSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService) => _feedbackService = feedbackService;

        /// <summary>Find all feedbacks</summary>
        /// <remarks>Return list with all feedbacks</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet] public IActionResult FindAll() => Ok(_feedbackService.FindAll());

        /// <summary>Find feedback by ID</summary>
        /// <remarks>Return feedback with given ID</remarks>
        /// <param name="id">Feedback fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult FindById(Guid id) => Ok(_feedbackService.FindById(id));

        /// <summary>Save feedback</summary>
        /// <remarks>Save new feedback</remarks>
        /// <param name="feedbackDto">Feedback to save</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Save(FeedbackDto feedbackDto)
        {
            FeedbackDto savedFeedbackDto = _feedbackService.Save(feedbackDto);
            return CreatedAtAction(nameof(FindById), new { id = savedFeedbackDto.Id }, savedFeedbackDto);
        }

        /// <summary>Update feedback by ID</summary>
        /// <remarks>Update feedback with given ID</remarks>
        /// <param name="feedbackDto">Updated feedback</param>
        /// <param name="id">Feedback updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")] public IActionResult UpdateById(FeedbackDto feedbackDto, Guid id) => Ok(_feedbackService.UpdateById(feedbackDto, id));

        /// <summary>Delete feedback by ID</summary>
        /// <remarks>Delete feedback with given ID</remarks>
        /// <param name="id">Feedback deleting ID</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id)
        {
            _feedbackService.DeleteById(id);
            return NoContent();
        }
    }
}