using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Services;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Services
{
    public class FeedbackServiceTest
    {
        private readonly Mock<IFeedbackRepository> _feedbackRepository = new();
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly IFeedbackService _feedbackService;

        private readonly Feedback _feedback1 = FeedbackMock.GetMockedFeedback1();
        private readonly Feedback _feedback2 = FeedbackMock.GetMockedFeedback2();
        private readonly IList<Feedback> _feedbacks = FeedbackMock.GetMockedFeedbacks();
        private readonly FeedbackDto _feedbackDto1 = FeedbackMock.GetMockedFeedbackDto1();
        private readonly FeedbackDto _feedbackDto2 = FeedbackMock.GetMockedFeedbackDto2();
        private readonly IList<FeedbackDto> _feedbackDtos = FeedbackMock.GetMockedFeedbackDtos();
        private readonly User _user1 = UserMock.GetMockedUser1();

        public FeedbackServiceTest()
        {
            _mapper.Setup(mock => mock.Map<FeedbackDto>(_feedback1)).Returns(_feedbackDto1);
            _mapper.Setup(mock => mock.Map<FeedbackDto>(_feedback2)).Returns(_feedbackDto2);
            _mapper.Setup(mock => mock.Map<Feedback>(_feedbackDto1)).Returns(_feedback1);
            _mapper.Setup(mock => mock.Map<Feedback>(_feedbackDto2)).Returns(_feedback2);
            _feedbackService = new FeedbackService(_feedbackRepository.Object, _userRepository.Object, _mapper.Object);
        }

        [Fact] public void FindAll_Test()
        {
            _feedbackRepository.Setup(mock => mock.FindAll()).Returns(_feedbacks);
            _feedbackService.FindAll().Should().BeEquivalentTo(_feedbackDtos);
        }

        [Fact] public void FindById_Test()
        {
            _feedbackRepository.Setup(mock => mock.FindById(Constants.ValidFeedbackGuid)).Returns(_feedback1);
            _feedbackService.FindById(Constants.ValidFeedbackGuid).Should().BeEquivalentTo(_feedbackDto1);
        }

        [Fact] public void Save_Test()
        {
            _userRepository.Setup(mock => mock.FindById(Constants.ValidUserGuid)).Returns(_user1);
            _feedbackRepository.Setup(mock => mock.Save(_feedback1)).Returns(_feedback1);
            _feedbackService.Save(_feedbackDto1).Should().BeEquivalentTo(_feedbackDto1);
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
            FeedbackDto updatedFeedbackDto = ConvertToDto(updatedFeedback);
            _mapper.Setup(mock => mock.Map<FeedbackDto>(updatedFeedback)).Returns(updatedFeedbackDto);
            _feedbackRepository.Setup(mock => mock.UpdateById(_feedback2, Constants.ValidFeedbackGuid)).Returns(updatedFeedback);
            _feedbackService.UpdateById(_feedbackDto2, Constants.ValidFeedbackGuid).Should().BeEquivalentTo(updatedFeedbackDto);
        }

        [Fact] public void DeleteById_Test()
        {
            _feedbackService.DeleteById(Constants.ValidFeedbackGuid);
            _feedbackRepository.Verify(mock => mock.DeleteById(Constants.ValidFeedbackGuid));
        }

        private FeedbackDto ConvertToDto(Feedback feedback) => new FeedbackDto
        {
            Id = feedback.Id,
            UserId = feedback.User.Id,
            Type = feedback.Type,
            Message = feedback.Message,
            SentAt = feedback.SentAt
        };
    }
}