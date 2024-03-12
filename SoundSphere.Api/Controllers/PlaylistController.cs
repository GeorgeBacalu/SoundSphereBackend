using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;

namespace SoundSphereV2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService) => _playlistService = playlistService;

        [HttpGet] public IActionResult FindAll() => Ok(_playlistService.FindAll());

        [HttpGet("{id}")] public IActionResult FindById(Guid id) => Ok(_playlistService.FindById(id));

        [HttpPost] public IActionResult Save(Playlist playlist)
        {
            Playlist savedPlaylist = _playlistService.Save(playlist);
            return CreatedAtAction(nameof(FindById), new { id = savedPlaylist.Id }, savedPlaylist);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(Playlist playlist, Guid id) => Ok(_playlistService.UpdateById(playlist, id));

        [HttpDelete("{id}")] public IActionResult DisableById(Guid id) => Ok(_playlistService.DisableById(id));
    }
}