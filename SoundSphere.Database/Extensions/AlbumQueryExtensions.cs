using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Extensions
{
    public static class AlbumQueryExtensions
    {
        public static IQueryable<Album> Filter(this IQueryable<Album> query, AlbumPaginationRequest payload)
        {
            if (payload.SearchCriteria == null || !payload.SearchCriteria.Any())
                return query;
            foreach (var searchCriterion in payload.SearchCriteria)
                query = searchCriterion switch
                {
                    AlbumSearchCriterion.ByTitle => query.Where(album => album.Title.Contains(payload.Title)),
                    AlbumSearchCriterion.ByReleaseDateRange => query.Where(album => album.ReleaseDate >= payload.DateRange.StartDate && album.ReleaseDate <= payload.DateRange.EndDate),
                    _ => query
                };
            return query;
        }

        public static IQueryable<Album> Sort(this IQueryable<Album> query, AlbumPaginationRequest payload)
        {
            if (payload.SortCriteria == null || !payload.SortCriteria.Any())
                return query.OrderBy(album => album.CreatedAt);
            var firstCriterion = payload.SortCriteria.First();
            var orderedQuery = firstCriterion.Key switch
            {
                AlbumSortCriterion.ByTitle => firstCriterion.Value == SortOrder.Ascending ? query.OrderBy(album => album.Title) : query.OrderByDescending(album => album.Title),
                AlbumSortCriterion.ByReleaseDate => firstCriterion.Value == SortOrder.Ascending ? query.OrderBy(album => album.ReleaseDate) : query.OrderByDescending(album => album.ReleaseDate),
                _ => query.OrderBy(album => album.CreatedAt)
            };
            foreach (var sortCriterion in payload.SortCriteria.Skip(1))
                orderedQuery = sortCriterion.Key switch
                {
                    AlbumSortCriterion.ByTitle => sortCriterion.Value == SortOrder.Ascending ? orderedQuery.ThenBy(album => album.Title) : orderedQuery.ThenByDescending(album => album.Title),
                    AlbumSortCriterion.ByReleaseDate => sortCriterion.Value == SortOrder.Ascending ? orderedQuery.ThenBy(album => album.ReleaseDate) : orderedQuery.ThenByDescending(album => album.ReleaseDate),
                    _ => orderedQuery
                };
            return orderedQuery;
        }

        public static IQueryable<Album> Paginate(this IQueryable<Album> query, AlbumPaginationRequest payload) => query.Skip(payload.Page * payload.Size).Take(payload.Size);
    }
}