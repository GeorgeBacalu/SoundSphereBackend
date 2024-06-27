using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;

namespace SoundSphere.Core.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISongRepository _songRepository;
        private readonly SoundSphereDbContext _context;
        private readonly IMapper _mapper;

        public PlaylistService(IPlaylistRepository playlistRepository, IUserRepository userRepository, ISongRepository songRepository, SoundSphereDbContext context, IMapper mapper) => 
            (_playlistRepository, _userRepository, _songRepository, _context, _mapper) = (playlistRepository, userRepository, songRepository, context, mapper);

        public IList<PlaylistDto> GetAll(PlaylistPaginationRequest? payload, Guid userId)
        {
            IList<PlaylistDto> playlistDtos = _playlistRepository.GetAll(payload, userId).ToDtos(_mapper);
            return playlistDtos;
        }

        public PlaylistDto GetById(Guid playlistId, Guid userId)
        {
            Playlist playlist = _playlistRepository.GetById(playlistId);
            if (!playlist.User.Id.Equals(userId))
                throw new ForbiddenAccessException(AccessPlaylistDenied);
            PlaylistDto playlistDto = playlist.ToDto(_mapper);
            return playlistDto;
        }

        public PlaylistDto Add(PlaylistDto playlistDto, Guid userId)
        {
            playlistDto.UserId = userId;
            Playlist playlistToCreate = playlistDto.ToEntity(_userRepository, _songRepository, _mapper);
            _playlistRepository.LinkPlaylistToUser(playlistToCreate);
            PlaylistDto createdPlaylistDto = _playlistRepository.Add(playlistToCreate).ToDto(_mapper);
            return createdPlaylistDto;
        }

        public PlaylistDto UpdateById(PlaylistDto playlistDto, Guid playlistId, Guid userId)
        {
            Playlist playlist = _playlistRepository.GetById(playlistId);
            if (!playlist.User.Id.Equals(userId))
                throw new ForbiddenAccessException(UpdatePlaylistDenied);
            playlistDto.UserId = userId;
            Playlist playlistToUpdate = playlistDto.ToEntity(_userRepository, _songRepository, _mapper);
            PlaylistDto updatedPlaylistDto = _playlistRepository.UpdateById(playlistToUpdate, playlistId).ToDto(_mapper);
            return updatedPlaylistDto;
        }

        public PlaylistDto DeleteById(Guid playlistId, Guid userId)
        {
            Playlist playlist = _playlistRepository.GetById(playlistId);
            if (!playlist.User.Id.Equals(userId))
                throw new ForbiddenAccessException(DeletePlaylistDenied);
            PlaylistDto deletedPlaylistDto = _playlistRepository.DeleteById(playlistId).ToDto(_mapper);
            return deletedPlaylistDto;
        }

        public PlaylistDto AddSong(Guid playlistId, Guid songId, Guid userId)
        {
            Playlist playlist = _playlistRepository.GetById(playlistId);
            if (!playlist.User.Id.Equals(userId))
                throw new ForbiddenAccessException(AddSongToPlaylistDenied);
            if (playlist.Songs.Any(song => song.Id.Equals(songId)))
                throw new InvalidRequestException(string.Format(SongAlreadyInPlaylist, songId, playlistId));
            Song song = _songRepository.GetById(songId);
            playlist.Songs.Add(song);
            PlaylistDto updatedPlaylistDto = _playlistRepository.UpdateById(playlist, playlistId).ToDto(_mapper);
            return updatedPlaylistDto;
        }

        public PlaylistDto RemoveSong(Guid playlistId, Guid songId, Guid userId)
        {
            Playlist playlist = _playlistRepository.GetById(playlistId);
            if (!playlist.User.Id.Equals(userId))
                throw new ForbiddenAccessException(RemoveSongFromPlaylistDenied);
            if (!playlist.Songs.Any(song => song.Id.Equals(songId)))
                throw new InvalidRequestException(string.Format(SongNotInPlaylist, songId, playlistId));
            Song song = _songRepository.GetById(songId);
            playlist.Songs.Remove(song);
            PlaylistDto updatedPlaylistDto = _playlistRepository.UpdateById(playlist, playlistId).ToDto(_mapper);
            return updatedPlaylistDto;
        }
    }
}