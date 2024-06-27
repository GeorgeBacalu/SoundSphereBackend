using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Extensions
{
    public static class PlaylistQueryExtensions
    {
        public static IQueryable<Playlist> Filter(this IQueryable<Playlist> query, PlaylistPaginationRequest payload)
        {
            if (payload.SearchCriteria == null || !payload.SearchCriteria.Any())
                return query;
            foreach (var searchCriterion in payload.SearchCriteria)
                query = searchCriterion switch
                {
                    PlaylistSearchCriterion.ByUserName => query.Where(playlist => playlist.User.Name.Contains(payload.UserName)),
                    PlaylistSearchCriterion.ByTitle => query.Where(playlist => playlist.Title.Contains(payload.Title)),
                    PlaylistSearchCriterion.ByCreateDateRange => query.Where(playlist => playlist.CreatedAt >= payload.DateRange.StartDate && playlist.CreatedAt <= payload.DateRange.EndDate),
                    PlaylistSearchCriterion.BySongTitle => query.Where(playlist => playlist.Songs.Any(song => song.Title.Contains(payload.SongTitle))),
                    _ => query
                };
            return query;
        }

        public static IQueryable<Playlist> Sort(this IQueryable<Playlist> query, PlaylistPaginationRequest payload)
        {
            if (payload.SortCriteria == null || !payload.SortCriteria.Any())
                return query.OrderBy(playlist => playlist.CreatedAt);
            var firstCriterion = payload.SortCriteria.First();
            var orderedQuery = firstCriterion.Key switch
            {
                PlaylistSortCriterion.ByCreateDate => firstCriterion.Value == SortOrder.Ascending ? query.OrderBy(playlist => playlist.CreatedAt) : query.OrderByDescending(playlist => playlist.CreatedAt),
                PlaylistSortCriterion.ByTitle => firstCriterion.Value == SortOrder.Ascending ? query.OrderBy(playlist => playlist.Title) : query.OrderByDescending(playlist => playlist.Title),
                _ => query.OrderBy(playlist => playlist.CreatedAt)
            };
            foreach (var sortCriterion in payload.SortCriteria.Skip(1))
                orderedQuery = sortCriterion.Key switch
                {
                    PlaylistSortCriterion.ByCreateDate => sortCriterion.Value == SortOrder.Ascending ? orderedQuery.ThenBy(playlist => playlist.CreatedAt) : orderedQuery.ThenByDescending(playlist => playlist.CreatedAt),
                    PlaylistSortCriterion.ByTitle => sortCriterion.Value == SortOrder.Ascending ? orderedQuery.ThenBy(playlist => playlist.Title) : orderedQuery.ThenByDescending(playlist => playlist.Title),
                    _ => orderedQuery
                };
            return orderedQuery;
        }

        public static IQueryable<Playlist> Paginate(this IQueryable<Playlist> query, PlaylistPaginationRequest payload) => query.Skip(payload.Page * payload.Size).Take(payload.Size);

        public static IQueryable<Playlist> ApplyPagination(this IQueryable<Playlist> query, PlaylistPaginationRequest? payload) => payload == null ? query.Take(10) : query.Filter(payload).Sort(payload).Paginate(payload);
    }
}