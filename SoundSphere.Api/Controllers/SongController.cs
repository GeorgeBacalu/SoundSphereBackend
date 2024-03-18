using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;

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

        [HttpPost] public IActionResult Save(SongDto songDto)
        {
            SongDto savedSongDto = _songService.Save(songDto);
            return CreatedAtAction(nameof(FindById), new { id = savedSongDto.Id }, savedSongDto);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(SongDto songDto, Guid id) => Ok(_songService.UpdateById(songDto, id));

        [HttpDelete("{id}")] public IActionResult DisableById(Guid id) => Ok(_songService.DisableById(id));
    }
}