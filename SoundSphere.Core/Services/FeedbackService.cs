using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public FeedbackService(IFeedbackRepository feedbackRepository, IUserRepository userRepository, IMapper mapper) => (_feedbackRepository, _userRepository, _mapper) = (feedbackRepository, userRepository, mapper);

        public IList<FeedbackDto> FindAll() => _feedbackRepository.FindAll().ToDtos(_mapper);

        public IList<FeedbackDto> FindAllPagination(FeedbackPaginationRequest payload) => _feedbackRepository.FindAllPagination(payload).ToDtos(_mapper);

        public FeedbackDto FindById(Guid id) => _feedbackRepository.FindById(id).ToDto(_mapper);

        public FeedbackDto Save(FeedbackDto feedbackDto)
        {
            Feedback feedback = feedbackDto.ToEntity(_userRepository, _mapper);
            if (feedback.Id == Guid.Empty) feedback.Id = Guid.NewGuid();
            feedback.SentAt = DateTime.Now;
            _feedbackRepository.LinkFeedbackToUser(feedback);
            return _feedbackRepository.Save(feedback).ToDto(_mapper);
        }

        public FeedbackDto UpdateById(FeedbackDto feedbackDto, Guid id) => _feedbackRepository.UpdateById(feedbackDto.ToEntity(_userRepository, _mapper), id).ToDto(_mapper);

        public void DeleteById(Guid id) => _feedbackRepository.DeleteById(id);
    }
}