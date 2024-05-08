using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Extensions
{
    public static class ArtistQueryExtensions
    {
        public static IQueryable<Artist> Filter(this IQueryable<Artist> query, ArtistPaginationRequest payload)
        {
            if (payload.SearchCriteria == null || !payload.SearchCriteria.Any()) return query;
            return payload.SearchCriteria.Aggregate(query, (current, searchCriterion) => searchCriterion switch
            {
                ArtistSearchCriterion.ByName => current.Where(artist => artist.Name.Contains(payload.Name)),
                _ => current
            });
        }

        public static IQueryable<Artist> Sort(this IQueryable<Artist> query, ArtistPaginationRequest payload)
        {
            if (payload.SortCriteria == null || !payload.SortCriteria.Any()) return query;
            return payload.SortCriteria.Aggregate(query, (current, searchCriterion) => searchCriterion.Key switch
            {
                ArtistSortCriterion.ByName => searchCriterion.Value == SortOrder.Ascending ? current.OrderBy(artist => artist.Name) : current.OrderByDescending(artist => artist.Name),
                _ => current
            });
        }

        public static IQueryable<Artist> Paginate(this IQueryable<Artist> query, ArtistPaginationRequest payload) => query.Skip((payload.Page - 1) * payload.Size).Take(payload.Size);
    }
}