using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository) => _feedbackRepository = feedbackRepository;

        public IList<Feedback> FindAll() => _feedbackRepository.FindAll();

        public Feedback FindById(Guid id) => _feedbackRepository.FindById(id);

        public Feedback Save(Feedback feedback)
        {
            if (feedback == null) throw new Exception("Can't persist null feedback to DB!");
            if (feedback.Id == Guid.Empty) feedback.Id = Guid.NewGuid();
            feedback.SentAt = DateTime.Now;
            return _feedbackRepository.Save(feedback);
        }

        public Feedback UpdateById(Feedback feedback, Guid id)
        {
            if (feedback == null) throw new Exception("Can't persist null feedback to DB!");
            return _feedbackRepository.UpdateById(feedback, id);
        }

        public void DeleteById(Guid id) => _feedbackRepository.DeleteById(id);
    }
}