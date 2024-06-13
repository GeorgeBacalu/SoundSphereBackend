using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Request.Pagination
{
    public record ArtistPaginationRequest(
        IDictionary<ArtistSortCriterion, SortOrder>? SortCriteria,
        IList<ArtistSearchCriterion>? SearchCriteria,

        [StringLength(75, ErrorMessage = "Name can't be longer than 75 characters")]
        string? Name) : PaginationRequest;

    public enum ArtistSortCriterion { InvalidSortCriterion, ByName = 10 }

    public enum ArtistSearchCriterion { InvalidSearchCriterion, ByName = 10 }
}