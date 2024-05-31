using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Extensions
{
    public static class AlbumQueryExtensions
    {
        public static IQueryable<Album> Filter(this IQueryable<Album> query, AlbumPaginationRequest payload) =>
            payload.SearchCriteria == null || !payload.SearchCriteria.Any() ? query :
            payload.SearchCriteria.Aggregate(query, (current, searchCriterion) => searchCriterion switch
            {
                AlbumSearchCriterion.ByTitle => current.Where(album => album.Title.Contains(payload.Title)),
                AlbumSearchCriterion.ByReleaseDateRange => current.Where(album => album.ReleaseDate >= payload.DateRange.StartDate && album.ReleaseDate <= payload.DateRange.EndDate),
                _ => current
            });

        public static IQueryable<Album> Sort(this IQueryable<Album> query, AlbumPaginationRequest payload) =>
            payload.SortCriteria == null || !payload.SortCriteria.Any() ? query :
            payload.SortCriteria.Aggregate(query, (current, sortCriterion) => sortCriterion.Key switch
            {
                AlbumSortCriterion.ByTitle => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(album => album.Title) : current.OrderByDescending(album => album.Title),
                AlbumSortCriterion.ByReleaseDate => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(album => album.ReleaseDate) : current.OrderByDescending(album => album.ReleaseDate),
                _ => current
            });

        public static IQueryable<Album> Paginate(this IQueryable<Album> query, AlbumPaginationRequest payload) => query.Skip(payload.Page * payload.Size).Take(payload.Size);
    }
}