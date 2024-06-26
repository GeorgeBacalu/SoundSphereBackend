using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Dtos.Response;
using System.Net.Mime;

namespace SoundSphere.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    public class SongController : BaseController
    {
        private readonly ISongService _songService;

        public SongController(ISongService songService) => _songService = songService;

        /// <summary>Get active songs paginated, sorted and filtered</summary>
        /// <remarks>Return list with active songs paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with songs pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("get")] public IActionResult GetAll(SongPaginationRequest? payload)
        {
            IList<SongDto> songDtos = _songService.GetAll(payload);
            return Ok(new { userId = GetUserId(), songDtos });
        }

        /// <summary>Get active song by ID</summary>
        /// <remarks>Return active song with given ID</remarks>
        /// <param name="id">Song fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id)
        {
            SongDto songDto = _songService.GetById(id);
            return Ok(new { userId = GetUserId(), songDto });
        }

        /// <summary>Add song</summary>
        /// <remarks>Add new song</remarks>
        /// <param name="songDto">Song to add</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Moderator,Admin")]
        [HttpPost] public IActionResult Add(SongDto songDto)
        {
            SongDto createdSongDto = _songService.Add(songDto);
            return CreatedAtAction(nameof(GetById), new { createdSongDto.Id }, createdSongDto);
        }

        /// <summary>Update song by ID</summary>
        /// <remarks>Update song with given ID</remarks>
        /// <param name="songDto">Updated song</param>
        /// <param name="id">Song updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Moderator,Admin")]
        [HttpPut("{id}")] public IActionResult UpdateById(SongDto songDto, Guid id)
        {
            SongDto updatedSongDto = _songService.UpdateById(songDto, id);
            return Ok(new { userId = GetUserId(), updatedSongDto });
        }

        /// <summary>Delete song by ID</summary>
        /// <remarks>Soft delete song with given ID</remarks>
        /// <param name="id">Song deleting ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id)
        {
            SongDto deletedSongDto = _songService.DeleteById(id);
            return Ok(new { userId = GetUserId(), deletedSongDto });
        }

        /// <summary>Get song recommendations</summary>
        /// <remarks>Return list with randomly selected songs as recommendations</remarks>
        /// <param name="nrRecommendations">Number of recommendations</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("recommendations")] public IActionResult GetRecommendations(int nrRecommendations)
        {
            IList<SongDto> recommendationDtos = _songService.GetRecommendations(nrRecommendations);
            return Ok(new { userId = GetUserId(), recommendationDtos });
        }

        /// <summary>Get song statistics</summary>
        /// <remarks>Return statistics about songs for the given date range</remarks>
        /// <param name="startDate">Start date for interval</param>
        /// <param name="endDate">End date for interval</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("statistics")] public IActionResult GetStatistics(DateTime? startDate, DateTime? endDate)
        {
            SongStatisticsDto statistics = _songService.GetStatistics(startDate, endDate);
            return Ok(new { userId = GetUserId(), statistics });
        }

        /// <summary>Play song by ID</summary>
        /// <remarks>Play song with given ID thus increasing the play count of that song for the user with the given ID</remarks>
        /// <param name="id">Song playing ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("{id}/play")] public IActionResult Play(Guid id)
        {
            _songService.Play(id, GetUserId());
            return Ok(new { userId = GetUserId(), message = "Song played successfully" });
        }

        /// <summary>Get song play count</summary>
        /// <remarks>Count how many times the song with the given ID was played by the user with the given ID</remarks>
        /// <param name="id">ID of the song to get the play count for, for the logged in user</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}/plays")] public IActionResult GetPlayCount(Guid id)
        {
            int playCount = _songService.GetPlayCount(id, GetUserId());
            return Ok(new { userId = GetUserId(), playCount });
        }
    }
}