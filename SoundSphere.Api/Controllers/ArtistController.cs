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
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService) => _artistService = artistService;

        ///<summary>Get active artists paginated, sorted and filtered</summary>
        ///<remarks>Return list with active artists paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with artists pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("get")] public IActionResult GetAll(ArtistPaginationRequest payload) => Ok(_artistService.GetAll(payload));

        /// <summary>Get active artist by ID</summary>
        /// <remarks>Return active artist with given ID</remarks>
        /// <param name="id">Artist fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id) => Ok(_artistService.GetById(id));

        /// <summary>Add artist</summary>
        /// <remarks>Add new artist</remarks>
        /// <param name="artistDto">Artist to add</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Add(ArtistDto artistDto)
        {
            ArtistDto createdArtistDto = _artistService.Add(artistDto);
            return CreatedAtAction(nameof(GetById), new { id = createdArtistDto.Id }, createdArtistDto);
        }

        /// <summary>Update artist by ID</summary>
        /// <remarks>Update artist with given ID</remarks>
        /// <param name="artistDto">Updated artist</param>
        /// <param name="id">Artist updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")] public IActionResult UpdateById(ArtistDto artistDto, Guid id) => Ok(_artistService.UpdateById(artistDto, id));

        /// <summary>Delete artist by ID</summary>
        /// <remarks>Soft delete artist with given ID</remarks>
        /// <param name="id">Artist deleting ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_artistService.DeleteById(id));
    }
}