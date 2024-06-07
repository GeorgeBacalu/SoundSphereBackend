using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoundSphere.Api.Controllers;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.FeedbackMock;

namespace SoundSphere.Tests.Unit.Controllers
{
    public class FeedbackControllerTest
    {
        private readonly Mock<IFeedbackService> _feedbackServiceMock = new();
        private readonly FeedbackController _feedbackController;

        private readonly FeedbackDto _feedbackDto1 = GetMockedFeedbackDto1();
        private readonly FeedbackDto _feedbackDto2 = GetMockedFeedbackDto2();
        private readonly IList<FeedbackDto> _feedbackDtos = GetMockedFeedbackDtos();
        private readonly IList<FeedbackDto> _paginatedFeedbackDtos = GetMockedPaginatedFeedbackDtos();
        private readonly FeedbackPaginationRequest _paginationRequest = GetMockedFeedbacksPaginationRequest();

        public FeedbackControllerTest() => _feedbackController = new(_feedbackServiceMock.Object);

        [Fact] public void GetAll_Test()
        {
            _feedbackServiceMock.Setup(mock => mock.GetAll(_paginationRequest)).Returns(_paginatedFeedbackDtos);
            OkObjectResult? result = _feedbackController.GetAll(_paginationRequest) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_paginatedFeedbackDtos);
        }

        [Fact] public void GetById_Test()
        {
            _feedbackServiceMock.Setup(mock => mock.GetById(ValidFeedbackGuid)).Returns(_feedbackDto1);
            OkObjectResult? result = _feedbackController.GetById(ValidFeedbackGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_feedbackDto1);
        }

        [Fact] public void Add_Test()
        {
            _feedbackServiceMock.Setup(mock => mock.Add(_feedbackDto1)).Returns(_feedbackDto1);
            CreatedAtActionResult? result = _feedbackController.Add(_feedbackDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status201Created);
            result?.Value.Should().Be(_feedbackDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            FeedbackDto updatedFeedbackDto = new FeedbackDto
            {
                Id = ValidFeedbackGuid,
                UserId = _feedbackDto1.UserId,
                Type = _feedbackDto2.Type,
                Message = _feedbackDto2.Message,
                CreatedAt = _feedbackDto1.CreatedAt
            };
            _feedbackServiceMock.Setup(mock => mock.UpdateById(_feedbackDto2, ValidFeedbackGuid)).Returns(updatedFeedbackDto);
            OkObjectResult? result = _feedbackController.UpdateById(_feedbackDto2, ValidFeedbackGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(updatedFeedbackDto);
        }

        [Fact] public void DeleteById_Test()
        {
            NoContentResult? result = _feedbackController.DeleteById(ValidFeedbackGuid) as NoContentResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status204NoContent);
        }
    }
}