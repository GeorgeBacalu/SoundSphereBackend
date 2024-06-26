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
    public class ArtistController : BaseController
    {
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService) => _artistService = artistService;

        ///<summary>Get active artists paginated, sorted and filtered</summary>
        ///<remarks>Return list with active artists paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with artists pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("get")] public IActionResult GetAll(ArtistPaginationRequest? payload)
        {
            IList<ArtistDto> artistDtos = _artistService.GetAll(payload);
            return Ok(new { userId = GetUserId(), artistDtos });
        }

        /// <summary>Get active artist by ID</summary>
        /// <remarks>Return active artist with given ID</remarks>
        /// <param name="id">Artist fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id)
        {
            ArtistDto artistDto = _artistService.GetById(id);
            return Ok(new { userId = GetUserId(), artistDto });
        }

        /// <summary>Add artist</summary>
        /// <remarks>Add new artist</remarks>
        /// <param name="artistDto">Artist to add</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Moderator,Admin")]
        [HttpPost] public IActionResult Add(ArtistDto artistDto)
        {
            ArtistDto createdArtistDto = _artistService.Add(artistDto);
            return CreatedAtAction(nameof(GetById), new { createdArtistDto.Id }, createdArtistDto);
        }

        /// <summary>Update artist by ID</summary>
        /// <remarks>Update artist with given ID</remarks>
        /// <param name="artistDto">Updated artist</param>
        /// <param name="id">Artist updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Moderator,Admin")]
        [HttpPut("{id}")] public IActionResult UpdateById(ArtistDto artistDto, Guid id)
        {
            ArtistDto updatedArtistDto = _artistService.UpdateById(artistDto, id);
            return Ok(new { userId = GetUserId(), updatedArtistDto });
        }

        /// <summary>Delete artist by ID</summary>
        /// <remarks>Soft delete artist with given ID</remarks>
        /// <param name="id">Artist deleting ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id)
        {
            ArtistDto deletedArtistDto = _artistService.DeleteById(id);
            return Ok(new { userId = GetUserId(), deletedArtistDto });
        }

        /// <summary>Get artist recommendations</summary>
        /// <remarks>Return list with randomly selected artists as recommendations</remarks>
        /// <param name="nrRecommendations">Number of recommendations</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("recommendations")] public IActionResult GetRecommendations(int nrRecommendations)
        {
            IList<ArtistDto> recommendationDtos = _artistService.GetRecommendations(nrRecommendations);
            return Ok(new { userId = GetUserId(), recommendationDtos });
        }

        /// <summary>(Un)follow artist by ID</summary>
        /// <remarks>Follow artist with given ID if they are not followed, otherwise unfollow them</remarks>
        /// <param name="id">Artist ID to (un)follow</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}/follow")] public IActionResult ToggleFollow(Guid id)
        {
            _artistService.ToggleFollow(id, GetUserId());
            return Ok(new { userId = GetUserId(), message = "Artist (un)followed successfully" });
        }

        /// <summary>Get number of followers for artist by ID</summary>
        /// <remarks>Return number of followers for artist with given ID</remarks>
        /// <param name="id">Artist ID to count followers for</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}/followers")] public IActionResult CountFollowers(Guid id)
        {
            int nrFollowers = _artistService.CountFollowers(id);
            return Ok(new { userId = GetUserId(), nrFollowers });
        }
    }
}