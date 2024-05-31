using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;

namespace SoundSphere.Database.Dtos.Request
{
    public record PlaylistPaginationRequest(
        IDictionary<PlaylistSortCriterion, SortOrder>? SortCriteria,
        IList<PlaylistSearchCriterion>? SearchCriteria,
        string? Title,
        DateTimeRange? DateRange,
        string? UserName,
        Guid SongId
        ) : PaginationRequest;

    public enum PlaylistSortCriterion { ByCreatedDate = 10, ByTitle = 20 }

    public enum PlaylistSearchCriterion { ByUserName = 10, ByTitle = 20, ByCreatedDateRange = 30, BySongTitle = 40 }
}