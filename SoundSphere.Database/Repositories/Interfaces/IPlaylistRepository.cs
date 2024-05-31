using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IPlaylistRepository
    {
        IList<Playlist> GetAll();

        IList<Playlist> GetAllActive();

        IList<Playlist> GetAllPagination(PlaylistPaginationRequest payload);

        IList<Playlist> GetAllActivePagination(PlaylistPaginationRequest payload);

        Playlist GetById(Guid id);

        Playlist Add(Playlist playlist);

        Playlist UpdateById(Playlist playlist, Guid id);

        Playlist DeleteById(Guid id);

        void LinkPlaylistToUser(Playlist playlist);
    }
}