using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface ISongRepository
    {
        IList<Song> FindAll();

        Song FindById(Guid id);

        Song Save(Song song);

        Song UpdateById(Song song, Guid id);

        Song DisableById(Guid id);

        void LinkSongToAlbum(Song song);

        void LinkSongToArtist(Song song);

        void AddSongLinks(Song song);

        void AddUserSong(Song song);
    }
}