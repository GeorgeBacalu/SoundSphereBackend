using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.FeedbackMock;

namespace SoundSphere.Tests.Integration.Services
{
    public class FeedbackServiceIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly IMapper _mapper;

        private readonly Feedback _feedback1 = GetMockedFeedback1();
        private readonly Feedback _feedback2 = GetMockedFeedback2();
        private readonly IList<Feedback> _feedbacks = GetMockedFeedbacks();
        private readonly FeedbackDto _feedbackDto1 = GetMockedFeedbackDto1();
        private readonly FeedbackDto _feedbackDto2 = GetMockedFeedbackDto2();
        private readonly IList<FeedbackDto> _feedbackDtos = GetMockedFeedbackDtos();
        private readonly IList<FeedbackDto> _paginatedFeedbackDtos = GetMockedPaginatedFeedbackDtos();
        private readonly FeedbackPaginationRequest _paginationRequest = GetMockedFeedbacksPaginationRequest();

        public FeedbackServiceIntegrationTest(DbFixture fixture) => (_fixture, _mapper) = (fixture, new MapperConfiguration(config => { config.CreateMap<Feedback, FeedbackDto>(); config.CreateMap<FeedbackDto, Feedback>(); }).CreateMapper());

        private void Execute(Action<FeedbackService, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var feedbackService = new FeedbackService(new FeedbackRepository(context), new UserRepository(context), _mapper);
            using var transaction = context.Database.BeginTransaction();
            context.AddRange(_feedbacks);
            context.SaveChanges();
            action(feedbackService, context);
            transaction.Rollback();
        }

        [Fact] public void GetAll_Test() => Execute((feedbackService, context) => feedbackService.GetAll().Should().BeEquivalentTo(_feedbackDtos));

        [Fact] public void GetAllPagination_Test() => Execute((feedbackService, context) => feedbackService.GetAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedFeedbackDtos));

        [Fact] public void GetById_Test() => Execute((feedbackService, context) => feedbackService.GetById(ValidFeedbackGuid).Should().Be(_feedbackDto1));

        [Fact] public void Add_Test() => Execute((feedbackService, context) =>
        {
            FeedbackDto newFeedbackDto = GetMockedFeedbackDto37();
            FeedbackDto result = feedbackService.Add(newFeedbackDto);
            context.Feedbacks.Find(newFeedbackDto.Id).Should().BeEquivalentTo(newFeedbackDto, options => options.Excluding(feedback => feedback.SentAt));
            result.Should().Be(newFeedbackDto);
        });

        [Fact] public void UpdateById_Test() => Execute((feedbackService, context) =>
        {
            Feedback updatedFeedback = new Feedback
            {
                Id = ValidFeedbackGuid,
                User = _feedback1.User,
                Type = _feedback2.Type,
                Message = _feedback2.Message,
                SentAt = _feedback1.SentAt
            };
            FeedbackDto updatedFeedbackDto = updatedFeedback.ToDto(_mapper);
            FeedbackDto result = feedbackService.UpdateById(_feedbackDto2, ValidFeedbackGuid);
            context.Feedbacks.Find(ValidFeedbackGuid).Should().Be(updatedFeedback);
            result.Should().Be(updatedFeedbackDto);
        });

        [Fact] public void DeleteById_Test() => Execute((feedbackService, context) =>
        {
            feedbackService.DeleteById(ValidFeedbackGuid);
            IList<Feedback> newFeedbacks = new List<Feedback>(_feedbacks);
            newFeedbacks.Remove(_feedback1);
            context.Feedbacks.Should().BeEquivalentTo(newFeedbacks);
        });
    }
}