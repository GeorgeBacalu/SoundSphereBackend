using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos.Request.Pagination
{
    public record NotificationPaginationRequest(
        IDictionary<NotificationSortCriterion, SortOrder>? SortCriteria,
        IList<NotificationSearchCriterion>? SearchCriteria,
        DateTimeRange? DateRange,
        string? Message,
        string? UserName,
        bool? IsRead,
        NotificationType? Type) : PaginationRequest;

    public enum NotificationSortCriterion { InvalidSortCriterion, ByCreateDate = 10, ByMessage = 20, ByUserName = 30 }

    public enum NotificationSearchCriterion { InvalidSearchCriterion, ByCreateDateRange = 10, ByMessage = 20, ByUserName = 30, ByIsRead = 40, ByType = 50 }
}