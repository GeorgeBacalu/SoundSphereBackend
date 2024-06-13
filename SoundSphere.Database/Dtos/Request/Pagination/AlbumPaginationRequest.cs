using Microsoft.Data.SqlClient;
using SoundSphere.Database.Attributes;
using SoundSphere.Database.Dtos.Request.Models;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Request.Pagination
{
    public record AlbumPaginationRequest(
        IDictionary<AlbumSortCriterion, SortOrder>? SortCriteria,
        IList<AlbumSearchCriterion>? SearchCriteria,
        
        [StringLength(75, ErrorMessage = "Title can't be longer than 75 characters")]
        string? Title,

        [DateRange]
        DateRange? DateRange) : PaginationRequest;

    public enum AlbumSortCriterion { InvalidSortCriterion, ByTitle = 10, ByReleaseDate = 20 }

    public enum AlbumSearchCriterion { InvalidSearchCriterion, ByTitle = 10, ByReleaseDateRange = 20 }
}