using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Extensions
{
    public static class SongQueryExtensions
    {
        public static IQueryable<Song> Filter(this IQueryable<Song> query, SongPaginationRequest payload)
        {
            if (payload.SearchCriteria == null || !payload.SearchCriteria.Any()) return query;
            return payload.SearchCriteria.Aggregate(query, (current, searchCriterion) => searchCriterion switch
            {
                SongSearchCriterion.ByTitle => current.Where(song => song.Title.Contains(payload.Title)),
                SongSearchCriterion.ByGenre => current.Where(song => song.Genre.Equals(payload.Genre)),
                SongSearchCriterion.ByReleaseDateRange => current.Where(song => song.ReleaseDate >= payload.DateRange.StartDate && song.ReleaseDate <= payload.DateRange.EndDate),
                SongSearchCriterion.ByDurationSecondsRange => current.Where(song => song.DurationSeconds >= payload.DurationRange.MinSeconds && song.DurationSeconds <= payload.DurationRange.MaxSeconds),
                SongSearchCriterion.ByAlbumTitle => current.Where(song => song.Album.Title.Contains(payload.AlbumTitle)),
                SongSearchCriterion.ByArtistName => current.Where(song => song.Artists.Select(artist => artist.Id).Contains(payload.ArtistId)),
                _ => current
            });
        }

        public static IQueryable<Song> Sort(this IQueryable<Song> query, SongPaginationRequest payload)
        {
            if (payload.SortCriteria == null || !payload.SortCriteria.Any()) return query;
            return payload.SortCriteria.Aggregate(query, (current, sortCriterion) => sortCriterion.Key switch
            {
                SongSortCriterion.ByTitle => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(song => song.Title) : current.OrderByDescending(song => song.Title),
                SongSortCriterion.ByReleaseDate => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(song => song.ReleaseDate) : current.OrderByDescending(song => song.ReleaseDate),
                _ => current
            });
        }

        public static IQueryable<Song> Paginate(this IQueryable<Song> query, SongPaginationRequest payload) => query.Skip((payload.Page - 1) * payload.Size).Take(payload.Size);
    }
}