using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;
using System.Net.Mime;

namespace SoundSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class AuthorityController : ControllerBase
    {
        private readonly IAuthorityService _authorityService;

        public AuthorityController(IAuthorityService authorityService) => _authorityService = authorityService;

        /// <summary>Find all authorities</summary>
        /// <remarks>Return list with all authorities</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet] public IActionResult FindAll() => Ok(_authorityService.FindAll());

        /// <summary>Find authority by ID</summary>
        /// <remarks>Return authority with given ID</remarks>
        /// <param name="id">Authority fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult FindById(Guid id) => Ok(_authorityService.FindById(id));

        /// <summary>Save authority</summary>
        /// <remarks>Save new authority</remarks>
        /// <param name="authorityDto">Authority to save</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Save(AuthorityDto authorityDto)
        {
            AuthorityDto savedAuthorityDto = _authorityService.Save(authorityDto);
            return CreatedAtAction(nameof(FindById), new { id = savedAuthorityDto.Id }, savedAuthorityDto);
        }
    }
}