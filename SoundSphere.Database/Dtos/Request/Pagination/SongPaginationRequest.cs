using Microsoft.Data.SqlClient;
using SoundSphere.Database.Attributes;
using SoundSphere.Database.Dtos.Request.Models;
using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Request.Pagination
{
    public record SongPaginationRequest(
        IDictionary<SongSortCriterion, SortOrder>? SortCriteria,
        IList<SongSearchCriterion>? SearchCriteria,

        [StringLength(75, ErrorMessage = "Title can't be longer than 75 characters")]
        string? Title,
        
        GenreType? Genre,

        [DateRange]
        DateRange? DateRange,

        [DurationRange]
        DurationRange? DurationRange,
        
        [StringLength(75, ErrorMessage = "Album title can't be longer than 75 characters")]
        string? AlbumTitle,

        [StringLength(75, ErrorMessage = "Artist name can't be longer than 75 characters")]
        string? ArtistName) : PaginationRequest;

    public enum SongSortCriterion { InvalidSortCriterion, ByTitle = 10, ByReleaseDate = 20 }

    public enum SongSearchCriterion { InvalidSearchCriterion, ByTitle = 10, ByGenre = 20, ByReleaseDateRange = 30, ByDurationSecondsRange = 40, ByAlbumTitle = 50, ByArtistName = 60 }
}