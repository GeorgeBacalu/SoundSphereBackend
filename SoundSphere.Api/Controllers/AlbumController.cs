using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;

namespace SoundSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService) => _albumService = albumService;

        [HttpGet] public IActionResult FindAll() => Ok(_albumService.FindAll());

        [HttpGet("{id}")] public IActionResult FindById(Guid id) => Ok(_albumService.FindById(id));

        [HttpPost] public IActionResult Save(AlbumDto albumDto)
        {
            AlbumDto savedAlbumDto = _albumService.Save(albumDto);
            return CreatedAtAction(nameof(FindById), new { id = savedAlbumDto.Id }, savedAlbumDto);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(AlbumDto albumDto, Guid id) => Ok(_albumService.UpdateById(albumDto, id));

        [HttpDelete("{id}")] public IActionResult DisableById(Guid id) => Ok(_albumService.DisableById(id));
    }
}