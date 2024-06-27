using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Extensions
{
    public static class FeedbackQueryExtensions
    {
        public static IQueryable<Feedback> Filter(this IQueryable<Feedback> query, FeedbackPaginationRequest payload)
        {
            if (payload.SearchCriteria == null || !payload.SearchCriteria.Any())
                return query;
            foreach (var searchCriterion in payload.SearchCriteria)
                query = searchCriterion switch
                {
                    FeedbackSearchCriterion.ByCreateDateRange => query.Where(feedback => feedback.CreatedAt >= payload.DateRange.StartDate && feedback.CreatedAt <= payload.DateRange.EndDate),
                    FeedbackSearchCriterion.ByMessage => query.Where(feedback => feedback.Message.Contains(payload.Message)),
                    FeedbackSearchCriterion.ByUserName => query.Where(feedback => feedback.User.Name.Contains(payload.UserName)),
                    FeedbackSearchCriterion.ByType => query.Where(feedback => feedback.Type == payload.Type),
                    _ => query
                };
            return query;
        }

        public static IQueryable<Feedback> Sort(this IQueryable<Feedback> query, FeedbackPaginationRequest payload)
        {
            if (payload.SortCriteria == null || !payload.SortCriteria.Any())
                return query.OrderBy(feedback => feedback.CreatedAt);
            var firstCriterion = payload.SortCriteria.First();
            var orderedQuery = firstCriterion.Key switch
            {
                FeedbackSortCriterion.ByCreateDate => firstCriterion.Value == SortOrder.Ascending ? query.OrderBy(feedback => feedback.CreatedAt) : query.OrderByDescending(feedback => feedback.CreatedAt),
                FeedbackSortCriterion.ByMessage => firstCriterion.Value == SortOrder.Ascending ? query.OrderBy(feedback => feedback.Message) : query.OrderByDescending(feedback => feedback.Message),
                FeedbackSortCriterion.ByUserName => firstCriterion.Value == SortOrder.Ascending ? query.OrderBy(feedback => feedback.User.Name) : query.OrderByDescending(feedback => feedback.User.Name),
                _ => query.OrderBy(feedback => feedback.CreatedAt)
            };
            foreach (var sortCriterion in payload.SortCriteria.Skip(1))
                orderedQuery = sortCriterion.Key switch
                {
                    FeedbackSortCriterion.ByCreateDate => sortCriterion.Value == SortOrder.Ascending ? orderedQuery.ThenBy(feedback => feedback.CreatedAt) : orderedQuery.ThenByDescending(feedback => feedback.CreatedAt),
                    FeedbackSortCriterion.ByMessage => sortCriterion.Value == SortOrder.Ascending ? orderedQuery.ThenBy(feedback => feedback.Message) : orderedQuery.ThenByDescending(feedback => feedback.Message),
                    FeedbackSortCriterion.ByUserName => sortCriterion.Value == SortOrder.Ascending ? orderedQuery.ThenBy(feedback => feedback.User.Name) : orderedQuery.ThenByDescending(feedback => feedback.User.Name),
                    _ => orderedQuery
                };
            return orderedQuery;
        }

        public static IQueryable<Feedback> Paginate(this IQueryable<Feedback> query, FeedbackPaginationRequest payload) => query.Skip(payload.Page * payload.Size).Take(payload.Size);

        public static IQueryable<Feedback> ApplyPagination(this IQueryable<Feedback> query, FeedbackPaginationRequest? payload) => payload == null ? query.Take(10) : query.Filter(payload).Sort(payload).Paginate(payload);
    }
}