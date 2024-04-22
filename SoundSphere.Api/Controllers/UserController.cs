using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;
using System.Net.Mime;

namespace SoundSphere.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) => _userService = userService;

        /// <summary>Find all users</summary>
        /// <remarks>Return list with all users</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet] public IActionResult FindAll() => Ok(_userService.FindAll());

        /// <summary>Find all active users</summary>
        /// <remarks>Return list with all active users</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("active")] public IActionResult FindAllActive() => Ok(_userService.FindAllActive());

        /// <summary>Find user by ID</summary>
        /// <remarks>Return user with given ID</remarks>
        /// <param name="id">User fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult FindById(Guid id) => Ok(_userService.FindById(id));

        /// <summary>Save user</summary>
        /// <remarks>Save new user</remarks>
        /// <param name="userDto">User to save</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Save(UserDto userDto)
        {
            UserDto savedUserDto = _userService.Save(userDto);
            return CreatedAtAction(nameof(FindById), new { id = savedUserDto.Id }, savedUserDto);
        }

        /// <summary>Update user by ID</summary>
        /// <remarks>Update user with given ID</remarks>
        /// <param name="userDto">Updated user</param>
        /// <param name="id">User updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")] public IActionResult UpdateById(UserDto userDto, Guid id) => Ok(_userService.UpdateById(userDto, id));

        /// <summary>Disable user by ID</summary>
        /// <remarks>Disable user with given ID</remarks>
        /// <param name="id">User disabling ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DisableById(Guid id) => Ok(_userService.DisableById(id));
    }
}