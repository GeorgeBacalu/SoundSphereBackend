using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IPlaylistRepository
    {
        IList<Playlist> GetAll(PlaylistPaginationRequest? payload, Guid userId);

        Playlist GetById(Guid id);

        Playlist Add(Playlist playlist);

        Playlist UpdateById(Playlist playlist, Guid id);

        Playlist DeleteById(Guid id);

        void LinkPlaylistToUser(Playlist playlist);
    }
}