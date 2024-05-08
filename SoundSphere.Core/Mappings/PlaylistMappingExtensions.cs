using AutoMapper;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Mappings
{
    public static class PlaylistMappingExtensions
    {
        public static IList<PlaylistDto> ToDtos(this IList<Playlist> playlists, IMapper mapper) => playlists.Select(playlist => playlist.ToDto(mapper)).ToList();

        public static IList<Playlist> ToEntities(this IList<PlaylistDto> playlistDtos, IUserRepository userRepository, ISongRepository songRepository, IMapper mapper) =>
            playlistDtos.Select(playlistDto => playlistDto.ToEntity(userRepository, songRepository, mapper)).ToList();

        public static PlaylistDto ToDto(this Playlist playlist, IMapper mapper)
        {
            PlaylistDto playlistDto = mapper.Map<PlaylistDto>(playlist);
            playlistDto.SongsIds = playlist.Songs.Select(song => song.Id).ToList();
            return playlistDto;
        }

        public static Playlist ToEntity(this PlaylistDto playlistDto, IUserRepository userRepository, ISongRepository songRepository, IMapper mapper)
        {
            Playlist playlist = mapper.Map<Playlist>(playlistDto);
            playlist.User = userRepository.FindById(playlistDto.UserId);
            playlist.Songs = playlistDto.SongsIds.Select(songRepository.FindById).ToList();
            return playlist;
        }
    }
}