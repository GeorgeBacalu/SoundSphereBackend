using Microsoft.Data.SqlClient;

namespace SoundSphere.Database.Dtos.Request.Pagination
{
    public record ArtistPaginationRequest(
        IDictionary<ArtistSortCriterion, SortOrder>? SortCriteria,
        IList<ArtistSearchCriterion>? SearchCriteria,
        string? Name
        ) : PaginationRequest;

    public enum ArtistSortCriterion { ByName = 10 }

    public enum ArtistSearchCriterion { ByName = 10 }
}