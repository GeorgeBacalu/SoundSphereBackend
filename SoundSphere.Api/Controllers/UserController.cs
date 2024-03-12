using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;

namespace SoundSphere.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) => _userService = userService;

        [HttpGet] public IActionResult FindAll() => Ok(_userService.FindAll());

        [HttpGet("{id}")] public IActionResult FindById(Guid id) => Ok(_userService.FindById(id));

        [HttpPost] public IActionResult Save(User user)
        {
            User savedUser = _userService.Save(user);
            return CreatedAtAction(nameof(FindById), new { id = savedUser.Id }, savedUser);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(User user, Guid id) => Ok(_userService.UpdateById(user, id));

        [HttpDelete("{id}")] public IActionResult DisableById(Guid id) => Ok(_userService.DisableById(id));
    }
}