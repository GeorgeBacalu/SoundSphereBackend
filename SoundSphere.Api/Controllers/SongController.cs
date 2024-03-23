using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;
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

        /// <summary>Find all songs</summary>
        /// <remarks>Return list with all songs</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet] public IActionResult FindAll() => Ok(_songService.FindAll());

        /// <summary>Find song by ID</summary>
        /// <remarks>Return song with given ID</remarks>
        /// <param name="id">Song fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult FindById(Guid id) => Ok(_songService.FindById(id));

        /// <summary>Save song</summary>
        /// <remarks>Save new song</remarks>
        /// <param name="songDto">Song to save</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Save(SongDto songDto)
        {
            SongDto savedSongDto = _songService.Save(songDto);
            return CreatedAtAction(nameof(FindById), new { id = savedSongDto.Id }, savedSongDto);
        }

        /// <summary>Update song by ID</summary>
        /// <remarks>Update song with given ID</remarks>
        /// <param name="songDto">Updated song</param>
        /// <param name="id">Song updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")] public IActionResult UpdateById(SongDto songDto, Guid id) => Ok(_songService.UpdateById(songDto, id));

        /// <summary>Disable song by ID</summary>
        /// <remarks>Disable song with given ID</remarks>
        /// <param name="id">Song disabling ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DisableById(Guid id) => Ok(_songService.DisableById(id));
    }
}