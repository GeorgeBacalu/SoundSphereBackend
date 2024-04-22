using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface ISongRepository
    {
        IList<Song> FindAll();

        IList<Song> FindAllActive();

        Song FindById(Guid id);

        Song Save(Song song);

        Song UpdateById(Song song, Guid id);

        Song DisableById(Guid id);

        void LinkSongToAlbum(Song song);

        void LinkSongToArtists(Song song);

        void AddSongLink(Song song);

        void AddUserSong(Song song);
    }
}