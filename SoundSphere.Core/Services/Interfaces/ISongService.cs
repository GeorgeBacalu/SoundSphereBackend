using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface ISongService
    {
        IList<Song> FindAll();

        Song FindById(Guid id);

        Song Save(Song song);

        Song UpdateById(Song song, Guid id);

        Song DisableById(Guid id);
    }
}