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
    public class AlbumController : BaseController
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService) => _albumService = albumService;

        /// <summary>Get active albums paginated, sorted and filtered</summary>
        /// <remarks>Return list with active albums paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with albums pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("get")] public IActionResult GetAll(AlbumPaginationRequest payload) 
        {
            IList<AlbumDto> result = _albumService.GetAll(payload);
            return Ok(new { userId = GetUserId(), albums = result });
        }

        /// <summary>Get active album by ID</summary>
        /// <remarks>Return active album with given ID</remarks>
        /// <param name="id">Album fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id) 
        {
            AlbumDto result = _albumService.GetById(id);
            return Ok(new { userId = GetUserId(), album = result });
        }

        /// <summary>Add album</summary>
        /// <remarks>Add new album</remarks>
        /// <param name="albumDto">Album to add</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Moderator,Admin")]
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
        [Authorize(Roles = "Moderator,Admin")]
        [HttpPut("{id}")] public IActionResult UpdateById(AlbumDto albumDto, Guid id)
        {
            AlbumDto result = _albumService.UpdateById(albumDto, id);
            return Ok(new { userId = GetUserId(), updatedAlbum = result });
        }

        /// <summary>Delete album by ID</summary>
        /// <remarks>Soft delete album with given ID</remarks>
        /// <param name="id">Album deleting ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id)
        {
            AlbumDto result = _albumService.DeleteById(id);
            return Ok(new { userId = GetUserId(), deletedAlbum = result });
        }
    }
}