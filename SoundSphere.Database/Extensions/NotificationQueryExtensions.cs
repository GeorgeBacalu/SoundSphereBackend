using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Extensions
{
    public static class NotificationQueryExtensions
    {
        public static IQueryable<Notification> Filter(this IQueryable<Notification> query, NotificationPaginationRequest payload) =>
            payload.SearchCriteria == null || !payload.SearchCriteria.Any() ? query :
            payload.SearchCriteria.Aggregate(query, (current, searchCriterion) => searchCriterion switch
            {
                NotificationSearchCriterion.ByCreateDateRange => current.Where(notification => notification.CreatedAt >= payload.DateRange.StartDate && notification.CreatedAt <= payload.DateRange.EndDate),
                NotificationSearchCriterion.ByMessage => current.Where(notification => notification.Message.Contains(payload.Message)),
                NotificationSearchCriterion.ByUserName => current.Where(notification => notification.User.Name.Contains(payload.UserName)),
                NotificationSearchCriterion.ByIsRead => current.Where(notification => notification.IsRead == payload.IsRead),
                _ => current
            });

        public static IQueryable<Notification> Sort(this IQueryable<Notification> query, NotificationPaginationRequest payload) =>
            payload.SortCriteria == null || !payload.SortCriteria.Any() ? query.OrderBy(notification => notification.CreatedAt) :
            payload.SortCriteria.Aggregate(query, (current, sortCriterion) => sortCriterion.Key switch
            {
                NotificationSortCriterion.ByCreateDate => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(notification => notification.CreatedAt) : current.OrderByDescending(notification => notification.CreatedAt),
                NotificationSortCriterion.ByMessage => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(notification => notification.Message) : current.OrderByDescending(notification => notification.Message),
                NotificationSortCriterion.ByUserName => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(notification => notification.User.Name) : current.OrderByDescending(notification => notification.User.Name),
                _ => current.OrderBy(notification => notification.CreatedAt)
            });

        public static IQueryable<Notification> Paginate(this IQueryable<Notification> query, NotificationPaginationRequest payload) => query.Skip(payload.Page * payload.Size).Take(payload.Size);
    }
}