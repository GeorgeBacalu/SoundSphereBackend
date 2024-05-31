using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
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

        /// <summary>Get all users</summary>
        /// <remarks>Return list with all users</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet] public IActionResult GetAll() => Ok(_userService.GetAll());

        /// <summary>Get all active users</summary>
        /// <remarks>Return list with all active users</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("active")] public IActionResult GetAllActive() => Ok(_userService.GetAllActive());

        /// <summary>Get users paginated, sorted and filtered</summary>
        /// <remarks>Return list with users paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with users pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("pagination")] public IActionResult GetAllPagination(UserPaginationRequest payload) => Ok(_userService.GetAllPagination(payload));

        /// <summary>Get active users paginated, sorted and filtered</summary>
        /// <remarks>Return list with active users paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with users pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("active/pagination")] public IActionResult GetAllActivePagination(UserPaginationRequest payload) => Ok(_userService.GetAllActivePagination(payload));

        /// <summary>Get user by ID</summary>
        /// <remarks>Return user with given ID</remarks>
        /// <param name="id">User fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id) => Ok(_userService.GetById(id));

        /// <summary>Add user</summary>
        /// <remarks>Add new user</remarks>
        /// <param name="userDto">User to add</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Add(UserDto userDto)
        {
            UserDto addedUserDto = _userService.Add(userDto);
            return CreatedAtAction(nameof(GetById), new { id = addedUserDto.Id }, addedUserDto);
        }

        /// <summary>Update user by ID</summary>
        /// <remarks>Update user with given ID</remarks>
        /// <param name="userDto">Updated user</param>
        /// <param name="id">User updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")] public IActionResult UpdateById(UserDto userDto, Guid id) => Ok(_userService.UpdateById(userDto, id));

        /// <summary>Delete user by ID</summary>
        /// <remarks>Soft delete user with given ID</remarks>
        /// <param name="id">User deleting ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_userService.DeleteById(id));
    }
}