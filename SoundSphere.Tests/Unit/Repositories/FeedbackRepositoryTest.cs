using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.FeedbackMock;

namespace SoundSphere.Tests.Unit.Repositories
{
    public class FeedbackRepositoryTest
    {
        private readonly Mock<DbSet<Feedback>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IFeedbackRepository _feedbackRepository;

        private readonly Feedback _feedback1 = GetMockedFeedback1();
        private readonly Feedback _feedback2 = GetMockedFeedback2();
        private readonly IList<Feedback> _feedbacks = GetMockedFeedbacks();
        private readonly IList<Feedback> _paginatedFeedbacks = GetMockedPaginatedFeedbacks();
        private readonly FeedbackPaginationRequest _paginationRequest = GetMockedFeedbacksPaginationRequest();

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

        [Fact] public void GetAll_Test() => _feedbackRepository.GetAll(_paginationRequest).Should().BeEquivalentTo(_paginatedFeedbacks);

        [Fact] public void GetById_ValidId_Test() => _feedbackRepository.GetById(ValidFeedbackGuid).Should().Be(_feedback1);

        [Fact] public void GetById_InvalidId_Test() => _feedbackRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(FeedbackNotFound, InvalidGuid));

        [Fact] public void Add_Test()
        {
            _feedbackRepository.Add(_feedback1).Should().Be(_feedback1);
            _dbSetMock.Verify(mock => mock.Add(It.IsAny<Feedback>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Feedback updatedFeedback = new Feedback
            {
                Id = ValidFeedbackGuid,
                User = _feedback1.User,
                Type = _feedback2.Type,
                Message = _feedback2.Message,
                CreatedAt = _feedback1.CreatedAt
            };
            _feedbackRepository.UpdateById(_feedback2, ValidFeedbackGuid).Should().Be(updatedFeedback);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _feedbackRepository
            .Invoking(repository => repository.UpdateById(_feedback2, InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(FeedbackNotFound, InvalidGuid));

        [Fact] public void DeleteById_ValidId_Test()
        {
            _feedbackRepository.DeleteById(ValidFeedbackGuid);
            _dbSetMock.Verify(mock => mock.Remove(It.IsAny<Feedback>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DeleteById_InvalidId_Test() => _feedbackRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(FeedbackNotFound, InvalidGuid));
    }
}