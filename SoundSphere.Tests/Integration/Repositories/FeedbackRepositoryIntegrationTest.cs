using FluentAssertions;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.FeedbackMock;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class FeedbackRepositoryIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        private readonly Feedback _feedback1 = GetMockedFeedback1();
        private readonly Feedback _feedback2 = GetMockedFeedback2();
        private readonly IList<Feedback> _feedbacks = GetMockedFeedbacks();
        private readonly IList<Feedback> _paginatedFeedbacks = GetMockedPaginatedFeedbacks();
        private readonly FeedbackPaginationRequest _paginationRequest = GetMockedFeedbacksPaginationRequest();

        public FeedbackRepositoryIntegrationTest(DbFixture fixture) => _fixture = fixture;

        private void Execute(Action<FeedbackRepository, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var feedbackRepository = new FeedbackRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Feedbacks.AddRange(_feedbacks);
            context.SaveChanges();
            action(feedbackRepository, context);
            transaction.Rollback();
        }

        [Fact] public void GetAll_Test() => Execute((feedbackRepository, context) => feedbackRepository.GetAll().Should().BeEquivalentTo(_feedbacks));

        [Fact] public void GetAllPagination_Test() => Execute((feedbackRepository, context) => feedbackRepository.GetAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedFeedbacks));
        
        [Fact] public void GetById_ValidId_Test() => Execute((feedbackRepository, context) => feedbackRepository.GetById(ValidFeedbackGuid).Should().Be(_feedback1));

        [Fact] public void GetById_InvalidId_Test() => Execute((feedbackRepository, context) => feedbackRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(FeedbackNotFound, InvalidGuid)));

        [Fact] public void Add_Test() => Execute((feedbackRepository, context) =>
        {
            Feedback newFeedback = GetMockedFeedback37();
            feedbackRepository.Add(newFeedback);
            context.Feedbacks.Find(newFeedback.Id).Should().BeEquivalentTo(newFeedback, options => options.Excluding(feedback => feedback.SentAt));
        });

        [Fact] public void UpdateById_ValidId_Test() => Execute((feedbackRepository, context) =>
        {
            Feedback updatedFeedback = new Feedback
            {
                Id = ValidFeedbackGuid,
                User = _feedback1.User,
                Type = _feedback2.Type,
                Message = _feedback2.Message,
                SentAt = _feedback1.SentAt
            };
            feedbackRepository.UpdateById(_feedback2, ValidFeedbackGuid);
            context.Feedbacks.Find(ValidFeedbackGuid).Should().Be(updatedFeedback);
        });

        [Fact] public void UpdateById_InvalidId_Test() => Execute((feedbackRepository, context) => feedbackRepository
            .Invoking(repository => repository.UpdateById(_feedback2, InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(FeedbackNotFound, InvalidGuid)));

        [Fact] public void DeleteById_ValidId_Test() => Execute((feedbackRepository, context) =>
        {
            feedbackRepository.DeleteById(ValidFeedbackGuid);
            IList<Feedback> newFeedbacks = new List<Feedback>(_feedbacks);
            newFeedbacks.Remove(_feedback1);
            context.Feedbacks.Should().BeEquivalentTo(newFeedbacks);
        });

        [Fact] public void DeleteById_InvalidId_Test() => Execute((feedbackRepository, context) => feedbackRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(FeedbackNotFound, InvalidGuid)));
    }
}