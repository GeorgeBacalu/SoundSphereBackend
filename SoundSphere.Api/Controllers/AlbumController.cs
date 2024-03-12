using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;

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

        [HttpPost] public IActionResult Save(Album album)
        {
            Album savedAlbum = _albumService.Save(album);
            return CreatedAtAction(nameof(FindById), new { id = savedAlbum.Id }, savedAlbum);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(Album album, Guid id) => Ok(_albumService.UpdateById(album, id));

        [HttpDelete("{id}")] public IActionResult DisableById(Guid id) => Ok(_albumService.DisableById(id));
    }
}