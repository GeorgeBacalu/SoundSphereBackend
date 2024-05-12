using Microsoft.Data.SqlClient;

namespace SoundSphere.Database.Dtos.Request
{
    public class ArtistPaginationRequest : PaginationRequest
    {
        public IDictionary<ArtistSortCriterion, SortOrder>? SortCriteria { get; set; }
        public IList<ArtistSearchCriterion>? SearchCriteria { get; set; }
        public string? Name { get; set; }
    }

    public enum ArtistSortCriterion { ByName = 10 }

    public enum ArtistSearchCriterion { ByName = 10 }
}