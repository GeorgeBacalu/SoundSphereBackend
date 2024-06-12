using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Extensions
{
    public static class NotificationQueryExtensions
    {
        public static IQueryable<Notification> Filter(this IQueryable<Notification> query, NotificationPaginationRequest payload)
        {
            if (payload.SearchCriteria == null || !payload.SearchCriteria.Any())
                return query;
            foreach (var searchCrtierion in payload.SearchCriteria)
                query = searchCrtierion switch
                {
                    NotificationSearchCriterion.ByCreateDateRange => query.Where(notification => notification.CreatedAt >= payload.DateRange.StartDate && notification.CreatedAt <= payload.DateRange.EndDate),
                    NotificationSearchCriterion.ByMessage => query.Where(notification => notification.Message.Contains(payload.Message)),
                    NotificationSearchCriterion.ByUserName => query.Where(notification => notification.User.Name.Contains(payload.UserName)),
                    NotificationSearchCriterion.ByIsRead => query.Where(notification => notification.IsRead == payload.IsRead),
                    NotificationSearchCriterion.ByType => query.Where(notification => notification.Type == payload.Type),
                    _ => query
                };
            return query;
        }

        public static IQueryable<Notification> Sort(this IQueryable<Notification> query, NotificationPaginationRequest payload)
        {
            if (payload.SortCriteria == null || !payload.SortCriteria.Any())
                return query.OrderBy(notification => notification.CreatedAt);
            var firstCriterion = payload.SortCriteria.First();
            var orderedQuery = firstCriterion.Key switch
            {
                NotificationSortCriterion.ByCreateDate => firstCriterion.Value == SortOrder.Ascending ? query.OrderBy(notification => notification.CreatedAt) : query.OrderByDescending(notification => notification.CreatedAt),
                NotificationSortCriterion.ByMessage => firstCriterion.Value == SortOrder.Ascending ? query.OrderBy(notification => notification.Message) : query.OrderByDescending(notification => notification.Message),
                NotificationSortCriterion.ByUserName => firstCriterion.Value == SortOrder.Ascending ? query.OrderBy(notification => notification.User.Name) : query.OrderByDescending(notification => notification.User.Name),
                _ => query.OrderBy(notification => notification.CreatedAt)
            };
            foreach (var sortCriterion in payload.SortCriteria.Skip(1))
                orderedQuery = sortCriterion.Key switch
                {
                    NotificationSortCriterion.ByCreateDate => sortCriterion.Value == SortOrder.Ascending ? orderedQuery.ThenBy(notification => notification.CreatedAt) : orderedQuery.ThenByDescending(notification => notification.CreatedAt),
                    NotificationSortCriterion.ByMessage => sortCriterion.Value == SortOrder.Ascending ? orderedQuery.ThenBy(notification => notification.Message) : orderedQuery.ThenByDescending(notification => notification.Message),
                    NotificationSortCriterion.ByUserName => sortCriterion.Value == SortOrder.Ascending ? orderedQuery.ThenBy(notification => notification.User.Name) : orderedQuery.ThenByDescending(notification => notification.User.Name),
                    _ => orderedQuery
                };
            return orderedQuery;
        }

        public static IQueryable<Notification> Paginate(this IQueryable<Notification> query, NotificationPaginationRequest payload) => query.Skip(payload.Page * payload.Size).Take(payload.Size);
    }
}