using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using System.Net.Mime;

namespace SoundSphere.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class SongController : ControllerBase
    {
        private readonly ISongService _songService;

        public SongController(ISongService songService) => _songService = songService;

        /// <summary>Get all songs</summary>
        /// <remarks>Return list with all songs</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet] public IActionResult GetAll() => Ok(_songService.GetAll());

        /// <summary>Get all active songs</summary>
        /// <remarks>Return list with all active songs</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("active")] public IActionResult GetAllActive() => Ok(_songService.GetAllActive());

        /// <summary>Get songs paginated, sorted and filtered</summary>
        /// <remarks>Return list with songs paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with songs pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("pagination")] public IActionResult GetAllPagination(SongPaginationRequest payload) => Ok(_songService.GetAllPagination(payload));

        /// <summary>Get active songs paginated, sorted and filtered</summary>
        /// <remarks>Return list with active songs paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with songs pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("active/pagination")] public IActionResult GetAllActivePagination(SongPaginationRequest payload) => Ok(_songService.GetAllActivePagination(payload));

        /// <summary>Get song by ID</summary>
        /// <remarks>Return song with given ID</remarks>
        /// <param name="id">Song fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id) => Ok(_songService.GetById(id));

        /// <summary>Add song</summary>
        /// <remarks>Add new song</remarks>
        /// <param name="songDto">Song to add</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Add(SongDto songDto)
        {
            SongDto addedSongDto = _songService.Add(songDto);
            return CreatedAtAction(nameof(GetById), new { id = addedSongDto.Id }, addedSongDto);
        }

        /// <summary>Update song by ID</summary>
        /// <remarks>Update song with given ID</remarks>
        /// <param name="songDto">Updated song</param>
        /// <param name="id">Song updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")] public IActionResult UpdateById(SongDto songDto, Guid id) => Ok(_songService.UpdateById(songDto, id));

        /// <summary>Delete song by ID</summary>
        /// <remarks>Soft delete song with given ID</remarks>
        /// <param name="id">Song deleting ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_songService.DeleteById(id));
    }
}