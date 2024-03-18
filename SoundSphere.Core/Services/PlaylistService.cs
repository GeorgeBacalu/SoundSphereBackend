using AutoMapper;
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
        private readonly IMapper _mapper;

        public PlaylistService(IPlaylistRepository playlistRepository, IUserRepository userRepository, ISongRepository songRepository, IMapper mapper)
        {
            _playlistRepository = playlistRepository;
            _userRepository = userRepository;
            _songRepository = songRepository;
            _mapper = mapper;
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

        public PlaylistDto ConvertToDto(Playlist playlist)
        {
            PlaylistDto playlistDto = _mapper.Map<PlaylistDto>(playlist);
            playlistDto.UserId = playlist.User.Id;
            playlistDto.SongsIds = playlist.Songs
                .Select(song => song.Id)
                .ToList();
            return playlistDto;
        }

        public Playlist ConvertToEntity(PlaylistDto playlistDto)
        {
            Playlist playlist = _mapper.Map<Playlist>(playlistDto);
            playlist.User = _userRepository.FindById(playlistDto.UserId);
            playlist.Songs = playlistDto.SongsIds
                .Select(id => _songRepository.FindById(id))
                .ToList();
            return playlist;
        }
    }
}