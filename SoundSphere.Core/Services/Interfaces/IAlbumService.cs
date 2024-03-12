using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IAlbumService
    {
        IList<Album> FindAll();

        Album FindById(Guid id);

        Album Save(Album album);

        Album UpdateById(Album album, Guid id);

        Album DisableById(Guid id);
    }
}