using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoundSphere.Api.Controllers;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Dtos;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Controllers
{
    public class UserControllerTest
    {
        private readonly Mock<IUserService> _userService = new();
        private readonly UserController _userController;

        private readonly UserDto _userDto1 = UserMock.GetMockedUserDto1();
        private readonly UserDto _userDto2 = UserMock.GetMockedUserDto2();
        private readonly IList<UserDto> _userDtos = UserMock.GetMockedUserDtos();
        private readonly IList<UserDto> _activeUserDtos = UserMock.GetMockedActiveUserDtos();

        public UserControllerTest() => _userController = new(_userService.Object);

        [Fact] public void FindAll_Test()
        {
            _userService.Setup(mock => mock.FindAll()).Returns(_userDtos);
            var result = _userController.FindAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_userDtos);
        }

        [Fact] public void FindAllActive_Test()
        {
            _userService.Setup(mock => mock.FindAllActive()).Returns(_activeUserDtos);
            var result = _userController.FindAllActive() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_activeUserDtos);
        }

        [Fact] public void FindById_Test()
        {
            _userService.Setup(mock => mock.FindById(Constants.ValidUserGuid)).Returns(_userDto1);
            var result = _userController.FindById(Constants.ValidUserGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_userDto1);
        }

        [Fact] public void Save_Test()
        {
            _userService.Setup(mock => mock.Save(_userDto1)).Returns(_userDto1);
            var result = _userController.Save(_userDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status201Created);
            result?.Value.Should().BeEquivalentTo(_userDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            UserDto updatedUserDto = CreateTestUserDto(_userDto2, _userDto1.IsActive);
            _userService.Setup(mock => mock.UpdateById(_userDto2, Constants.ValidUserGuid)).Returns(updatedUserDto);
            var result = _userController.UpdateById(_userDto2, Constants.ValidUserGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(updatedUserDto);
        }

        [Fact] public void DisableById_Test()
        {
            UserDto disabledUserDto = CreateTestUserDto(_userDto1, false);
            _userService.Setup(mock => mock.DisableById(Constants.ValidUserGuid)).Returns(disabledUserDto);
            var result = _userController.DisableById(Constants.ValidUserGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(disabledUserDto);
        }

        private UserDto CreateTestUserDto(UserDto userDto, bool isActive) => new UserDto
        {
            Id = Constants.ValidUserGuid,
            Name = userDto.Name,
            Email = userDto.Email,
            Mobile = userDto.Mobile,
            Address = userDto.Address,
            Birthday = userDto.Birthday,
            Avatar = userDto.Avatar,
            RoleId = userDto.RoleId,
            AuthoritiesIds = userDto.AuthoritiesIds,
            IsActive = isActive
        };
    }
}