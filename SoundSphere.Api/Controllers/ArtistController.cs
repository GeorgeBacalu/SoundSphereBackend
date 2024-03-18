using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;

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

        [HttpPost] public IActionResult Save(ArtistDto artistDto)
        {
            ArtistDto savedArtistDto = _artistService.Save(artistDto);
            return CreatedAtAction(nameof(FindById), new { id = savedArtistDto.Id }, savedArtistDto);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(ArtistDto artistDto, Guid id) => Ok(_artistService.UpdateById(artistDto, id));

        [HttpDelete("{id}")] public IActionResult DisableById(Guid id) => Ok(_artistService.DisableById(id));
    }
}