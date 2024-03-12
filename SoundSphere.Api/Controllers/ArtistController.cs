using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;

namespace SoundSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService) => _artistService = artistService;

        [HttpGet] public IActionResult FindAll() => Ok(_artistService.FindAll());

        [HttpGet("{id}")] public IActionResult FindById(Guid id) => Ok(_artistService.FindById(id));

        [HttpPost] public IActionResult Save(Artist artist)
        {
            Artist savedArtist = _artistService.Save(artist);
            return CreatedAtAction(nameof(FindById), new { id = savedArtist.Id }, savedArtist);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(Artist artist, Guid id) => Ok(_artistService.UpdateById(artist, id));

        [HttpDelete("{id}")] public IActionResult DisableById(Guid id) => Ok(_artistService.DisableById(id));
    }
}