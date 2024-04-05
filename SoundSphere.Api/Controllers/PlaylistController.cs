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
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService) => _playlistService = playlistService;

        /// <summary>Find all playlists</summary>
        /// <remarks>Return list with all playlists</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet] public IActionResult FindAll() => Ok(_playlistService.FindAll());

        /// <summary>Find all active playlists</summary>
        /// <remarks>Return list with all active playlists</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("active")] public IActionResult FindAllActive() => Ok(_playlistService.FindAllActive());

        /// <summary>Find playlist by ID</summary>
        /// <remarks>Return playlist with given ID</remarks>
        /// <param name="id">Playlist fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult FindById(Guid id) => Ok(_playlistService.FindById(id));

        /// <summary>Save playlist</summary>
        /// <remarks>Save new playlist</remarks>
        /// <param name="playlistDto">Playlist to save</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Save(PlaylistDto playlistDto)
        {
            PlaylistDto savedPlaylistDto = _playlistService.Save(playlistDto);
            return CreatedAtAction(nameof(FindById), new { id = savedPlaylistDto.Id }, savedPlaylistDto);
        }

        /// <summary>Update playlist by ID</summary>
        /// <remarks>Update playlist with given ID</remarks>
        /// <param name="playlistDto">Updated playlist</param>
        /// <param name="id">Playlist updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")] public IActionResult UpdateById(PlaylistDto playlistDto, Guid id) => Ok(_playlistService.UpdateById(playlistDto, id));

        /// <summary>Disable playlist by ID</summary>
        /// <remarks>Disable playlist with given ID</remarks>
        /// <param name="id">Playlist disabling ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DisableById(Guid id) => Ok(_playlistService.DisableById(id));
    }
}