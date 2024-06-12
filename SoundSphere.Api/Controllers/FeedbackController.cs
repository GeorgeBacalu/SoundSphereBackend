﻿using Microsoft.AspNetCore.Authorization;
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
    public class FeedbackController : BaseController
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService) => _feedbackService = feedbackService;

        /// <summary>Get active feedbacks paginated, sorted and filtered</summary>
        /// <remarks>Return list with active feedbacks paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with feedbacks pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("get")] public IActionResult GetAll(FeedbackPaginationRequest payload)
        {
            IList<FeedbackDto> result = _feedbackService.GetAll(payload);
            return Ok(new { userId = GetUserId(), feedbacks = result });
        }

        /// <summary>Get active feedback by ID</summary>
        /// <remarks>Return active feedback with given ID</remarks>
        /// <param name="id">Feedback fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id)
        {
            FeedbackDto result = _feedbackService.GetById(id);
            return Ok(new { userId = GetUserId(), feedback = result });
        }

        /// <summary>Add feedback</summary>
        /// <remarks>Add new feedback</remarks>
        /// <param name="feedbackDto">Feedback to add</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Add(FeedbackDto feedbackDto)
        {
            FeedbackDto createdFeedbackDto = _feedbackService.Add(feedbackDto);
            return CreatedAtAction(nameof(GetById), new { id = createdFeedbackDto.Id }, createdFeedbackDto);
        }

        /// <summary>Update feedback by ID</summary>
        /// <remarks>Update feedback with given ID</remarks>
        /// <param name="feedbackDto">Updated feedback</param>
        /// <param name="id">Feedback updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")] public IActionResult UpdateById(FeedbackDto feedbackDto, Guid id)
        {
            FeedbackDto result = _feedbackService.UpdateById(feedbackDto, id);
            return Ok(new { userId = GetUserId(), updatedFeedback = result });
        }

        /// <summary>Delete feedback by ID</summary>
        /// <remarks>Soft delete feedback with given ID</remarks>
        /// <param name="id">Feedback deleting ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id)
        {
            FeedbackDto result = _feedbackService.DeleteById(id);
            return Ok(new { userId = GetUserId(), deletedFeedback = result });
        }
    }
}