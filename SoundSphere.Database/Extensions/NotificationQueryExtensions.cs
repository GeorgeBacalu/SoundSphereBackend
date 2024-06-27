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
                    NotificationSearchCriterion.BySenderName => query.Where(notification => notification.Sender.Name.Contains(payload.SenderName)),
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
                NotificationSortCriterion.BySenderName => firstCriterion.Value == SortOrder.Ascending ? query.OrderBy(notification => notification.Sender.Name) : query.OrderByDescending(notification => notification.Sender.Name),
                _ => query.OrderBy(notification => notification.CreatedAt)
            };
            foreach (var sortCriterion in payload.SortCriteria.Skip(1))
                orderedQuery = sortCriterion.Key switch
                {
                    NotificationSortCriterion.ByCreateDate => sortCriterion.Value == SortOrder.Ascending ? orderedQuery.ThenBy(notification => notification.CreatedAt) : orderedQuery.ThenByDescending(notification => notification.CreatedAt),
                    NotificationSortCriterion.ByMessage => sortCriterion.Value == SortOrder.Ascending ? orderedQuery.ThenBy(notification => notification.Message) : orderedQuery.ThenByDescending(notification => notification.Message),
                    NotificationSortCriterion.BySenderName => sortCriterion.Value == SortOrder.Ascending ? orderedQuery.ThenBy(notification => notification.Sender.Name) : orderedQuery.ThenByDescending(notification => notification.Sender.Name),
                    _ => orderedQuery
                };
            return orderedQuery;
        }

        public static IQueryable<Notification> Paginate(this IQueryable<Notification> query, NotificationPaginationRequest payload) => query.Skip(payload.Page * payload.Size).Take(payload.Size);

        public static IQueryable<Notification> ApplyPagination(this IQueryable<Notification> query, NotificationPaginationRequest? payload) => payload == null ? query.Take(10) : query.Filter(payload).Sort(payload).Paginate(payload);
    }
}