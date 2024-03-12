using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;

namespace SoundSphere.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ISongService _songService;

        public SongController(ISongService songService) => _songService = songService;

        [HttpGet] public IActionResult FindAll() => Ok(_songService.FindAll());

        [HttpGet("{id}")] public IActionResult FindById(Guid id) => Ok(_songService.FindById(id));

        [HttpPost] public IActionResult Save(Song song)
        {
            Song savedSong = _songService.Save(song);
            return CreatedAtAction(nameof(FindById), new { id = savedSong.Id }, savedSong);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(Song song, Guid id) => Ok(_songService.UpdateById(song, id));

        [HttpDelete("{id}")] public IActionResult DisableById(Guid id) => Ok(_songService.DisableById(id));
    }
}