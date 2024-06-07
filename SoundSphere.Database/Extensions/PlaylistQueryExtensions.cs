using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Extensions
{
    public static class PlaylistQueryExtensions
    {
        public static IQueryable<Playlist> Filter(this IQueryable<Playlist> query, PlaylistPaginationRequest payload) =>
            payload.SearchCriteria == null || !payload.SearchCriteria.Any() ? query :
            payload.SearchCriteria.Aggregate(query, (current, searchCriterion) => searchCriterion switch
            {
                PlaylistSearchCriterion.ByUserName => current.Where(playlist => playlist.User.Name.Contains(payload.UserName)),
                PlaylistSearchCriterion.ByTitle => current.Where(playlist => playlist.Title.Contains(payload.Title)),
                PlaylistSearchCriterion.ByCreateDateRange => current.Where(playlist => playlist.CreatedAt >= payload.DateRange.StartDate && playlist.CreatedAt <= payload.DateRange.EndDate),
                PlaylistSearchCriterion.BySongTitle => current.Where(playlist => playlist.Songs.Any(song => song.Title.Contains(payload.SongTitle))),
                _ => current
            });

        public static IQueryable<Playlist> Sort(this IQueryable<Playlist> query, PlaylistPaginationRequest payload) =>
            payload.SortCriteria == null || !payload.SortCriteria.Any() ? query.OrderBy(playlist => playlist.CreatedAt) :
            payload.SortCriteria.Aggregate(query, (current, sortCriterion) => sortCriterion.Key switch
            {
                PlaylistSortCriterion.ByCreateDate => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(playlist => playlist.CreatedAt) : current.OrderByDescending(playlist => playlist.CreatedAt),
                PlaylistSortCriterion.ByTitle => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(playlist => playlist.Title) : current.OrderByDescending(playlist => playlist.Title),
                _ => current.OrderBy(playlist => playlist.CreatedAt)
            });

        public static IQueryable<Playlist> Paginate(this IQueryable<Playlist> query, PlaylistPaginationRequest payload) => query.Skip(payload.Page * payload.Size).Take(payload.Size);
    }
}