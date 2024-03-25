using AutoMapper;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public FeedbackService(IFeedbackRepository feedbackRepository, IUserRepository userRepository, IMapper mapper)
        {
            _feedbackRepository = feedbackRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public IList<FeedbackDto> FindAll() => ConvertToDtos(_feedbackRepository.FindAll());

        public FeedbackDto FindById(Guid id) => ConvertToDto(_feedbackRepository.FindById(id));

        public FeedbackDto Save(FeedbackDto feedbackDto)
        {
            Feedback feedback = ConvertToEntity(feedbackDto);
            if (feedback.Id == Guid.Empty) feedback.Id = Guid.NewGuid();
            feedback.SentAt = DateTime.Now;
            _feedbackRepository.LinkFeedbackToUser(feedback);
            return ConvertToDto(_feedbackRepository.Save(feedback));
        }

        public FeedbackDto UpdateById(FeedbackDto feedbackDto, Guid id) => ConvertToDto(_feedbackRepository.UpdateById(ConvertToEntity(feedbackDto), id));

        public void DeleteById(Guid id) => _feedbackRepository.DeleteById(id);

        public IList<FeedbackDto> ConvertToDtos(IList<Feedback> feedbacks) => feedbacks.Select(ConvertToDto).ToList();

        public IList<Feedback> ConvertToEntities(IList<FeedbackDto> feedbackDtos) => feedbackDtos.Select(ConvertToEntity).ToList();

        public FeedbackDto ConvertToDto(Feedback feedback)
        {
            FeedbackDto feedbackDto = _mapper.Map<FeedbackDto>(feedback);
            feedbackDto.UserId = feedback.User.Id;
            return feedbackDto;
        }

        public Feedback ConvertToEntity(FeedbackDto feedbackDto)
        {
            Feedback feedback = _mapper.Map<Feedback>(feedbackDto);
            feedback.User = _userRepository.FindById(feedbackDto.UserId);
            return feedback;
        }
    }
}