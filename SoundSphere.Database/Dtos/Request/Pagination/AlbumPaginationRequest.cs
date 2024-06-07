using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;

namespace SoundSphere.Database.Dtos.Request.Pagination
{
    public record AlbumPaginationRequest(
        IDictionary<AlbumSortCriterion, SortOrder>? SortCriteria,
        IList<AlbumSearchCriterion>? SearchCriteria,
        string? Title,
        DateRange? DateRange
        ) : PaginationRequest;

    public enum AlbumSortCriterion { ByTitle = 10, ByReleaseDate = 20 }

    public enum AlbumSearchCriterion { ByTitle = 10, ByReleaseDateRange = 20 }
}