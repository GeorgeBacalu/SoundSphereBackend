using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;

namespace SoundSphere.Database.Dtos.Request
{
    public class NotificationPaginationRequest : PaginationRequest
    {
        public IDictionary<NotificationSortCriterion, SortOrder>? SortCriteria { get; set; }
        public IList<NotificationSearchCriterion>? SearchCriteria { get; set; }
        public string? Message { get; set; }
        public DateTimeRange? DateRange { get; set; }
        public string? UserName { get; set; }
        public bool? IsRead { get; set; }
    }

    public enum NotificationSortCriterion { BySendDate = 10, ByMessage = 20, ByUserName = 30 }

    public enum NotificationSearchCriterion { BySendDateRange = 10, ByMessage = 20, ByUserName = 30, ByIsRead = 40 }
}