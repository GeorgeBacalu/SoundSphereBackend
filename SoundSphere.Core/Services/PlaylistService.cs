using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
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

        public IList<PlaylistDto> GetAll(PlaylistPaginationRequest payload)
        {
            IList<PlaylistDto> playlistDtos = _playlistRepository.GetAll(payload).ToDtos(_mapper);
            return playlistDtos;
        }

        public PlaylistDto GetById(Guid id)
        {
            PlaylistDto playlistDto = _playlistRepository.GetById(id).ToDto(_mapper);
            return playlistDto;
        }

        public PlaylistDto Add(PlaylistDto playlistDto)
        {
            Playlist playlistToCreate = playlistDto.ToEntity(_userRepository, _songRepository, _mapper);
            _playlistRepository.LinkPlaylistToUser(playlistToCreate);
            PlaylistDto createdPlaylistDto = _playlistRepository.Add(playlistToCreate).ToDto(_mapper);
            return createdPlaylistDto;
        }

        public PlaylistDto UpdateById(PlaylistDto playlistDto, Guid id)
        {
            Playlist playlistToUpdate = playlistDto.ToEntity(_userRepository, _songRepository, _mapper);
            PlaylistDto updatedPlaylistDto = _playlistRepository.UpdateById(playlistToUpdate, id).ToDto(_mapper);
            return updatedPlaylistDto;
        }

        public PlaylistDto DeleteById(Guid id)
        {
            PlaylistDto deletedPlaylistDto = _playlistRepository.DeleteById(id).ToDto(_mapper);
            return deletedPlaylistDto;
        }
    }
}