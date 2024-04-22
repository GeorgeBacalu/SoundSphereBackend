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
    public class NotificationControllerTest
    {
        private readonly Mock<INotificationService> _notificationServiceMock = new();
        private readonly NotificationController _notificationController;

        private readonly NotificationDto _notificationDto1 = NotificationMock.GetMockedNotificationDto1();
        private readonly NotificationDto _notificationDto2 = NotificationMock.GetMockedNotificationDto2();
        private readonly IList<NotificationDto> _notificationDtos = NotificationMock.GetMockedNotificationDtos();

        public NotificationControllerTest() => _notificationController = new(_notificationServiceMock.Object);

        [Fact] public void FindAll_Test()
        {
            _notificationServiceMock.Setup(mock => mock.FindAll()).Returns(_notificationDtos);
            OkObjectResult? result = _notificationController.FindAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_notificationDtos);
        }

        [Fact] public void FindById_Test()
        {
            _notificationServiceMock.Setup(mock => mock.FindById(Constants.ValidNotificationGuid)).Returns(_notificationDto1);
            OkObjectResult? result = _notificationController.FindById(Constants.ValidNotificationGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_notificationDto1);
        }

        [Fact] public void Save_Test()
        {
            _notificationServiceMock.Setup(mock => mock.Save(_notificationDto1)).Returns(_notificationDto1);
            CreatedAtActionResult? result = _notificationController.Save(_notificationDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status201Created);
            result?.Value.Should().Be(_notificationDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            NotificationDto updatedNotificationDto = new NotificationDto
            {
                Id = Constants.ValidNotificationGuid,
                UserId = _notificationDto1.UserId,
                Type = _notificationDto2.Type,
                Message = _notificationDto2.Message,
                SentAt = _notificationDto1.SentAt,
                IsRead = _notificationDto2.IsRead
            };
            _notificationServiceMock.Setup(mock => mock.UpdateById(_notificationDto2, Constants.ValidNotificationGuid)).Returns(updatedNotificationDto);
            OkObjectResult? result = _notificationController.UpdateById(_notificationDto2, Constants.ValidNotificationGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(updatedNotificationDto);
        }

        [Fact] public void DeleteById_Test()
        {
            NoContentResult? result = _notificationController.DeleteById(Constants.ValidNotificationGuid) as NoContentResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }
    }
}