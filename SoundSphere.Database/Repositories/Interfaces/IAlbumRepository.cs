using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IAlbumRepository
    {
        IList<Album> FindAll();

        Album FindById(Guid id);

        Album Save(Album album);

        Album UpdateById(Album album, Guid id);

        Album DisableById(Guid id);
    }
}