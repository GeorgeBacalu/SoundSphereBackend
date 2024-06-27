using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface ISongRepository
    {
        IList<Song> GetAll(SongPaginationRequest? payload);

        Song GetById(Guid id);

        Song Add(Song song);

        Song UpdateById(Song song, Guid id);

        Song DeleteById(Guid id);

        int CountByDateRangeAndGenre(DateTime? startDate, DateTime? endDate, GenreType? genre);

        void LinkSongToAlbum(Song song);

        void LinkSongToArtists(Song song);

        void AddSongLink(Song song);

        void AddUserSong(Song song);
    }
}