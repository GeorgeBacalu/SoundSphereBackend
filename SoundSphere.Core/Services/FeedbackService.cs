using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserRepository _userRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository, IUserRepository userRepository)
        {
            _feedbackRepository = feedbackRepository;
            _userRepository = userRepository;
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

        public FeedbackDto ConvertToDto(Feedback feedback) => new FeedbackDto
        {
            Id = feedback.Id,
            UserId = feedback.User.Id,
            Type = feedback.Type,
            Message = feedback.Message,
            SentAt = feedback.SentAt
        };

        public Feedback ConvertToEntity(FeedbackDto feedbackDto) => new Feedback
        {
            Id = feedbackDto.Id,
            User = _userRepository.FindById(feedbackDto.UserId),
            Type = feedbackDto.Type,
            Message = feedbackDto.Message,
            SentAt = feedbackDto.SentAt
        };
    }
}