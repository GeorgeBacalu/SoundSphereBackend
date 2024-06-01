using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoundSphere.Api.Controllers;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.UserMock;

namespace SoundSphere.Tests.Unit.Controllers
{
    public class UserControllerTest
    {
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly UserController _userController;

        private readonly UserDto _userDto1 = GetMockedUserDto1();
        private readonly UserDto _userDto2 = GetMockedUserDto2();
        private readonly IList<UserDto> _userDtos = GetMockedUserDtos();
        private readonly IList<UserDto> _activeUserDtos = GetMockedActiveUserDtos();
        private readonly IList<UserDto> _paginatedUserDtos = GetMockedPaginatedUserDtos();
        private readonly IList<UserDto> _activePaginatedUserDtos = GetMockedActivePaginatedUserDtos();
        private readonly UserPaginationRequest _paginationRequest = GetMockedUsersPaginationRequest();

        public UserControllerTest() => _userController = new(_userServiceMock.Object);

        [Fact] public void GetAll_Test()
        {
            _userServiceMock.Setup(mock => mock.GetAll()).Returns(_userDtos);
            OkObjectResult? result = _userController.GetAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_userDtos);
        }

        [Fact] public void GetAllActive_Test()
        {
            _userServiceMock.Setup(mock => mock.GetAllActive()).Returns(_activeUserDtos);
            OkObjectResult? result = _userController.GetAllActive() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_activeUserDtos);
        }

        [Fact] public void GetAllPagination_Test()
        {
            _userServiceMock.Setup(mock => mock.GetAllPagination(_paginationRequest)).Returns(_paginatedUserDtos);
            OkObjectResult? result = _userController.GetAllPagination(_paginationRequest) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_paginatedUserDtos);
        }

        [Fact] public void GetAllActivePagination_Test()
        {
            _userServiceMock.Setup(mock => mock.GetAllActivePagination(_paginationRequest)).Returns(_activePaginatedUserDtos);
            OkObjectResult? result = _userController.GetAllActivePagination(_paginationRequest) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_activePaginatedUserDtos);
        }

        [Fact] public void GetById_Test()
        {
            _userServiceMock.Setup(mock => mock.GetById(ValidUserGuid)).Returns(_userDto1);
            OkObjectResult? result = _userController.GetById(ValidUserGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_userDto1);
        }

        [Fact] public void Add_Test()
        {
            _userServiceMock.Setup(mock => mock.Add(_userDto1)).Returns(_userDto1);
            CreatedAtActionResult? result = _userController.Add(_userDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status201Created);
            result?.Value.Should().Be(_userDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            UserDto updatedUserDto = GetUserDto(_userDto2, _userDto1.IsActive);
            _userServiceMock.Setup(mock => mock.UpdateById(_userDto2, ValidUserGuid)).Returns(updatedUserDto);
            OkObjectResult? result = _userController.UpdateById(_userDto2, ValidUserGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(updatedUserDto);
        }

        [Fact] public void DeleteById_Test()
        {
            UserDto deletedUserDto = GetUserDto(_userDto1, false);
            _userServiceMock.Setup(mock => mock.DeleteById(ValidUserGuid)).Returns(deletedUserDto);
            OkObjectResult? result = _userController.DeleteById(ValidUserGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(deletedUserDto);
        }

        private UserDto GetUserDto(UserDto userDto, bool isActive) => new UserDto
        {
            Id = ValidUserGuid,
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