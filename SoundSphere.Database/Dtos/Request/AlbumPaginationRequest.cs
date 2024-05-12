using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;

namespace SoundSphere.Database.Dtos.Request
{
    public class AlbumPaginationRequest : PaginationRequest
    {
        public IDictionary<AlbumSortCriterion, SortOrder>? SortCriteria { get; set; }
        public IList<AlbumSearchCriterion>? SearchCriteria { get; set; }
        public string? Title { get; set; }
        public DateRange? DateRange { get; set; }
    }

    public enum AlbumSortCriterion { ByTitle = 10, ByReleaseDate = 20 }

    public enum AlbumSearchCriterion { ByTitle = 10, ByReleaseDateRange = 20 }
}