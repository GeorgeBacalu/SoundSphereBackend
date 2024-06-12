using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Extensions
{
    public static class ArtistQueryExtensions
    {
        public static IQueryable<Artist> Filter(this IQueryable<Artist> query, ArtistPaginationRequest payload)
        {
            if (payload.SearchCriteria == null || !payload.SearchCriteria.Any())
                return query;
            foreach (var searchCriterion in payload.SearchCriteria)
                query = searchCriterion switch
                {
                    ArtistSearchCriterion.ByName => query.Where(artist => artist.Name.Contains(payload.Name)),
                    _ => query
                };
            return query;
        }

        public static IQueryable<Artist> Sort(this IQueryable<Artist> query, ArtistPaginationRequest payload)
        {
            if (payload.SortCriteria == null || !payload.SortCriteria.Any())
                return query.OrderBy(artist => artist.CreatedAt);
            var firstCriterion = payload.SortCriteria.First();
            var orderedQuery = firstCriterion.Key switch
            {
                ArtistSortCriterion.ByName => firstCriterion.Value == SortOrder.Ascending ? query.OrderBy(artist => artist.Name) : query.OrderByDescending(artist => artist.Name),
                _ => query.OrderBy(artist => artist.CreatedAt)
            };
            foreach (var sortCriterion in payload.SortCriteria.Skip(1))
                orderedQuery = sortCriterion.Key switch
                {
                    ArtistSortCriterion.ByName => sortCriterion.Value == SortOrder.Ascending ? orderedQuery.ThenBy(artist => artist.Name) : orderedQuery.ThenByDescending(artist => artist.Name),
                    _ => orderedQuery
                };
            return query;
        }

        public static IQueryable<Artist> Paginate(this IQueryable<Artist> query, ArtistPaginationRequest payload) => query.Skip(payload.Page * payload.Size).Take(payload.Size);
    }
}