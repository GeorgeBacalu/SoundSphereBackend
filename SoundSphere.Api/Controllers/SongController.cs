using Microsoft.AspNetCore.Authorization;
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
    public class SongController : BaseController
    {
        private readonly ISongService _songService;

        public SongController(ISongService songService) => _songService = songService;

        /// <summary>Get active songs paginated, sorted and filtered</summary>
        /// <remarks>Return list with active songs paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with songs pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("get")] public IActionResult GetAll(SongPaginationRequest payload)
        {
            IList<SongDto> result = _songService.GetAll(payload);
            return Ok(new { userId = GetUserId(), songs = result });
        }

        /// <summary>Get active song by ID</summary>
        /// <remarks>Return active song with given ID</remarks>
        /// <param name="id">Song fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id)
        {
            SongDto result = _songService.GetById(id);
            return Ok(new { userId = GetUserId(), song = result });
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
            return CreatedAtAction(nameof(GetById), new { id = createdSongDto.Id }, createdSongDto);
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
            SongDto result = _songService.UpdateById(songDto, id);
            return Ok(new { userId = GetUserId(), updatedSong = result });
        }

        /// <summary>Delete song by ID</summary>
        /// <remarks>Soft delete song with given ID</remarks>
        /// <param name="id">Song deleting ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id)
        {
            SongDto result = _songService.DeleteById(id);
            return Ok(new { userId = GetUserId(), deletedSong = result });
        }
    }
}