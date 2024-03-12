using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;

namespace SoundSphereV2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorityController : ControllerBase
    {
        private readonly IAuthorityService _authorityService;

        public AuthorityController(IAuthorityService authorityService) => _authorityService = authorityService;

        [HttpGet] public IActionResult FindAll() => Ok(_authorityService.FindAll());

        [HttpGet("{id}")] public IActionResult FindById(Guid id) => Ok(_authorityService.FindById(id));

        [HttpPost] public IActionResult Save(Authority authority)
        {
            Authority savedAuthority = _authorityService.Save(authority);
            return CreatedAtAction(nameof(FindById), new { id = savedAuthority.Id }, savedAuthority);
        }
    }
}