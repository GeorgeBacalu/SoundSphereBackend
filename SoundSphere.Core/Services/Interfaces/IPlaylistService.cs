using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IPlaylistService
    {
        IList<PlaylistDto> GetAll(PlaylistPaginationRequest? payload, Guid userId);

        PlaylistDto GetById(Guid playlistId, Guid userId);

        PlaylistDto Add(PlaylistDto playlistDto, Guid userId);

        PlaylistDto UpdateById(PlaylistDto playlistDto, Guid playlistId, Guid userId);

        PlaylistDto DeleteById(Guid playlistId, Guid userId);

        PlaylistDto AddSong(Guid playlistId, Guid songId, Guid userId);

        PlaylistDto RemoveSong(Guid playlistId, Guid songId, Guid userId);
    }
}