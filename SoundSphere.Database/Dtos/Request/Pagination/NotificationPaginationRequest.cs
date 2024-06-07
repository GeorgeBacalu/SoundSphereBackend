using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;

namespace SoundSphere.Database.Dtos.Request.Pagination
{
    public record NotificationPaginationRequest(
        IDictionary<NotificationSortCriterion, SortOrder>? SortCriteria,
        IList<NotificationSearchCriterion>? SearchCriteria,
        DateTimeRange? DateRange,
        string? Message,
        string? UserName,
        bool? IsRead
        ) : PaginationRequest;

    public enum NotificationSortCriterion { ByCreateDate = 10, ByMessage = 20, ByUserName = 30 }

    public enum NotificationSearchCriterion { ByCreateDateRange = 10, ByMessage = 20, ByUserName = 30, ByIsRead = 40 }
}