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

        /// <summary>Get active playlists paginated, sorted and filtered</summary>
        /// <remarks>Return list with active playlists paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with playlists pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("query")] public IActionResult GetAll(PlaylistPaginationRequest payload) => Ok(_playlistService.GetAll(payload));

        /// <summary>Get active playlist by ID</summary>
        /// <remarks>Return active playlist with given ID</remarks>
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
            PlaylistDto createdPlaylistDto = _playlistService.Add(playlistDto);
            return CreatedAtAction(nameof(GetById), new { id = createdPlaylistDto.Id }, createdPlaylistDto);
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