using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;

namespace SoundSphere.Database.Dtos.Request
{
    public record NotificationPaginationRequest(
        IDictionary<NotificationSortCriterion, SortOrder>? SortCriteria,
        IList<NotificationSearchCriterion>? SearchCriteria,
        string? Message,
        DateTimeRange? DateRange,
        string? UserName,
        bool? IsRead
        ) : PaginationRequest;

    public enum NotificationSortCriterion { BySendDate = 10, ByMessage = 20, ByUserName = 30 }

    public enum NotificationSearchCriterion { BySendDateRange = 10, ByMessage = 20, ByUserName = 30, ByIsRead = 40 }
}