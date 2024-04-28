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
    public class FeedbackControllerTest
    {
        private readonly Mock<IFeedbackService> _feedbackServiceMock = new();
        private readonly FeedbackController _feedbackController;

        private readonly FeedbackDto _feedbackDto1 = FeedbackMock.GetMockedFeedbackDto1();
        private readonly FeedbackDto _feedbackDto2 = FeedbackMock.GetMockedFeedbackDto2();
        private readonly IList<FeedbackDto> _feedbackDtos = FeedbackMock.GetMockedFeedbackDtos();

        public FeedbackControllerTest() => _feedbackController = new(_feedbackServiceMock.Object);

        [Fact] public void FindAll_Test()
        {
            _feedbackServiceMock.Setup(mock => mock.FindAll()).Returns(_feedbackDtos);
            OkObjectResult? result = _feedbackController.FindAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_feedbackDtos);
        }

        [Fact] public void FindById_Test()
        {
            _feedbackServiceMock.Setup(mock => mock.FindById(Constants.ValidFeedbackGuid)).Returns(_feedbackDto1);
            OkObjectResult? result = _feedbackController.FindById(Constants.ValidFeedbackGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_feedbackDto1);
        }

        [Fact] public void Save_Test()
        {
            _feedbackServiceMock.Setup(mock => mock.Save(_feedbackDto1)).Returns(_feedbackDto1);
            CreatedAtActionResult? result = _feedbackController.Save(_feedbackDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status201Created);
            result?.Value.Should().Be(_feedbackDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            FeedbackDto updatedFeedbackDto = new FeedbackDto
            {
                Id = Constants.ValidFeedbackGuid,
                UserId = _feedbackDto1.UserId,
                Type = _feedbackDto2.Type,
                Message = _feedbackDto2.Message,
                SentAt = _feedbackDto1.SentAt
            };
            _feedbackServiceMock.Setup(mock => mock.UpdateById(_feedbackDto2, Constants.ValidFeedbackGuid)).Returns(updatedFeedbackDto);
            OkObjectResult? result = _feedbackController.UpdateById(_feedbackDto2, Constants.ValidFeedbackGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(updatedFeedbackDto);
        }

        [Fact] public void DeleteById_Test()
        {
            NoContentResult? result = _feedbackController.DeleteById(Constants.ValidFeedbackGuid) as NoContentResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }
    }
}