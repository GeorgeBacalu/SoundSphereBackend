using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoundSphere.Api.Controllers;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Controllers
{
    public class RoleControllerTest
    {
        private readonly Mock<IRoleService> _roleServiceMock = new();
        private readonly RoleController _roleController;

        private readonly RoleDto _roleDto1 = RoleMock.GetMockedRoleDto1();
        private readonly IList<RoleDto> _roleDtos = RoleMock.GetMockedRoleDtos();

        public RoleControllerTest() => _roleController = new(_roleServiceMock.Object);

        [Fact] public void FindAll_Test()
        {
            _roleServiceMock.Setup(mock => mock.FindAll()).Returns(_roleDtos);
            OkObjectResult? result = _roleController.FindAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_roleDtos);
        }

        [Fact] public void FindById_Test()
        {
            _roleServiceMock.Setup(mock => mock.FindById(Constants.ValidRoleGuid)).Returns(_roleDto1);
            OkObjectResult? result = _roleController.FindById(Constants.ValidRoleGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_roleDto1);
        }

        [Fact] public void Save_Test()
        {
            _roleServiceMock.Setup(mock => mock.Save(_roleDto1)).Returns(_roleDto1);
            CreatedAtActionResult? result = _roleController.Save(_roleDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status201Created);
            result?.Value.Should().Be(_roleDto1);
        }
    }
}