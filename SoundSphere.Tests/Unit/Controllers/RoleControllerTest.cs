using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoundSphere.Api.Controllers;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.RoleMock;

namespace SoundSphere.Tests.Unit.Controllers
{
    public class RoleControllerTest
    {
        private readonly Mock<IRoleService> _roleServiceMock = new();
        private readonly RoleController _roleController;

        private readonly RoleDto _roleDto1 = GetMockedRoleDto1();
        private readonly IList<RoleDto> _roleDtos = GetMockedRoleDtos();

        public RoleControllerTest() => _roleController = new(_roleServiceMock.Object);

        [Fact] public void GetAll_Test()
        {
            _roleServiceMock.Setup(mock => mock.GetAll()).Returns(_roleDtos);
            OkObjectResult? result = _roleController.GetAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_roleDtos);
        }

        [Fact] public void GetById_Test()
        {
            _roleServiceMock.Setup(mock => mock.GetById(ValidRoleGuid)).Returns(_roleDto1);
            OkObjectResult? result = _roleController.GetById(ValidRoleGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_roleDto1);
        }

        [Fact] public void Add_Test()
        {
            _roleServiceMock.Setup(mock => mock.Add(_roleDto1)).Returns(_roleDto1);
            CreatedAtActionResult? result = _roleController.Add(_roleDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status201Created);
            result?.Value.Should().Be(_roleDto1);
        }
    }
}