using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using System.Net.Mime;

namespace SoundSphere.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class AuthorityController : BaseController
    {
        private readonly IAuthorityService _authorityService;

        public AuthorityController(IAuthorityService authorityService) => _authorityService = authorityService;

        /// <summary>Get all authorities</summary>
        /// <remarks>Return list with all authorities</remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet] public IActionResult GetAll() => Ok(_authorityService.GetAll());

        /// <summary>Get authority by ID</summary>
        /// <remarks>Return authority with given ID</remarks>
        /// <param name="id">Authority fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id) => Ok(_authorityService.GetById(id));

        /// <summary>Add authority</summary>
        /// <remarks>Add new authority</remarks>
        /// <param name="authorityDto">Authority to add</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Add(AuthorityDto authorityDto)
        {
            AuthorityDto createdAuthorityDto = _authorityService.Add(authorityDto);
            return CreatedAtAction(nameof(GetById), new { id = createdAuthorityDto.Id }, createdAuthorityDto);
        }
    }
}