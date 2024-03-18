using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;

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

        [HttpPost] public IActionResult Save(PlaylistDto playlistDto)
        {
            PlaylistDto savedPlaylistDto = _playlistService.Save(playlistDto);
            return CreatedAtAction(nameof(FindById), new { id = savedPlaylistDto.Id }, savedPlaylistDto);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(PlaylistDto playlistDto, Guid id) => Ok(_playlistService.UpdateById(playlistDto, id));

        [HttpDelete("{id}")] public IActionResult DisableById(Guid id) => Ok(_playlistService.DisableById(id));
    }
}