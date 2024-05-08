using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IAlbumRepository
    {
        IList<Album> FindAll();

        IList<Album> FindAllActive();

        IList<Album> FindAllPagination(AlbumPaginationRequest payload);

        IList<Album> FindAllActivePagination(AlbumPaginationRequest payload);

        Album FindById(Guid id);

        Album Save(Album album);

        Album UpdateById(Album album, Guid id);

        Album DisableById(Guid id);

        void AddAlbumLink(Album album);
    }
}