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

        /// <summary>Get active albums paginated, sorted and filtered</summary>
        /// <remarks>Return list with active albums paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with albums pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("query")] public IActionResult GetAll(AlbumPaginationRequest payload) => Ok(_albumService.GetAll(payload));

        /// <summary>Get active album by ID</summary>
        /// <remarks>Return active album with given ID</remarks>
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
            AlbumDto createdAlbumDto = _albumService.Add(albumDto);
            return CreatedAtAction(nameof(GetById), new { id = createdAlbumDto.Id }, createdAlbumDto);
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