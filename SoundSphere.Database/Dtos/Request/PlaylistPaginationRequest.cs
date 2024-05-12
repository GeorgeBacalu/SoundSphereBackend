using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;

namespace SoundSphere.Database.Dtos.Request
{
    public class PlaylistPaginationRequest : PaginationRequest
    {
        public IDictionary<PlaylistSortCriterion, SortOrder>? SortCriteria { get; set; }
        public IList<PlaylistSearchCriterion>? SearchCriteria { get; set; }
        public string? Title { get; set; }
        public DateTimeRange? DateRange { get; set; }
        public string? UserName { get; set; }
        public Guid SongId { get; set; }
    }

    public enum PlaylistSortCriterion { ByCreatedDate = 10, ByTitle = 20 }

    public enum PlaylistSearchCriterion { ByUserName = 10, ByTitle = 20, ByCreatedDateRange = 30, BySongTitle = 40 }
}