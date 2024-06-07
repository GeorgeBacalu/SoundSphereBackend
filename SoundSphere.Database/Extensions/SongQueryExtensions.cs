using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Extensions
{
    public static class SongQueryExtensions
    {
        public static IQueryable<Song> Filter(this IQueryable<Song> query, SongPaginationRequest payload) =>
            payload.SearchCriteria == null || !payload.SearchCriteria.Any() ? query :
            payload.SearchCriteria.Aggregate(query, (current, searchCriterion) => searchCriterion switch
            {
                SongSearchCriterion.ByTitle => current.Where(song => song.Title.Contains(payload.Title)),
                SongSearchCriterion.ByGenre => current.Where(song => song.Genre.Equals(payload.Genre)),
                SongSearchCriterion.ByReleaseDateRange => current.Where(song => song.ReleaseDate >= payload.DateRange.StartDate && song.ReleaseDate <= payload.DateRange.EndDate),
                SongSearchCriterion.ByDurationSecondsRange => current.Where(song => song.DurationSeconds >= payload.DurationRange.MinSeconds && song.DurationSeconds <= payload.DurationRange.MaxSeconds),
                SongSearchCriterion.ByAlbumTitle => current.Where(song => song.Album.Title.Contains(payload.AlbumTitle)),
                SongSearchCriterion.ByArtistName => current.Where(song => song.Artists.Any(artist => artist.Name.Contains(payload.ArtistName))),
                _ => current
            });

        public static IQueryable<Song> Sort(this IQueryable<Song> query, SongPaginationRequest payload) =>
            payload.SortCriteria == null || !payload.SortCriteria.Any() ? query.OrderBy(song => song.CreatedAt) :
            payload.SortCriteria.Aggregate(query, (current, sortCriterion) => sortCriterion.Key switch
            {
                SongSortCriterion.ByTitle => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(song => song.Title) : current.OrderByDescending(song => song.Title),
                SongSortCriterion.ByReleaseDate => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(song => song.ReleaseDate) : current.OrderByDescending(song => song.ReleaseDate),
                _ => current.OrderBy(song => song.CreatedAt)
            });

        public static IQueryable<Song> Paginate(this IQueryable<Song> query, SongPaginationRequest payload) => query.Skip(payload.Page * payload.Size).Take(payload.Size);
    }
}