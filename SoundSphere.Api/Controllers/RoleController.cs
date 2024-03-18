using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;

namespace SoundSphereV2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService) => _roleService = roleService;

        [HttpGet] public IActionResult FindAll() => Ok(_roleService.FindAll());

        [HttpGet("{id}")] public IActionResult FindById(Guid id) => Ok(_roleService.FindById(id));

        [HttpPost] public IActionResult Save(RoleDto roleDto)
        {
            RoleDto savedRoleDto = _roleService.Save(roleDto);
            return CreatedAtAction(nameof(FindById), new { id = savedRoleDto.Id }, savedRoleDto);
        }
    }
}