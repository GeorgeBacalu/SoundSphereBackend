using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IPlaylistService
    {
        IList<PlaylistDto> GetAll();

        IList<PlaylistDto> GetAllActive();

        IList<PlaylistDto> GetAllPagination(PlaylistPaginationRequest payload);

        IList<PlaylistDto> GetAllActivePagination(PlaylistPaginationRequest payload);

        PlaylistDto GetById(Guid id);

        PlaylistDto Add(PlaylistDto playlistDto);

        PlaylistDto UpdateById(PlaylistDto playlistDto, Guid id);

        PlaylistDto DeleteById(Guid id);
    }
}