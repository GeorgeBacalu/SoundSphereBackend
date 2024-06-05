using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos.Request
{
    public record SongPaginationRequest(
        IDictionary<SongSortCriterion, SortOrder>? SortCriteria,
        IList<SongSearchCriterion>? SearchCriteria,
        string? Title,
        GenreType? Genre,
        DateRange? DateRange,
        DurationRange? DurationRange,
        string? AlbumTitle,
        string ArtistName
        ) : PaginationRequest;

    public enum SongSortCriterion { ByTitle = 10, ByReleaseDate = 20 }

    public enum SongSearchCriterion { ByTitle = 10, ByGenre = 20, ByReleaseDateRange = 30, ByDurationSecondsRange = 40, ByAlbumTitle = 50, ByArtistName = 60 }
}