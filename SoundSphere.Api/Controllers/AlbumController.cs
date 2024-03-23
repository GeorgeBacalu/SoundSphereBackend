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
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService) => _albumService = albumService;

        /// <summary>Find all albums</summary>
        /// <remarks>Return list with all albums</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet] public IActionResult FindAll() => Ok(_albumService.FindAll());

        /// <summary>Find album by ID</summary>
        /// <remarks>Return album with given ID</remarks>
        /// <param name="id">Album fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult FindById(Guid id) => Ok(_albumService.FindById(id));

        /// <summary>Save album</summary>
        /// <remarks>Save new album</remarks>
        /// <param name="albumDto">Album to save</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Save(AlbumDto albumDto)
        {
            AlbumDto savedAlbumDto = _albumService.Save(albumDto);
            return CreatedAtAction(nameof(FindById), new { id = savedAlbumDto.Id }, savedAlbumDto);
        }

        /// <summary>Update album by ID</summary>
        /// <remarks>Update album with given ID</remarks>
        /// <param name="albumDto">Updated album</param>
        /// <param name="id">Album updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")] public IActionResult UpdateById(AlbumDto albumDto, Guid id) => Ok(_albumService.UpdateById(albumDto, id));

        /// <summary>Disable album by ID</summary>
        /// <remarks>Disable album with given ID</remarks>
        /// <param name="id">Album disabling ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DisableById(Guid id) => Ok(_albumService.DisableById(id));
    }
}