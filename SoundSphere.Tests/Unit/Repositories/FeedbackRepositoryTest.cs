using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Repositories
{
    public class FeedbackRepositoryTest
    {
        private readonly Mock<DbSet<Feedback>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IFeedbackRepository _feedbackRepository;

        private readonly Feedback _feedback1 = FeedbackMock.GetMockedFeedback1();
        private readonly Feedback _feedback2 = FeedbackMock.GetMockedFeedback2();
        private readonly IList<Feedback> _feedbacks = FeedbackMock.GetMockedFeedbacks();

        public FeedbackRepositoryTest()
        {
            IQueryable<Feedback> queryableFeedbacks = _feedbacks.AsQueryable();
            _dbSetMock.As<IQueryable<Feedback>>().Setup(mock => mock.Provider).Returns(queryableFeedbacks.Provider);
            _dbSetMock.As<IQueryable<Feedback>>().Setup(mock => mock.Expression).Returns(queryableFeedbacks.Expression);
            _dbSetMock.As<IQueryable<Feedback>>().Setup(mock => mock.ElementType).Returns(queryableFeedbacks.ElementType);
            _dbSetMock.As<IQueryable<Feedback>>().Setup(mock => mock.GetEnumerator()).Returns(queryableFeedbacks.GetEnumerator());
            _dbContextMock.Setup(mock => mock.Feedbacks).Returns(_dbSetMock.Object);
            _feedbackRepository = new FeedbackRepository(_dbContextMock.Object);
        }

        [Fact] public void FindAll_Test() => _feedbackRepository.FindAll().Should().BeEquivalentTo(_feedbacks);

        [Fact] public void FindById_ValidId_Test() => _feedbackRepository.FindById(Constants.ValidFeedbackGuid).Should().Be(_feedback1);

        [Fact] public void FindById_InvalidId_Test() => _feedbackRepository
            .Invoking(repository => repository.FindById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.FeedbackNotFound, Constants.InvalidGuid));

        [Fact] public void Save_Test()
        {
            _feedbackRepository.Save(_feedback1).Should().Be(_feedback1);
            _dbSetMock.Verify(mock => mock.Add(It.IsAny<Feedback>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Feedback updatedFeedback = new Feedback
            {
                Id = Constants.ValidFeedbackGuid,
                User = _feedback1.User,
                Type = _feedback2.Type,
                Message = _feedback2.Message,
                SentAt = _feedback1.SentAt
            };
            _feedbackRepository.UpdateById(_feedback2, Constants.ValidFeedbackGuid).Should().Be(updatedFeedback);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _feedbackRepository
            .Invoking(repository => repository.UpdateById(_feedback2, Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.FeedbackNotFound, Constants.InvalidGuid));

        [Fact] public void DeleteById_ValidId_Test()
        {
            _feedbackRepository.DeleteById(Constants.ValidFeedbackGuid);
            _dbSetMock.Verify(mock => mock.Remove(It.IsAny<Feedback>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DeleteById_InvalidId_Test() => _feedbackRepository
            .Invoking(repository => repository.DeleteById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.FeedbackNotFound, Constants.InvalidGuid));
    }
}