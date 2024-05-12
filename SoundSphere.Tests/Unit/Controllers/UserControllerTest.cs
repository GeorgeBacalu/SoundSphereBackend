using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoundSphere.Api.Controllers;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Controllers
{
    public class UserControllerTest
    {
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly UserController _userController;

        private readonly UserDto _userDto1 = UserMock.GetMockedUserDto1();
        private readonly UserDto _userDto2 = UserMock.GetMockedUserDto2();
        private readonly IList<UserDto> _userDtos = UserMock.GetMockedUserDtos();
        private readonly IList<UserDto> _activeUserDtos = UserMock.GetMockedActiveUserDtos();
        private readonly IList<UserDto> _paginatedUserDtos = UserMock.GetMockedPaginatedUserDtos();
        private readonly IList<UserDto> _activePaginatedUserDtos = UserMock.GetMockedActivePaginatedUserDtos();
        private readonly UserPaginationRequest _paginationRequest = UserMock.GetMockedPaginationRequest();

        public UserControllerTest() => _userController = new(_userServiceMock.Object);

        [Fact] public void FindAll_Test()
        {
            _userServiceMock.Setup(mock => mock.FindAll()).Returns(_userDtos);
            OkObjectResult? result = _userController.FindAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_userDtos);
        }

        [Fact] public void FindAllActive_Test()
        {
            _userServiceMock.Setup(mock => mock.FindAllActive()).Returns(_activeUserDtos);
            OkObjectResult? result = _userController.FindAllActive() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_activeUserDtos);
        }

        [Fact] public void FindAllPagination_Test()
        {
            _userServiceMock.Setup(mock => mock.FindAllPagination(_paginationRequest)).Returns(_paginatedUserDtos);
            OkObjectResult? result = _userController.FindAllPagination(_paginationRequest) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_paginatedUserDtos);
        }

        [Fact] public void FindAllActivePagination_Test()
        {
            _userServiceMock.Setup(mock => mock.FindAllActivePagination(_paginationRequest)).Returns(_activePaginatedUserDtos);
            OkObjectResult? result = _userController.FindAllActivePagination(_paginationRequest) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_activePaginatedUserDtos);
        }

        [Fact] public void FindById_Test()
        {
            _userServiceMock.Setup(mock => mock.FindById(Constants.ValidUserGuid)).Returns(_userDto1);
            OkObjectResult? result = _userController.FindById(Constants.ValidUserGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_userDto1);
        }

        [Fact] public void Save_Test()
        {
            _userServiceMock.Setup(mock => mock.Save(_userDto1)).Returns(_userDto1);
            CreatedAtActionResult? result = _userController.Save(_userDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status201Created);
            result?.Value.Should().Be(_userDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            UserDto updatedUserDto = GetUserDto(_userDto2, _userDto1.IsActive);
            _userServiceMock.Setup(mock => mock.UpdateById(_userDto2, Constants.ValidUserGuid)).Returns(updatedUserDto);
            OkObjectResult? result = _userController.UpdateById(_userDto2, Constants.ValidUserGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(updatedUserDto);
        }

        [Fact] public void DisableById_Test()
        {
            UserDto disabledUserDto = GetUserDto(_userDto1, false);
            _userServiceMock.Setup(mock => mock.DisableById(Constants.ValidUserGuid)).Returns(disabledUserDto);
            OkObjectResult? result = _userController.DisableById(Constants.ValidUserGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(disabledUserDto);
        }

        private UserDto GetUserDto(UserDto userDto, bool isActive) => new UserDto
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