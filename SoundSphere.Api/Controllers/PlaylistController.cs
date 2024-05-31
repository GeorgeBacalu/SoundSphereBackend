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
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService) => _playlistService = playlistService;

        /// <summary>Get all playlists</summary>
        /// <remarks>Return list with all playlists</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet] public IActionResult GetAll() => Ok(_playlistService.GetAll());

        /// <summary>Get all active playlists</summary>
        /// <remarks>Return list with all active playlists</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("active")] public IActionResult GetAllActive() => Ok(_playlistService.GetAllActive());

        /// <summary>Get playlists paginated, sorted and filtered</summary>
        /// <remarks>Return list with playlists paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with playlists pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("pagination")] public IActionResult GetAllPagination(PlaylistPaginationRequest payload) => Ok(_playlistService.GetAllPagination(payload));

        /// <summary>Get active playlists paginated, sorted and filtered</summary>
        /// <remarks>Return list with playlists paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with playlists pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("active/pagination")] public IActionResult GetAllActivePagination(PlaylistPaginationRequest payload) => Ok(_playlistService.GetAllActivePagination(payload));

        /// <summary>Get playlist by ID</summary>
        /// <remarks>Return playlist with given ID</remarks>
        /// <param name="id">Playlist fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id) => Ok(_playlistService.GetById(id));

        /// <summary>Add playlist</summary>
        /// <remarks>Add new playlist</remarks>
        /// <param name="playlistDto">Playlist to add</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Add(PlaylistDto playlistDto)
        {
            PlaylistDto addedPlaylistDto = _playlistService.Add(playlistDto);
            return CreatedAtAction(nameof(GetById), new { id = addedPlaylistDto.Id }, addedPlaylistDto);
        }

        /// <summary>Update playlist by ID</summary>
        /// <remarks>Update playlist with given ID</remarks>
        /// <param name="playlistDto">Updated playlist</param>
        /// <param name="id">Playlist updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")] public IActionResult UpdateById(PlaylistDto playlistDto, Guid id) => Ok(_playlistService.UpdateById(playlistDto, id));

        /// <summary>Delete playlist by ID</summary>
        /// <remarks>Soft delete playlist with given ID</remarks>
        /// <param name="id">Playlist deleting ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_playlistService.DeleteById(id));
    }
}