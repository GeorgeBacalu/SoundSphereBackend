using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Services;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.FeedbackMock;
using static SoundSphere.Tests.Mocks.UserMock;

namespace SoundSphere.Tests.Unit.Services
{
    public class FeedbackServiceTest
    {
        private readonly Mock<IFeedbackRepository> _feedbackRepositoryMock = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly IFeedbackService _feedbackService;

        private readonly Feedback _feedback1 = GetMockedFeedback1();
        private readonly Feedback _feedback2 = GetMockedFeedback2();
        private readonly IList<Feedback> _feedbacks = GetMockedFeedbacks();
        private readonly IList<Feedback> _paginatedFeedbacks = GetMockedPaginatedFeedbacks();
        private readonly FeedbackDto _feedbackDto1 = GetMockedFeedbackDto1();
        private readonly FeedbackDto _feedbackDto2 = GetMockedFeedbackDto2();
        private readonly IList<FeedbackDto> _feedbackDtos = GetMockedFeedbackDtos();
        private readonly IList<FeedbackDto> _paginatedFeedbackDtos = GetMockedPaginatedFeedbackDtos();
        private readonly FeedbackPaginationRequest _paginationRequest = GetMockedFeedbacksPaginationRequest();
        private readonly User _user1 = GetMockedUser1();

        public FeedbackServiceTest()
        {
            _mapperMock.Setup(mock => mock.Map<FeedbackDto>(_feedback1)).Returns(_feedbackDto1);
            _mapperMock.Setup(mock => mock.Map<FeedbackDto>(_feedback2)).Returns(_feedbackDto2);
            _mapperMock.Setup(mock => mock.Map<Feedback>(_feedbackDto1)).Returns(_feedback1);
            _mapperMock.Setup(mock => mock.Map<Feedback>(_feedbackDto2)).Returns(_feedback2);
            _feedbackService = new FeedbackService(_feedbackRepositoryMock.Object, _userRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact] public void GetAll_Test()
        {
            _feedbackRepositoryMock.Setup(mock => mock.GetAll()).Returns(_feedbacks);
            _feedbackService.GetAll().Should().BeEquivalentTo(_feedbackDtos);
        }

        [Fact] public void GetAllPagination_Test()
        {
            _feedbackRepositoryMock.Setup(mock => mock.GetAllPagination(_paginationRequest)).Returns(_paginatedFeedbacks);
            _feedbackService.GetAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedFeedbackDtos);
        }

        [Fact] public void GetById_Test()
        {
            _feedbackRepositoryMock.Setup(mock => mock.GetById(ValidFeedbackGuid)).Returns(_feedback1);
            _feedbackService.GetById(ValidFeedbackGuid).Should().Be(_feedbackDto1);
        }

        [Fact] public void Add_Test()
        {
            _userRepositoryMock.Setup(mock => mock.GetById(ValidUserGuid)).Returns(_user1);
            _feedbackRepositoryMock.Setup(mock => mock.Add(_feedback1)).Returns(_feedback1);
            _feedbackService.Add(_feedbackDto1).Should().Be(_feedbackDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            Feedback updatedFeedback = new Feedback
            {
                Id = ValidFeedbackGuid,
                User = _feedback1.User,
                Type = _feedback2.Type,
                Message = _feedback2.Message,
                SentAt = _feedback1.SentAt
            };
            FeedbackDto updatedFeedbackDto = ToDto(updatedFeedback);
            _mapperMock.Setup(mock => mock.Map<FeedbackDto>(updatedFeedback)).Returns(updatedFeedbackDto);
            _feedbackRepositoryMock.Setup(mock => mock.UpdateById(_feedback2, ValidFeedbackGuid)).Returns(updatedFeedback);
            _feedbackService.UpdateById(_feedbackDto2, ValidFeedbackGuid).Should().Be(updatedFeedbackDto);
        }

        [Fact] public void DeleteById_Test()
        {
            _feedbackService.DeleteById(ValidFeedbackGuid);
            _feedbackRepositoryMock.Verify(mock => mock.DeleteById(ValidFeedbackGuid));
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