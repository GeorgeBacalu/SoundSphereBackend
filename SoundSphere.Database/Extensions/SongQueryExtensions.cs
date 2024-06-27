using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Extensions
{
    public static class SongQueryExtensions
    {
        public static IQueryable<Song> Filter(this IQueryable<Song> query, SongPaginationRequest payload)
        {
            if (payload.SearchCriteria == null || !payload.SearchCriteria.Any())
                return query;
            foreach (var searchCriterion in payload.SearchCriteria)
                query = searchCriterion switch
                {
                    SongSearchCriterion.ByTitle => query.Where(song => song.Title.Contains(payload.Title)),
                    SongSearchCriterion.ByGenre => query.Where(song => song.Genre.Equals(payload.Genre)),
                    SongSearchCriterion.ByReleaseDateRange => query.Where(song => song.ReleaseDate >= payload.DateRange.StartDate && song.ReleaseDate <= payload.DateRange.EndDate),
                    SongSearchCriterion.ByDurationSecondsRange => query.Where(song => song.DurationSeconds >= payload.DurationRange.MinSeconds && song.DurationSeconds <= payload.DurationRange.MaxSeconds),
                    SongSearchCriterion.ByAlbumTitle => query.Where(song => song.Album.Title.Contains(payload.AlbumTitle)),
                    SongSearchCriterion.ByArtistName => query.Where(song => song.Artists.Any(artist => artist.Name.Contains(payload.ArtistName))),
                    _ => query
                };
            return query;
        }

        public static IQueryable<Song> Sort(this IQueryable<Song> query, SongPaginationRequest payload)
        {
            if (payload.SortCriteria == null || !payload.SortCriteria.Any())
                return query.OrderBy(song => song.CreatedAt);
            var firstCriterion = payload.SortCriteria.First();
            var orderedQuery = firstCriterion.Key switch
            {
                SongSortCriterion.ByTitle => firstCriterion.Value == SortOrder.Ascending ? query.OrderBy(song => song.Title) : query.OrderByDescending(song => song.Title),
                SongSortCriterion.ByReleaseDate => firstCriterion.Value == SortOrder.Ascending ? query.OrderBy(song => song.ReleaseDate) : query.OrderByDescending(song => song.ReleaseDate),
                _ => query.OrderBy(song => song.CreatedAt)
            };
            foreach (var sortCriterion in payload.SortCriteria.Skip(1))
                orderedQuery = sortCriterion.Key switch
                {
                    SongSortCriterion.ByTitle => sortCriterion.Value == SortOrder.Ascending ? orderedQuery.ThenBy(song => song.Title) : orderedQuery.ThenByDescending(song => song.Title),
                    SongSortCriterion.ByReleaseDate => sortCriterion.Value == SortOrder.Ascending ? orderedQuery.ThenBy(song => song.ReleaseDate) : orderedQuery.ThenByDescending(song => song.ReleaseDate),
                    _ => orderedQuery
                };
            return orderedQuery;
        }

        public static IQueryable<Song> Paginate(this IQueryable<Song> query, SongPaginationRequest payload) => query.Skip(payload.Page * payload.Size).Take(payload.Size);

        public static IQueryable<Song> ApplyPagination(this IQueryable<Song> query, SongPaginationRequest? payload) => payload == null ? query.Take(10) : query.Filter(payload).Sort(payload).Paginate(payload);
    }
}