using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;

namespace SoundSphere.Database.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly SoundSphereContext _context;

        public FeedbackRepository(SoundSphereContext context) => _context = context;

        public IList<Feedback> FindAll() => _context.Feedbacks
            .Include(feedback => feedback.User)
            .ToList();

        public Feedback FindById(Guid id) => _context.Feedbacks
            .Include(feedback => feedback.User)
            .FirstOrDefault(feedback => feedback.Id == id)
            ?? throw new ResourceNotFoundException($"Feedback with id {id} not found!");

        public Feedback Save(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();
            return feedback;
        }

        public Feedback UpdateById(Feedback feedback, Guid id)
        {
            Feedback feedbackToUpdate = FindById(id);
            DateTime SentAt = feedbackToUpdate.SentAt;
            _context.Entry(feedbackToUpdate).CurrentValues.SetValues(feedback);
            feedbackToUpdate.SentAt = SentAt;
            _context.SaveChanges();
            return feedbackToUpdate;
        }

        public void DeleteById(Guid id)
        {
            Feedback feedbackToDelete = FindById(id);
            _context.Feedbacks.Remove(feedbackToDelete);
            _context.SaveChanges();
        }

        public void LinkFeedbackToUser(Feedback feedback)
        {
            User existingUser = _context.Users.Find(feedback.User.Id);
            if (existingUser != null)
            {
                _context.Entry(existingUser).State = EntityState.Unchanged;
                feedback.User = existingUser;
            }
        }
    }
}