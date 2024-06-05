using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Extensions
{
    public static class FeedbackQueryExtensions
    {
        public static IQueryable<Feedback> Filter(this IQueryable<Feedback> query, FeedbackPaginationRequest payload) =>
            payload.SearchCriteria == null || !payload.SearchCriteria.Any() ? query :
            payload.SearchCriteria.Aggregate(query, (current, searchCriterion) => searchCriterion switch
            {
                FeedbackSearchCriterion.ByCreateDateRange => current.Where(feedback => feedback.CreatedAt >= payload.DateRange.StartDate && feedback.CreatedAt <= payload.DateRange.EndDate),
                FeedbackSearchCriterion.ByMessage => current.Where(feedback => feedback.Message.Contains(payload.Message)),
                FeedbackSearchCriterion.ByUserName => current.Where(feedback => feedback.User.Name.Contains(payload.UserName)),
                _ => current
            });

        public static IQueryable<Feedback> Sort(this IQueryable<Feedback> query, FeedbackPaginationRequest payload) =>
            payload.SortCriteria == null || !payload.SortCriteria.Any() ? query.OrderBy(feedback => feedback.CreatedAt) :
            payload.SortCriteria.Aggregate(query, (current, sortCriterion) => sortCriterion.Key switch
            {
                FeedbackSortCriterion.ByCreateDate => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(feedback => feedback.CreatedAt) : current.OrderByDescending(feedback => feedback.CreatedAt),
                FeedbackSortCriterion.ByMessage => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(feedback => feedback.Message) : current.OrderByDescending(feedback => feedback.Message),
                FeedbackSortCriterion.ByUserName => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(feedback => feedback.User.Name) : current.OrderByDescending(feedback => feedback.User.Name),
                _ => current.OrderBy(feedback => feedback.CreatedAt)
            });

        public static IQueryable<Feedback> Paginate(this IQueryable<Feedback> query, FeedbackPaginationRequest payload) => query.Skip(payload.Page * payload.Size).Take(payload.Size);
    }
}