using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using System.Net.Mime;

namespace SoundSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService) => _artistService = artistService;

        /// <summary>Find all artists</summary>
        /// <remarks>Return list with all artists</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet] public IActionResult FindAll() => Ok(_artistService.FindAll());

        /// <summary>Find all active artists</summary>
        /// <remarks>Return list with all active artists</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("active")] public IActionResult FindAllActive() => Ok(_artistService.FindAllActive());

        /// <summary>Find artist by ID</summary>
        /// <remarks>Return artist with given ID</remarks>
        /// <param name="id">Artist fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult FindById(Guid id) => Ok(_artistService.FindById(id));

        /// <summary>Save artist</summary>
        /// <remarks>Save new artist</remarks>
        /// <param name="artistDto">Artist to save</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Save(ArtistDto artistDto)
        {
            ArtistDto savedArtistDto = _artistService.Save(artistDto);
            return CreatedAtAction(nameof(FindById), new { id = savedArtistDto.Id }, savedArtistDto);
        }

        /// <summary>Update artist by ID</summary>
        /// <remarks>Update artist with given ID</remarks>
        /// <param name="artistDto">Updated artist</param>
        /// <param name="id">Artist updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")] public IActionResult UpdateById(ArtistDto artistDto, Guid id) => Ok(_artistService.UpdateById(artistDto, id));

        /// <summary>Disable artist by ID</summary>
        /// <remarks>Disable artist with given ID</remarks>
        /// <param name="id">Artist disabling ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DisableById(Guid id) => Ok(_artistService.DisableById(id));
    }
}