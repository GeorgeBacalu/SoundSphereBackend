using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos.Request.Pagination
{
    public record FeedbackPaginationRequest(
        IDictionary<FeedbackSortCriterion, SortOrder>? SortCriteria,
        IList<FeedbackSearchCriterion>? SearchCriteria,
        DateTimeRange? DateRange,
        string? Message,
        string? UserName,
        FeedbackType? Type) : PaginationRequest;

    public enum FeedbackSortCriterion { InvalidSortCriterion, ByCreateDate = 10, ByMessage = 20, ByUserName = 30 }

    public enum FeedbackSearchCriterion { InvalidSearchCriterion, ByCreateDateRange = 10, ByMessage = 20, ByUserName = 30, ByType = 40 }
}