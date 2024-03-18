using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IPlaylistService
    {
        IList<PlaylistDto> FindAll();

        PlaylistDto FindById(Guid id);

        PlaylistDto Save(PlaylistDto playlistDto);

        PlaylistDto UpdateById(PlaylistDto playlistDto, Guid id);

        PlaylistDto DisableById(Guid id);

        IList<PlaylistDto> ConvertToDtos(IList<Playlist> playlists);

        IList<Playlist> ConvertToEntities(IList<PlaylistDto> playlistDtos);

        PlaylistDto ConvertToDto(Playlist playlist);

        Playlist ConvertToEntity(PlaylistDto playlistDto);
    }
}