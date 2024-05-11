using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Services;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Services
{
    public class FeedbackServiceTest
    {
        private readonly Mock<IFeedbackRepository> _feedbackRepositoryMock = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly IFeedbackService _feedbackService;

        private readonly Feedback _feedback1 = FeedbackMock.GetMockedFeedback1();
        private readonly Feedback _feedback2 = FeedbackMock.GetMockedFeedback2();
        private readonly IList<Feedback> _feedbacks = FeedbackMock.GetMockedFeedbacks();
        private readonly IList<Feedback> _paginatedFeedbacks = FeedbackMock.GetMockedPaginatedFeedbacks();
        private readonly FeedbackDto _feedbackDto1 = FeedbackMock.GetMockedFeedbackDto1();
        private readonly FeedbackDto _feedbackDto2 = FeedbackMock.GetMockedFeedbackDto2();
        private readonly IList<FeedbackDto> _feedbackDtos = FeedbackMock.GetMockedFeedbackDtos();
        private readonly IList<FeedbackDto> _paginatedFeedbackDtos = FeedbackMock.GetMockedPaginatedFeedbackDtos();
        private readonly FeedbackPaginationRequest _paginationRequest = FeedbackMock.GetMockedPaginationRequest();
        private readonly User _user1 = UserMock.GetMockedUser1();

        public FeedbackServiceTest()
        {
            _mapperMock.Setup(mock => mock.Map<FeedbackDto>(_feedback1)).Returns(_feedbackDto1);
            _mapperMock.Setup(mock => mock.Map<FeedbackDto>(_feedback2)).Returns(_feedbackDto2);
            _mapperMock.Setup(mock => mock.Map<Feedback>(_feedbackDto1)).Returns(_feedback1);
            _mapperMock.Setup(mock => mock.Map<Feedback>(_feedbackDto2)).Returns(_feedback2);
            _feedbackService = new FeedbackService(_feedbackRepositoryMock.Object, _userRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact] public void FindAll_Test()
        {
            _feedbackRepositoryMock.Setup(mock => mock.FindAll()).Returns(_feedbacks);
            _feedbackService.FindAll().Should().BeEquivalentTo(_feedbackDtos);
        }

        [Fact] public void FindAllPagination_Test()
        {
            _feedbackRepositoryMock.Setup(mock => mock.FindAllPagination(_paginationRequest)).Returns(_paginatedFeedbacks);
            _feedbackService.FindAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedFeedbackDtos);
        }

        [Fact] public void FindById_Test()
        {
            _feedbackRepositoryMock.Setup(mock => mock.FindById(Constants.ValidFeedbackGuid)).Returns(_feedback1);
            _feedbackService.FindById(Constants.ValidFeedbackGuid).Should().Be(_feedbackDto1);
        }

        [Fact] public void Save_Test()
        {
            _userRepositoryMock.Setup(mock => mock.FindById(Constants.ValidUserGuid)).Returns(_user1);
            _feedbackRepositoryMock.Setup(mock => mock.Save(_feedback1)).Returns(_feedback1);
            _feedbackService.Save(_feedbackDto1).Should().Be(_feedbackDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            Feedback updatedFeedback = new Feedback
            {
                Id = Constants.ValidFeedbackGuid,
                User = _feedback1.User,
                Type = _feedback2.Type,
                Message = _feedback2.Message,
                SentAt = _feedback1.SentAt
            };
            FeedbackDto updatedFeedbackDto = ToDto(updatedFeedback);
            _mapperMock.Setup(mock => mock.Map<FeedbackDto>(updatedFeedback)).Returns(updatedFeedbackDto);
            _feedbackRepositoryMock.Setup(mock => mock.UpdateById(_feedback2, Constants.ValidFeedbackGuid)).Returns(updatedFeedback);
            _feedbackService.UpdateById(_feedbackDto2, Constants.ValidFeedbackGuid).Should().Be(updatedFeedbackDto);
        }

        [Fact] public void DeleteById_Test()
        {
            _feedbackService.DeleteById(Constants.ValidFeedbackGuid);
            _feedbackRepositoryMock.Verify(mock => mock.DeleteById(Constants.ValidFeedbackGuid));
        }

        private FeedbackDto ToDto(Feedback feedback) => new FeedbackDto
        {
            Id = feedback.Id,
            UserId = feedback.User.Id,
            Type = feedback.Type,
            Message = feedback.Message,
            SentAt = feedback.SentAt
        };
    }
}