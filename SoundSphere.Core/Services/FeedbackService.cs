using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Dtos.Response;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;

namespace SoundSphere.Core.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public FeedbackService(IFeedbackRepository feedbackRepository, IUserRepository userRepository, IMapper mapper) => (_feedbackRepository, _userRepository, _mapper) = (feedbackRepository, userRepository, mapper);

        public IList<FeedbackDto> GetAll(FeedbackPaginationRequest? payload)
        {
            IList<FeedbackDto> feedbackDtos = _feedbackRepository.GetAll(payload).ToDtos(_mapper);
            return feedbackDtos;
        }

        public FeedbackDto GetById(Guid id)
        {
            FeedbackDto feedbackDto = _feedbackRepository.GetById(id).ToDto(_mapper);
            return feedbackDto;
        }

        public FeedbackDto Add(FeedbackDto feedbackDto, Guid userId)
        {
            feedbackDto.UserId = userId;
            Feedback feedbackToCreate = feedbackDto.ToEntity(_userRepository, _mapper);
            _feedbackRepository.LinkFeedbackToUser(feedbackToCreate);
            FeedbackDto createdFeedbackDto = _feedbackRepository.Add(feedbackToCreate).ToDto(_mapper);
            return createdFeedbackDto;
        }

        public FeedbackDto UpdateById(FeedbackDto feedbackDto, Guid feedbackId, Guid userId)
        {
            Feedback feedback = _feedbackRepository.GetById(feedbackId);
            if (!feedback.User.Id.Equals(userId))
                throw new ForbiddenAccessException(UpdateFeedbackDenied);
            feedbackDto.UserId = userId;
            Feedback feedbackToUpdate = feedbackDto.ToEntity(_userRepository, _mapper);
            FeedbackDto updatedFeedbackDto = _feedbackRepository.UpdateById(feedbackToUpdate, feedbackId).ToDto(_mapper);
            return updatedFeedbackDto;
        }

        public FeedbackDto DeleteById(Guid feedbackId, Guid userId)
        {
            Feedback feedback = _feedbackRepository.GetById(feedbackId);
            if (!feedback.User.Id.Equals(userId))
                throw new ForbiddenAccessException(DeleteFeedbackDenied);
            FeedbackDto deletedFeedbackDto = _feedbackRepository.DeleteById(feedbackId).ToDto(_mapper);
            return deletedFeedbackDto;
        }

        public FeedbackStatisticsDto GetStatistics(DateTime? startDate, DateTime? endDate)
        {
            FeedbackStatisticsDto statistics = new FeedbackStatisticsDto
            (
                TotalFeedbacks: _feedbackRepository.CountByDateRangeAndType(startDate, endDate, null),
                NrIssues: _feedbackRepository.CountByDateRangeAndType(startDate, endDate, FeedbackType.Issue),
                NrImprovements: _feedbackRepository.CountByDateRangeAndType(startDate, endDate, FeedbackType.Improvement),
                NrOptimizations: _feedbackRepository.CountByDateRangeAndType(startDate, endDate, FeedbackType.Optimization)
            );
            return statistics;
        }
    }
}