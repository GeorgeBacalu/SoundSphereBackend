using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IPlaylistRepository
    {
        IList<Playlist> FindAll();

        IList<Playlist> FindAllActive();

        IList<Playlist> FindAllPagination(PlaylistPaginationRequest payload);

        IList<Playlist> FindAllActivePagination(PlaylistPaginationRequest payload);

        Playlist FindById(Guid id);

        Playlist Save(Playlist playlist);

        Playlist UpdateById(Playlist playlist, Guid id);

        Playlist DisableById(Guid id);

        void LinkPlaylistToUser(Playlist playlist);
    }
}