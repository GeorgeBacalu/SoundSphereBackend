using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;

namespace SoundSphere.Database.Dtos.Request.Pagination
{
    public record PlaylistPaginationRequest(
        IDictionary<PlaylistSortCriterion, SortOrder>? SortCriteria,
        IList<PlaylistSearchCriterion>? SearchCriteria,
        DateTimeRange? DateRange,
        string? Title,
        string? UserName,
        string? SongTitle) : PaginationRequest;

    public enum PlaylistSortCriterion { InvalidSortCriterion, ByCreateDate = 10, ByTitle = 20 }

    public enum PlaylistSearchCriterion { InvalidSearchCriterion, ByUserName = 10, ByTitle = 20, ByCreateDateRange = 30, BySongTitle = 40 }
}