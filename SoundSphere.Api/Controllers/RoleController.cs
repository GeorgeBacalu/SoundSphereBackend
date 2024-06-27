using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using System.Net.Mime;

namespace SoundSphere.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService) => _roleService = roleService;

        /// <summary>Get all roles</summary>
        /// <remarks>Return list with all roles</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet] public IActionResult GetAll()
        {
            IList<RoleDto> roleDtos = _roleService.GetAll();
            return Ok(roleDtos);
        }

        /// <summary>Get role by ID</summary>
        /// <remarks>Return role with given ID</remarks>
        /// <param name="id">Role fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id)
        {
            RoleDto roleDto = _roleService.GetById(id);
            return Ok(roleDto);
        }

        /// <summary>Add role</summary>
        /// <remarks>Add new role</remarks>
        /// <param name="roleDto">Role to add</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Add(RoleDto roleDto)
        {
            RoleDto createdRoleDto = _roleService.Add(roleDto);
            return CreatedAtAction(nameof(GetById), new { createdRoleDto.Id }, createdRoleDto);
        }
    }
}