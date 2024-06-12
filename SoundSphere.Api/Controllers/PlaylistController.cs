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
        [HttpPost("get")] public IActionResult GetAll(PlaylistPaginationRequest payload)
        {
            IList<PlaylistDto> result = _playlistService.GetAll(payload);
            return Ok(new { userId = GetUserId(), playlists = result });
        }

        /// <summary>Get active playlist by ID</summary>
        /// <remarks>Return active playlist with given ID</remarks>
        /// <param name="id">Playlist fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id)
        {
            PlaylistDto result = _playlistService.GetById(id);
            return Ok(new { userId = GetUserId(), playlist = result });
        }

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
        [HttpPut("{id}")] public IActionResult UpdateById(PlaylistDto playlistDto, Guid id)
        {
            PlaylistDto result = _playlistService.UpdateById(playlistDto, id);
            return Ok(new { userId = GetUserId(), updatedPlaylist = result });
        }

        /// <summary>Delete playlist by ID</summary>
        /// <remarks>Soft delete playlist with given ID</remarks>
        /// <param name="id">Playlist deleting ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id)
        {
            PlaylistDto result = _playlistService.DeleteById(id);
            return Ok(new { userId = GetUserId(), deletedPlaylist = result });
        }
    }
}