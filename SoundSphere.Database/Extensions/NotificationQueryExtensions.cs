using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Extensions
{
    public static class NotificationQueryExtensions
    {
        public static IQueryable<Notification> Filter(this IQueryable<Notification> query, NotificationPaginationRequest payload)
        {
            if (payload.SearchCriteria == null || !payload.SearchCriteria.Any()) return query;
            return payload.SearchCriteria.Aggregate(query, (current, searchCriterion) => searchCriterion switch
            {
                NotificationSearchCriterion.BySendDateRange => current.Where(notification => notification.SentAt >= payload.DateRange.StartDate && notification.SentAt <= payload.DateRange.EndDate),
                NotificationSearchCriterion.ByMessage => current.Where(notification => notification.Message.Contains(payload.Message)),
                NotificationSearchCriterion.ByUserName => current.Where(notification => notification.User.Name.Contains(payload.UserName)),
                NotificationSearchCriterion.ByIsRead => current.Where(notification => notification.IsRead == payload.IsRead),
                _ => current
            });
        }

        public static IQueryable<Notification> Sort(this IQueryable<Notification> query, NotificationPaginationRequest payload)
        {
            if (payload.SortCriteria == null || !payload.SortCriteria.Any()) return query;
            return payload.SortCriteria.Aggregate(query, (current, sortCriterion) => sortCriterion.Key switch
            {
                NotificationSortCriterion.BySendDate => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(notification => notification.SentAt) : current.OrderByDescending(notification => notification.SentAt),
                NotificationSortCriterion.ByMessage => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(notification => notification.Message) : current.OrderByDescending(notification => notification.Message),
                NotificationSortCriterion.ByUserName => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(notification => notification.User.Name) : current.OrderByDescending(notification => notification.User.Name),
                _ => current
            });
        }

        public static IQueryable<Notification> Paginate(this IQueryable<Notification> query, NotificationPaginationRequest payload) => query.Skip((payload.Page - 1) * payload.Size).Take(payload.Size);
    }
}