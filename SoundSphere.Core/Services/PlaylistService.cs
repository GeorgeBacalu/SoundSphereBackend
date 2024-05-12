using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISongRepository _songRepository;
        private readonly IMapper _mapper;

        public PlaylistService(IPlaylistRepository playlistRepository, IUserRepository userRepository, ISongRepository songRepository, IMapper mapper) => 
            (_playlistRepository, _userRepository, _songRepository, _mapper) = (playlistRepository, userRepository, songRepository, mapper);

        public IList<PlaylistDto> FindAll() => _playlistRepository.FindAll().ToDtos(_mapper);

        public IList<PlaylistDto> FindAllActive() => _playlistRepository.FindAllActive().ToDtos(_mapper);

        public IList<PlaylistDto> FindAllPagination(PlaylistPaginationRequest payload) => _playlistRepository.FindAllPagination(payload).ToDtos(_mapper);

        public IList<PlaylistDto> FindAllActivePagination(PlaylistPaginationRequest payload) => _playlistRepository.FindAllActivePagination(payload).ToDtos(_mapper);

        public PlaylistDto FindById(Guid id) => _playlistRepository.FindById(id).ToDto(_mapper);

        public PlaylistDto Save(PlaylistDto playlistDto)
        {
            Playlist playlist = playlistDto.ToEntity(_userRepository, _songRepository, _mapper);
            if (playlist.Id == Guid.Empty) playlist.Id = Guid.NewGuid();
            playlist.IsActive = true;
            playlist.CreatedAt = DateTime.Now;
            _playlistRepository.LinkPlaylistToUser(playlist);
            return _playlistRepository.Save(playlist).ToDto(_mapper);
        }

        public PlaylistDto UpdateById(PlaylistDto playlistDto, Guid id) => _playlistRepository.UpdateById(playlistDto.ToEntity(_userRepository, _songRepository, _mapper), id).ToDto(_mapper);

        public PlaylistDto DisableById(Guid id) => _playlistRepository.DisableById(id).ToDto(_mapper);
    }
}