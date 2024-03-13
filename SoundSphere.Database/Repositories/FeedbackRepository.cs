using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly SoundSphereContext _context;

        public FeedbackRepository(SoundSphereContext context) => _context = context;

        public IList<Feedback> FindAll() => _context.Feedbacks.ToList();

        public Feedback FindById(Guid id) => _context.Feedbacks.Find(id) ?? throw new Exception($"Feedback with id {id} not found!");

        public Feedback Save(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();
            return feedback;
        }

        public Feedback UpdateById(Feedback feedback, Guid id)
        {
            Feedback feedbackToUpdate = FindById(id);
            _context.Entry(feedbackToUpdate).CurrentValues.SetValues(feedback);
            _context.SaveChanges();
            return feedbackToUpdate;
        }

        public void DeleteById(Guid id)
        {
            Feedback feedbackToDelete = FindById(id);
            _context.Feedbacks.Remove(feedbackToDelete);
            _context.SaveChanges();
        }
    }
}