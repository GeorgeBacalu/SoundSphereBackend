using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISongRepository _songRepository;

        public PlaylistService(IPlaylistRepository playlistRepository, IUserRepository userRepository, ISongRepository songRepository)
        {
            _playlistRepository = playlistRepository;
            _userRepository = userRepository;
            _songRepository = songRepository;
        }

        public IList<PlaylistDto> FindAll() => ConvertToDtos(_playlistRepository.FindAll());

        public PlaylistDto FindById(Guid id) => ConvertToDto(_playlistRepository.FindById(id));

        public PlaylistDto Save(PlaylistDto playlistDto)
        {
            Playlist playlist = ConvertToEntity(playlistDto);
            if (playlist.Id == Guid.Empty) playlist.Id = Guid.NewGuid();
            playlist.IsActive = true;
            playlist.CreatedAt = DateTime.Now;
            _playlistRepository.LinkPlaylistToUser(playlist);
            return ConvertToDto(_playlistRepository.Save(playlist));
        }

        public PlaylistDto UpdateById(PlaylistDto playlistDto, Guid id) => ConvertToDto(_playlistRepository.UpdateById(ConvertToEntity(playlistDto), id));

        public PlaylistDto DisableById(Guid id) => ConvertToDto(_playlistRepository.DisableById(id));

        public IList<PlaylistDto> ConvertToDtos(IList<Playlist> playlists) => playlists.Select(ConvertToDto).ToList();

        public IList<Playlist> ConvertToEntities(IList<PlaylistDto> playlistDtos) => playlistDtos.Select(ConvertToEntity).ToList();

        public PlaylistDto ConvertToDto(Playlist playlist) => new PlaylistDto
        {
            Id = playlist.Id,
            Title = playlist.Title,
            UserId = playlist.User.Id,
            SongsIds = playlist.Songs
                    .Select(song => song.Id)
                    .ToList(),
            CreatedAt = playlist.CreatedAt,
            IsActive = playlist.IsActive
        };

        public Playlist ConvertToEntity(PlaylistDto playlistDto) => new Playlist
        {
            Id = playlistDto.Id,
            Title = playlistDto.Title,
            User = _userRepository.FindById(playlistDto.UserId),
            Songs = playlistDto.SongsIds
                    .Select(_songRepository.FindById)
                    .ToList(),
            CreatedAt = playlistDto.CreatedAt,
            IsActive = playlistDto.IsActive
        };
    }
}