using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;

namespace SoundSphere.Database.Dtos.Request
{
    public record FeedbackPaginationRequest(
        IDictionary<FeedbackSortCriterion, SortOrder>? SortCriteria,
        IList<FeedbackSearchCriterion>? SearchCriteria,
        string? Message,
        DateTimeRange? DateRange,
        string? UserName
        ) : PaginationRequest;

    public enum FeedbackSortCriterion { BySendDate = 10, ByMessage = 20, ByUserName = 30 }

    public enum FeedbackSearchCriterion { BySendDateRange = 10, ByMessage = 20, ByUserName = 30 }
}