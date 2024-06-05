using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoundSphere.Api.Controllers;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.NotificationMock;

namespace SoundSphere.Tests.Unit.Controllers
{
    public class NotificationControllerTest
    {
        private readonly Mock<INotificationService> _notificationServiceMock = new();
        private readonly NotificationController _notificationController;

        private readonly NotificationDto _notificationDto1 = GetMockedNotificationDto1();
        private readonly NotificationDto _notificationDto2 = GetMockedNotificationDto2();
        private readonly IList<NotificationDto> _notificationDtos = GetMockedNotificationDtos();
        private readonly IList<NotificationDto> _paginatedNotificationDtos = GetMockedPaginatedNotificationDtos();
        private readonly NotificationPaginationRequest _paginationRequest = GetMockedNotificationsPaginationRequest();

        public NotificationControllerTest() => _notificationController = new(_notificationServiceMock.Object);

        [Fact] public void GetAllPagination_Test()
        {
            _notificationServiceMock.Setup(mock => mock.GetAll(_paginationRequest)).Returns(_paginatedNotificationDtos);
            OkObjectResult? result = _notificationController.GetAll(_paginationRequest) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_paginatedNotificationDtos);
        }

        [Fact] public void GetById_Test()
        {
            _notificationServiceMock.Setup(mock => mock.GetById(ValidNotificationGuid)).Returns(_notificationDto1);
            OkObjectResult? result = _notificationController.GetById(ValidNotificationGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_notificationDto1);
        }

        [Fact] public void Add_Test()
        {
            _notificationServiceMock.Setup(mock => mock.Add(_notificationDto1)).Returns(_notificationDto1);
            CreatedAtActionResult? result = _notificationController.Add(_notificationDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status201Created);
            result?.Value.Should().Be(_notificationDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            NotificationDto updatedNotificationDto = new NotificationDto
            {
                Id = ValidNotificationGuid,
                UserId = _notificationDto1.UserId,
                Type = _notificationDto2.Type,
                Message = _notificationDto2.Message,
                IsRead = _notificationDto2.IsRead,
                CreatedAt = _notificationDto1.CreatedAt
            };
            _notificationServiceMock.Setup(mock => mock.UpdateById(_notificationDto2, ValidNotificationGuid)).Returns(updatedNotificationDto);
            OkObjectResult? result = _notificationController.UpdateById(_notificationDto2, ValidNotificationGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(updatedNotificationDto);
        }

        [Fact] public void DeleteById_Test()
        {
            NoContentResult? result = _notificationController.DeleteById(ValidNotificationGuid) as NoContentResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status204NoContent);
        }
    }
}