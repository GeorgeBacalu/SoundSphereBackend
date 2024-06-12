using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Dtos.Request.Auth;
using System.Net.Mime;

namespace SoundSphere.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) => _userService = userService;

        /// <summary>Get active users paginated, sorted and filtered</summary>
        /// <remarks>Return list with active users paginated, sorted and filtered</remarks>
        /// <param name="payload">Request body with users pagination rules</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("get")] public IActionResult GetAll(UserPaginationRequest payload)
        {
            IList<UserDto> result = _userService.GetAll(payload);
            return Ok(new { userId = GetUserId(), users = result });
        }

        /// <summary>Get active user by ID</summary>
        /// <remarks>Return active user with given ID</remarks>
        /// <param name="id">User fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id)
        {
            UserDto result = _userService.GetById(id);
            return Ok(new { userId = GetUserId(), user = result });
        }

        /// <summary>Register new user</summary>
        /// <remarks>Create new user and return the created user</remarks>
        /// <param name="payload">User registration details</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("register")] public IActionResult Register(RegisterRequest payload)
        {
            UserDto? registeredUserDto = _userService.Register(payload);
            return CreatedAtAction(nameof(GetById), new { id = registeredUserDto?.Id }, registeredUserDto);
        }

        /// <summary>Login user</summary>
        /// <remarks>Authenticate user and return JWT token</remarks>
        /// <param name="payload">User login details</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("login")] public IActionResult Login(LoginRequest payload)
        {
            string? result = _userService.Login(payload);
            return Ok(new { token = result });
        }

        /// <summary>Update user by ID</summary>
        /// <remarks>Update user with given ID</remarks>
        /// <param name="userDto">Updated user</param>
        /// <param name="id">User updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")] public IActionResult UpdateById(UserDto userDto, Guid id)
        {
            UserDto result = _userService.UpdateById(userDto, id);
            return Ok(new { userId = GetUserId(), updatedUser = result });
        }

        /// <summary>Delete user by ID</summary>
        /// <remarks>Soft delete user with given ID</remarks>
        /// <param name="id">User deleting ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id)
        {
            UserDto result = _userService.DeleteById(id);
            return Ok(new { userId = GetUserId(), deletedUser = result });
        }
    }
}