﻿using FluentAssertions;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class FeedbackRepositoryIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        private readonly Feedback _feedback1 = FeedbackMock.GetMockedFeedback1();
        private readonly Feedback _feedback2 = FeedbackMock.GetMockedFeedback2();
        private readonly IList<Feedback> _feedbacks = FeedbackMock.GetMockedFeedbacks();

        public FeedbackRepositoryIntegrationTest(DbFixture fixture) => _fixture = fixture;

        private void Execute(Action<FeedbackRepository, SoundSphereContext> action)
        {
            using var context = _fixture.CreateContext();
            var feedbackRepository = new FeedbackRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Feedbacks.AddRange(_feedbacks);
            context.SaveChanges();
            action(feedbackRepository, context);
        }

        [Fact] public void FindAll_Test() => Execute((feedbackRepository, context) => feedbackRepository.FindAll().Should().BeEquivalentTo(_feedbacks));

        [Fact] public void FindById_ValidId_Test() => Execute((feedbackRepository, context) => feedbackRepository.FindById(Constants.ValidFeedbackGuid).Should().BeEquivalentTo(_feedback1));

        [Fact] public void FindById_InvalidId_Test() => Execute((feedbackRepository, context) => 
            feedbackRepository.Invoking(repository => repository.FindById(Constants.InvalidGuid))
                              .Should().Throw<ResourceNotFoundException>()
                              .WithMessage($"Feedback with id {Constants.InvalidGuid} not found!"));

        [Fact] public void Save_Test() => Execute((feedbackRepository, context) =>
        {
            Feedback newFeedback = FeedbackMock.GetMockedFeedback3();
            feedbackRepository.Save(newFeedback);
            context.Feedbacks.Find(newFeedback.Id).Should().BeEquivalentTo(newFeedback, options => options.Excluding(feedback => feedback.SentAt));
        });

        [Fact] public void UpdateById_ValidId_Test() => Execute((feedbackRepository, context) =>
        {
            Feedback updatedFeedback = new Feedback
            {
                Id = Constants.ValidFeedbackGuid,
                User = _feedback1.User,
                Type = _feedback2.Type,
                Message = _feedback2.Message,
                SentAt = _feedback1.SentAt
            };
            feedbackRepository.UpdateById(_feedback2, Constants.ValidFeedbackGuid);
            context.Feedbacks.Find(Constants.ValidFeedbackGuid).Should().BeEquivalentTo(updatedFeedback);
        });

        [Fact] public void UpdateById_InvalidId_Test() => Execute((feedbackRepository, context) =>
            feedbackRepository.Invoking(repository => repository.UpdateById(_feedback2, Constants.InvalidGuid))
                              .Should().Throw<ResourceNotFoundException>()
                              .WithMessage($"Feedback with id {Constants.InvalidGuid} not found!"));

        [Fact] public void DeleteById_ValidId_Test() => Execute((feedbackRepository, context) =>
        {
            feedbackRepository.DeleteById(Constants.ValidFeedbackGuid);
            context.Feedbacks.Should().BeEquivalentTo(new List<Feedback> { _feedback2 });
        });

        [Fact] public void DeleteById_InvalidId_Test() => Execute((feedbackRepository, context) =>
            feedbackRepository.Invoking(repository => repository.DeleteById(Constants.InvalidGuid))
                              .Should().Throw<ResourceNotFoundException>()
                              .WithMessage($"Feedback with id {Constants.InvalidGuid} not found!"));
    }
}