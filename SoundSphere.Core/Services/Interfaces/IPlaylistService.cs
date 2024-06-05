using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IPlaylistService
    {
        IList<PlaylistDto> GetAll(PlaylistPaginationRequest payload);

        PlaylistDto GetById(Guid id);

        PlaylistDto Add(PlaylistDto playlistDto);

        PlaylistDto UpdateById(PlaylistDto playlistDto, Guid id);

        PlaylistDto DeleteById(Guid id);
    }
}