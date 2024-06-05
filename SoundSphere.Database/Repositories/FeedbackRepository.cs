using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Extensions;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;

namespace SoundSphere.Database.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly SoundSphereDbContext _context;

        public FeedbackRepository(SoundSphereDbContext context) => _context = context;

        public IList<Feedback> GetAll(FeedbackPaginationRequest payload) => _context.Feedbacks
            .Include(feedback => feedback.User)
            .Where(feedback => feedback.DeletedAt == null)
            .Filter(payload)
            .Sort(payload)
            .Paginate(payload)
            .ToList();

        public Feedback GetById(Guid id) => _context.Feedbacks
            .Include(feedback => feedback.User)
            .Where(feedback => feedback.DeletedAt == null)
            .FirstOrDefault(feedback => feedback.Id == id)
            ?? throw new ResourceNotFoundException(string.Format(FeedbackNotFound, id));

        public Feedback Add(Feedback feedback)
        {
            if (feedback.Id == Guid.Empty) feedback.Id = Guid.NewGuid();
            feedback.CreatedAt = DateTime.Now;
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();
            return feedback;
        }

        public Feedback UpdateById(Feedback feedback, Guid id)
        {
            Feedback feedbackToUpdate = GetById(id);
            feedbackToUpdate.Type = feedback.Type;
            feedbackToUpdate.Message = feedback.Message;
            if (_context.Entry(feedbackToUpdate).State == EntityState.Modified)
                feedbackToUpdate.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return feedbackToUpdate;
        }

        public Feedback DeleteById(Guid id)
        {
            Feedback feedbackToDelete = GetById(id);
            feedbackToDelete.DeletedAt = DateTime.Now;
            _context.SaveChanges();
            return feedbackToDelete;
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