﻿using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Extensions;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;

namespace SoundSphere.Database.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly SoundSphereDbContext _context;

        public FeedbackRepository(SoundSphereDbContext context) => _context = context;

        public IList<Feedback> FindAll() => _context.Feedbacks
            .Include(feedback => feedback.User)
            .ToList();

        public IList<Feedback> FindAllPagination(FeedbackPaginationRequest payload) => _context.Feedbacks
            .Include(feedback => feedback.User)
            .Filter(payload)
            .Sort(payload)
            .Paginate(payload)
            .ToList();

        public Feedback FindById(Guid id) => _context.Feedbacks
            .Include(feedback => feedback.User)
            .FirstOrDefault(feedback => feedback.Id == id)
            ?? throw new ResourceNotFoundException(string.Format(Constants.FeedbackNotFound, id));

        public Feedback Save(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();
            return feedback;
        }

        public Feedback UpdateById(Feedback feedback, Guid id)
        {
            Feedback feedbackToUpdate = FindById(id);
            feedbackToUpdate.Type = feedback.Type;
            feedbackToUpdate.Message = feedback.Message;
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