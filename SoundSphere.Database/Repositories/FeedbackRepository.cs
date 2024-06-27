using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request.Pagination;
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

        public IList<Feedback> GetAll(FeedbackPaginationRequest? payload)
        {
            IList<Feedback> feedbacks = _context.Feedbacks
                .Include(feedback => feedback.User)
                .Where(feedback => feedback.DeletedAt == null)
                .ApplyPagination(payload)
                .ToList();
            return feedbacks;
        }

        public Feedback GetById(Guid id)
        {
            Feedback? feedback = _context.Feedbacks
                .Include(feedback => feedback.User)
                .Where(feedback => feedback.DeletedAt == null)
                .FirstOrDefault(feedback => feedback.Id.Equals(id));
            if (feedback == null)
                throw new ResourceNotFoundException(string.Format(FeedbackNotFound, id));
            return feedback;
        }

        public Feedback Add(Feedback feedback)
        {
            if (feedback.Id == Guid.Empty)
                feedback.Id = Guid.NewGuid();
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

        public int CountByDateRangeAndType(DateTime? startDate, DateTime? endDate, FeedbackType? type)
        {
            int nrFeedbacks = _context.Feedbacks
                .Where(feedback => feedback.DeletedAt == null)
                .Count(feedback =>
                    (startDate == null || feedback.CreatedAt >= startDate) &&
                    (endDate == null || feedback.CreatedAt <= endDate) &&
                    (type == null || feedback.Type == type));
            return nrFeedbacks;
        }

        public void LinkFeedbackToUser(Feedback feedback)
        {
            User? existingUser = _context.Users.Find(feedback.User.Id);
            if (existingUser != null)
            {
                _context.Entry(existingUser).State = EntityState.Unchanged;
                feedback.User = existingUser;
            }
        }
    }
}