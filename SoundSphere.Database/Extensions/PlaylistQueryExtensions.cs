using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request;
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
                PlaylistSearchCriterion.ByCreatedDateRange => current.Where(playlist => playlist.CreatedAt >= payload.DateRange.StartDate && playlist.CreatedAt <= payload.DateRange.EndDate),
                PlaylistSearchCriterion.BySongTitle => current.Where(playlist => playlist.Songs.Select(song => song.Id).Contains(payload.SongId)),
                _ => current
            });

        public static IQueryable<Playlist> Sort(this IQueryable<Playlist> query, PlaylistPaginationRequest payload) =>
            payload.SortCriteria == null || !payload.SortCriteria.Any() ? query :
            payload.SortCriteria.Aggregate(query, (current, sortCriterion) => sortCriterion.Key switch
            {
                PlaylistSortCriterion.ByCreatedDate => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(playlist => playlist.CreatedAt) : current.OrderByDescending(playlist => playlist.CreatedAt),
                PlaylistSortCriterion.ByTitle => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(playlist => playlist.Title) : current.OrderByDescending(playlist => playlist.Title),
                _ => current
            });

        public static IQueryable<Playlist> Paginate(this IQueryable<Playlist> query, PlaylistPaginationRequest payload) => query.Skip(payload.Page * payload.Size).Take(payload.Size);
    }
}