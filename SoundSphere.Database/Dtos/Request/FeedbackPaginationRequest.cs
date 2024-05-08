using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;

namespace SoundSphere.Database.Dtos.Request
{
    public class FeedbackPaginationRequest : PaginationRequest
    {
        public IDictionary<FeedbackSortCriterion, SortOrder>? SortCriteria { get; set; }
        public IList<FeedbackSearchCriterion>? SearchCriteria { get; set; }
        public string? Message { get; set; }
        public DateTimeRange? DateRange { get; set; }
        public string? UserName { get; set; }
    }

    public enum FeedbackSortCriterion { BySendDate = 10, ByMessage = 20, ByUserName = 30 }

    public enum FeedbackSearchCriterion { BySendDateRange = 10, ByMessage = 20, ByUserName = 30 }
}