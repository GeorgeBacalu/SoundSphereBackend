using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Services;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Integration.Services
{
    public class FeedbackServiceIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly IMapper _mapper;

        private readonly Feedback _feedback1 = FeedbackMock.GetMockedFeedback1();
        private readonly Feedback _feedback2 = FeedbackMock.GetMockedFeedback2();
        private readonly IList<Feedback> _feedbacks = FeedbackMock.GetMockedFeedbacks();
        private readonly FeedbackDto _feedbackDto1 = FeedbackMock.GetMockedFeedbackDto1();
        private readonly FeedbackDto _feedbackDto2 = FeedbackMock.GetMockedFeedbackDto2();
        private readonly IList<FeedbackDto> _feedbackDtos = FeedbackMock.GetMockedFeedbackDtos();

        public FeedbackServiceIntegrationTest(DbFixture fixture)
        {
            _fixture = fixture;
            _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Feedback, FeedbackDto>();
                config.CreateMap<FeedbackDto, Feedback>();
            }).CreateMapper();
        }

        private void Execute(Action<FeedbackService, SoundSphereContext> action)
        {
            using var context = _fixture.CreateContext();
            var feedbackService = new FeedbackService(new FeedbackRepository(context), new UserRepository(context), _mapper);
            using var transaction = context.Database.BeginTransaction();
            context.AddRange(_feedbacks);
            context.SaveChanges();
            action(feedbackService, context);
        }

        [Fact] public void FindAll_Test() => Execute((feedbackService, context) => feedbackService.FindAll().Should().BeEquivalentTo(_feedbackDtos));

        [Fact] public void FindById_Test() => Execute((feedbackService, context) => feedbackService.FindById(Constants.ValidFeedbackGuid).Should().BeEquivalentTo(_feedbackDto1));

        [Fact] public void Save_Test() => Execute((feedbackService, context) =>
        {
            FeedbackDto newFeedbackDto = FeedbackMock.GetMockedFeedbackDto3();
            feedbackService.Save(newFeedbackDto);
            context.Feedbacks.Find(newFeedbackDto.Id).Should().BeEquivalentTo(FeedbackMock.GetMockedFeedback3(), options => options.Excluding(feedback => feedback.SentAt));
        });

        [Fact] public void UpdateById_Test() => Execute((feedbackService, context) =>
        {
            Feedback updatedFeedback = new Feedback
            {
                Id = Constants.ValidFeedbackGuid,
                User = _feedback1.User,
                Type = _feedback2.Type,
                Message = _feedback2.Message,
                SentAt = _feedback1.SentAt
            };
            FeedbackDto updatedFeedbackDto = feedbackService.ConvertToDto(updatedFeedback);
            feedbackService.UpdateById(_feedbackDto2, Constants.ValidFeedbackGuid);
            context.Feedbacks.Find(Constants.ValidFeedbackGuid).Should().BeEquivalentTo(updatedFeedback);
        });

        [Fact] public void DeleteById_Test() => Execute((feedbackService, context) =>
        {
            feedbackService.DeleteById(Constants.ValidFeedbackGuid);
            context.Feedbacks.Should().BeEquivalentTo(new List<Feedback> { _feedback2 });
        });
    }
}