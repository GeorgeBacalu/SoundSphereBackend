using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Extensions
{
    public static class FeedbackQueryExtensions
    {
        public static IQueryable<Feedback> Filter(this IQueryable<Feedback> query, FeedbackPaginationRequest payload)
        {
            if (payload.SearchCriteria == null || !payload.SearchCriteria.Any()) return query;
            return payload.SearchCriteria.Aggregate(query, (current, searchCriterion) => searchCriterion switch
            {
                FeedbackSearchCriterion.BySendDateRange => current.Where(feedback => feedback.SentAt >= payload.DateRange.StartDate && feedback.SentAt <= payload.DateRange.EndDate),
                FeedbackSearchCriterion.ByMessage => current.Where(feedback => feedback.Message.Contains(payload.Message)),
                FeedbackSearchCriterion.ByUserName => current.Where(feedback => feedback.User.Name.Contains(payload.UserName)),
                _ => current
            });
        }

        public static IQueryable<Feedback> Sort(this IQueryable<Feedback> query, FeedbackPaginationRequest payload)
        {
            if (payload.SortCriteria == null || !payload.SortCriteria.Any()) return query;
            return payload.SortCriteria.Aggregate(query, (current, sortCriterion) => sortCriterion.Key switch
            {
                FeedbackSortCriterion.BySendDate => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(feedback => feedback.SentAt) : current.OrderByDescending(feedback => feedback.SentAt),
                FeedbackSortCriterion.ByMessage => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(feedback => feedback.Message) : current.OrderByDescending(feedback => feedback.Message),
                FeedbackSortCriterion.ByUserName => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(feedback => feedback.User.Name) : current.OrderByDescending(feedback => feedback.User.Name),
                _ => current
            });
        }

        public static IQueryable<Feedback> Paginate(this IQueryable<Feedback> query, FeedbackPaginationRequest payload) => query.Skip((payload.Page - 1) * payload.Size).Take(payload.Size);
    }
}