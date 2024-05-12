using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos.Request
{
    public class SongPaginationRequest : PaginationRequest
    {
        public IDictionary<SongSortCriterion, SortOrder>? SortCriteria { get; set; }
        public IList<SongSearchCriterion>? SearchCriteria { get; set; }
        public string? Title { get; set; }
        public GenreType? Genre { get; set; }
        public DateRange? DateRange { get; set; }
        public DurationRange? DurationRange { get; set; }
        public string? AlbumTitle { get; set; }
        public Guid ArtistId { get; set; }
    }

    public enum SongSortCriterion { ByTitle = 10, ByReleaseDate = 20 }

    public enum SongSearchCriterion { ByTitle = 10, ByGenre = 20, ByReleaseDateRange = 30, ByDurationSecondsRange = 40, ByAlbumTitle = 50, ByArtistName = 60 }
}