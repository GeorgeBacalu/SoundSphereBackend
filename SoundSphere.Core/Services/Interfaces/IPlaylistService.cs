using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IPlaylistService
    {
        IList<PlaylistDto> FindAll();

        IList<PlaylistDto> FindAllActive();

        IList<PlaylistDto> FindAllPagination(PlaylistPaginationRequest payload);

        IList<PlaylistDto> FindAllActivePagination(PlaylistPaginationRequest payload);

        PlaylistDto FindById(Guid id);

        PlaylistDto Save(PlaylistDto playlistDto);

        PlaylistDto UpdateById(PlaylistDto playlistDto, Guid id);

        PlaylistDto DisableById(Guid id);
    }
}