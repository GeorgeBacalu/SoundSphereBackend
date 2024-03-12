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
    }
}