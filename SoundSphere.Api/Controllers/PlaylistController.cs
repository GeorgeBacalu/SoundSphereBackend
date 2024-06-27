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
    public class PlaylistController : BaseController
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService) => _playlistService = playlistService;

        /// <summary>Get active playlists paginated, sorted and filtered</summary>
        /// <remarks>Return list with active playlists paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with playlists pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("get")] public IActionResult GetAll(PlaylistPaginationRequest? payload)
        {
            IList<PlaylistDto> playlistDtos = _playlistService.GetAll(payload, GetUserId());
            return Ok(new { userId = GetUserId(), playlistDtos });
        }

        /// <summary>Get active playlist by ID</summary>
        /// <remarks>Return active playlist with given ID</remarks>
        /// <param name="id">Playlist fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id)
        {
            PlaylistDto playlistDto = _playlistService.GetById(id, GetUserId());
            return Ok(new { userId = GetUserId(), playlistDto });
        }

        /// <summary>Add playlist</summary>
        /// <remarks>Add new playlist</remarks>
        /// <param name="playlistDto">Playlist to add</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Add(PlaylistDto playlistDto)
        {
            PlaylistDto createdPlaylistDto = _playlistService.Add(playlistDto, GetUserId());
            return CreatedAtAction(nameof(GetById), new { createdPlaylistDto.Id }, createdPlaylistDto);
        }

        /// <summary>Update playlist by ID</summary>
        /// <remarks>Update playlist with given ID</remarks>
        /// <param name="playlistDto">Updated playlist</param>
        /// <param name="id">Playlist updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")] public IActionResult UpdateById(PlaylistDto playlistDto, Guid id)
        {
            PlaylistDto updatedPlaylistDto = _playlistService.UpdateById(playlistDto, id, GetUserId());
            return Ok(new { userId = GetUserId(), updatedPlaylistDto });
        }

        /// <summary>Delete playlist by ID</summary>
        /// <remarks>Soft delete playlist with given ID</remarks>
        /// <param name="id">Playlist deleting ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id)
        {
            PlaylistDto deletedPlaylistDto = _playlistService.DeleteById(id, GetUserId());
            return Ok(new { userId = GetUserId(), deletedPlaylistDto });
        }

        /// <summary>Add song to playlist</summary>
        /// <remarks>Add song to playlist with given IDs</remarks>
        /// <param name="playlistId">ID of the playlist to add the song to</param>
        /// <param name="songId">ID of the song to add to the playlist</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPost("{playlistId}/song/{songId}")] public IActionResult AddSong(Guid playlistId, Guid songId)
        {
            PlaylistDto updatedPlaylistDto = _playlistService.AddSong(playlistId, songId, GetUserId());
            return Ok(new { userId = GetUserId(), updatedPlaylistDto });
        }

        /// <summary>Remove song from playlist</summary>
        /// <remarks>Remove song from playlist with given IDs</remarks>
        /// <param name="playlistId">ID of the playlist to remove the song from</param>
        /// <param name="songId">ID of the song to remove from the playlist</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpDelete("{playlistId}/song/{songId}")] public IActionResult RemoveSong(Guid playlistId, Guid songId)
        {
            PlaylistDto updatedPlaylistDto = _playlistService.RemoveSong(playlistId, songId, GetUserId());
            return Ok(new { userId = GetUserId(), updatedPlaylistDto });
        }
    }
}