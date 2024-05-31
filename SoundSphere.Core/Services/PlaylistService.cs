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

        public IList<PlaylistDto> GetAll() => _playlistRepository.GetAll().ToDtos(_mapper);

        public IList<PlaylistDto> GetAllActive() => _playlistRepository.GetAllActive().ToDtos(_mapper);

        public IList<PlaylistDto> GetAllPagination(PlaylistPaginationRequest payload) => _playlistRepository.GetAllPagination(payload).ToDtos(_mapper);

        public IList<PlaylistDto> GetAllActivePagination(PlaylistPaginationRequest payload) => _playlistRepository.GetAllActivePagination(payload).ToDtos(_mapper);

        public PlaylistDto GetById(Guid id) => _playlistRepository.GetById(id).ToDto(_mapper);

        public PlaylistDto Add(PlaylistDto playlistDto)
        {
            Playlist playlist = playlistDto.ToEntity(_userRepository, _songRepository, _mapper);
            _playlistRepository.LinkPlaylistToUser(playlist);
            return _playlistRepository.Add(playlist).ToDto(_mapper);
        }

        public PlaylistDto UpdateById(PlaylistDto playlistDto, Guid id) => _playlistRepository.UpdateById(playlistDto.ToEntity(_userRepository, _songRepository, _mapper), id).ToDto(_mapper);

        public PlaylistDto DeleteById(Guid id) => _playlistRepository.DeleteById(id).ToDto(_mapper);
    }
}