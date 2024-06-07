using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;

namespace SoundSphere.Database.Dtos.Request.Pagination
{
    public record FeedbackPaginationRequest(
        IDictionary<FeedbackSortCriterion, SortOrder>? SortCriteria,
        IList<FeedbackSearchCriterion>? SearchCriteria,
        DateTimeRange? DateRange,
        string? Message,
        string? UserName
        ) : PaginationRequest;

    public enum FeedbackSortCriterion { ByCreateDate = 10, ByMessage = 20, ByUserName = 30 }

    public enum FeedbackSearchCriterion { ByCreateDateRange = 10, ByMessage = 20, ByUserName = 30 }
}