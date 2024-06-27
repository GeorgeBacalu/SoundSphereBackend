using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoundSphere.Api.Controllers;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.AuthorityMock;

namespace SoundSphere.Tests.Unit.Controllers
{
    public class AuthorityControllerTest
    {
        private readonly Mock<IAuthorityService> _authorityServiceMock = new();
        private readonly AuthorityController _authorityController;

        private readonly AuthorityDto _authorityDto1 = GetMockedAuthorityDto1();
        private readonly IList<AuthorityDto> _authorityDtos = GetMockedAuthorityDtosAdmin();

        public AuthorityControllerTest() => _authorityController = new(_authorityServiceMock.Object);

        [Fact] public void GetAll_Test()
        {
            _authorityServiceMock.Setup(mock => mock.GetAll()).Returns(_authorityDtos);
            OkObjectResult? result = _authorityController.GetAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_authorityDtos);
        }

        [Fact] public void GetById_Test()
        {
            _authorityServiceMock.Setup(mock => mock.GetById(ValidAuthorityGuid)).Returns(_authorityDto1);
            OkObjectResult? result = _authorityController.GetById(ValidAuthorityGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_authorityDto1);
        }

        [Fact] public void Add_Test()
        {
            _authorityServiceMock.Setup(mock => mock.Add(_authorityDto1)).Returns(_authorityDto1);
            CreatedAtActionResult? result = _authorityController.Add(_authorityDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status201Created);
            result?.Value.Should().Be(_authorityDto1);
        }
    }
}