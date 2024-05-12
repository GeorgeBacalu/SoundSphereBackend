using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using System.Net.Mime;

namespace SoundSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService) => _roleService = roleService;

        /// <summary>Find all roles</summary>
        /// <remarks>Return list with all roles</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet] public IActionResult FindAll() => Ok(_roleService.FindAll());

        /// <summary>Find role by ID</summary>
        /// <remarks>Return role with given ID</remarks>
        /// <param name="id">Role fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult FindById(Guid id) => Ok(_roleService.FindById(id));

        /// <summary>Save role</summary>
        /// <remarks>Save new role</remarks>
        /// <param name="roleDto">Role to save</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Save(RoleDto roleDto)
        {
            RoleDto savedRoleDto = _roleService.Save(roleDto);
            return CreatedAtAction(nameof(FindById), new { id = savedRoleDto.Id }, savedRoleDto);
        }
    }
}