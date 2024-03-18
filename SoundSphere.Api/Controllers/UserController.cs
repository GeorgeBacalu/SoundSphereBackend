using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;

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

        [HttpPost] public IActionResult Save(UserDto userDto)
        {
            UserDto savedUserDto = _userService.Save(userDto);
            return CreatedAtAction(nameof(FindById), new { id = savedUserDto.Id }, savedUserDto);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(UserDto userDto, Guid id) => Ok(_userService.UpdateById(userDto, id));

        [HttpDelete("{id}")] public IActionResult DisableById(Guid id) => Ok(_userService.DisableById(id));
    }
}