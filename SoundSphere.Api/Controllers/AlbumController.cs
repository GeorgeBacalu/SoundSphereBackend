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
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService) => _albumService = albumService;

        /// <summary>Get all albums</summary>
        /// <remarks>Return list with all albums</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet] public IActionResult GetAll() => Ok(_albumService.GetAll());

        /// <summary>Get all active albums</summary>
        /// <remarks>Return list with all active albums</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("active")] public IActionResult GetAllActive() => Ok(_albumService.GetAllActive());

        /// <summary>Get albums paginated, sorted and filtered</summary>
        /// <remarks>Return list with albums paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with albums pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("pagination")] public IActionResult GetAllPagination(AlbumPaginationRequest payload) => Ok(_albumService.GetAllPagination(payload));

        /// <summary>Get active albums paginated, sorted and filtered</summary>
        /// <remarks>Return list with active albums paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with albums pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("active/pagination")] public IActionResult GetAllActivePagination(AlbumPaginationRequest payload) => Ok(_albumService.GetAllActivePagination(payload));

        /// <summary>Get album by ID</summary>
        /// <remarks>Return album with given ID</remarks>
        /// <param name="id">Album fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id) => Ok(_albumService.GetById(id));

        /// <summary>Add album</summary>
        /// <remarks>Add new album</remarks>
        /// <param name="albumDto">Album to add</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Add(AlbumDto albumDto)
        {
            AlbumDto addedAlbumDto = _albumService.Add(albumDto);
            return CreatedAtAction(nameof(GetById), new { id = addedAlbumDto.Id }, addedAlbumDto);
        }

        /// <summary>Update album by ID</summary>
        /// <remarks>Update album with given ID</remarks>
        /// <param name="albumDto">Updated album</param>
        /// <param name="id">Album updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")] public IActionResult UpdateById(AlbumDto albumDto, Guid id) => Ok(_albumService.UpdateById(albumDto, id));

        /// <summary>Delete album by ID</summary>
        /// <remarks>Soft delete album with given ID</remarks>
        /// <param name="id">Album deleting ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_albumService.DeleteById(id));
    }
}