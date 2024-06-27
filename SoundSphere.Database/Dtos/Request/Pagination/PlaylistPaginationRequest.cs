using Microsoft.Data.SqlClient;
using SoundSphere.Database.Attributes;
using SoundSphere.Database.Dtos.Request.Models;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Request.Pagination
{
    public record PlaylistPaginationRequest(
        IDictionary<PlaylistSortCriterion, SortOrder>? SortCriteria,
        IList<PlaylistSearchCriterion>? SearchCriteria,

        [DateRange]
        DateTimeRange? DateRange,

        [StringLength(75, ErrorMessage = "Title can't be longer than 75 characters")]
        string? Title,

        [StringLength(75, ErrorMessage = "User name can't be longer than 75 characters")]
        string? UserName,

        [StringLength(75, ErrorMessage = "Song title can't be longer than 75 characters")]
        string? SongTitle) : PaginationRequest;

    public enum PlaylistSortCriterion { InvalidSortCriterion, ByCreateDate = 10, ByTitle = 20 }

    public enum PlaylistSearchCriterion { InvalidSearchCriterion, ByUserName = 10, ByTitle = 20, ByCreateDateRange = 30, BySongTitle = 40 }
}