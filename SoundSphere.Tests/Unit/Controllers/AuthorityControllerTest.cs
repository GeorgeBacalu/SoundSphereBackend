using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoundSphere.Api.Controllers;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database;
using SoundSphere.Database.Dtos;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Controllers
{
    public class AuthorityControllerTest
    {
        private readonly Mock<IAuthorityService> _authorityServiceMock = new();
        private readonly AuthorityController _authorityController;

        private readonly AuthorityDto _authorityDto1 = AuthorityMock.GetMockedAuthorityDto1();
        private readonly IList<AuthorityDto> _authorityDtos = AuthorityMock.GetMockedAuthorityDtos();

        public AuthorityControllerTest() => _authorityController = new(_authorityServiceMock.Object);

        [Fact] public void FindAll_Test()
        {
            _authorityServiceMock.Setup(mock => mock.FindAll()).Returns(_authorityDtos);
            OkObjectResult? result = _authorityController.FindAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_authorityDtos);
        }

        [Fact] public void FindById_Test()
        {
            _authorityServiceMock.Setup(mock => mock.FindById(Constants.ValidAuthorityGuid)).Returns(_authorityDto1);
            OkObjectResult? result = _authorityController.FindById(Constants.ValidAuthorityGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_authorityDto1);
        }

        [Fact] public void Save_Test()
        {
            _authorityServiceMock.Setup(mock => mock.Save(_authorityDto1)).Returns(_authorityDto1);
            CreatedAtActionResult? result = _authorityController.Save(_authorityDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status201Created);
            result?.Value.Should().Be(_authorityDto1);
        }
    }
}