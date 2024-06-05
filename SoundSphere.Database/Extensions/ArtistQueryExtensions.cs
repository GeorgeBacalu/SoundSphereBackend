using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Extensions
{
    public static class ArtistQueryExtensions
    {
        public static IQueryable<Artist> Filter(this IQueryable<Artist> query, ArtistPaginationRequest payload) =>
            payload.SearchCriteria == null || !payload.SearchCriteria.Any() ? query :
            payload.SearchCriteria.Aggregate(query, (current, searchCriterion) => searchCriterion switch
            {
                ArtistSearchCriterion.ByName => current.Where(artist => artist.Name.Contains(payload.Name)),
                _ => current
            });

        public static IQueryable<Artist> Sort(this IQueryable<Artist> query, ArtistPaginationRequest payload) =>
            payload.SortCriteria == null || !payload.SortCriteria.Any() ? query.OrderBy(artist => artist.CreatedAt) :
            payload.SortCriteria.Aggregate(query, (current, sortCriterion) => sortCriterion.Key switch
            {
                ArtistSortCriterion.ByName => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(artist => artist.Name) : current.OrderByDescending(artist => artist.Name),
                _ => current.OrderBy(artist => artist.CreatedAt)
            });

        public static IQueryable<Artist> Paginate(this IQueryable<Artist> query, ArtistPaginationRequest payload) => query.Skip(payload.Page * payload.Size).Take(payload.Size);
    }
}