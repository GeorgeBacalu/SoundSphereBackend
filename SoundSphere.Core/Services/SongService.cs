using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Dtos.Response;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;

namespace SoundSphere.Core.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly SoundSphereDbContext _context;
        private readonly IMapper _mapper;

        public SongService(ISongRepository songRepository, IAlbumRepository albumRepository, IArtistRepository artistRepository, SoundSphereDbContext context, IMapper mapper) => 
            (_songRepository, _albumRepository, _artistRepository, _context, _mapper) = (songRepository, albumRepository, artistRepository, context, mapper);

        public IList<SongDto> GetAll(SongPaginationRequest? payload)
        {
            IList<SongDto> songDtos = _songRepository.GetAll(payload).ToDtos(_mapper);
            return songDtos;
        }

        public SongDto GetById(Guid  id)
        {
            SongDto songDto = _songRepository.GetById(id).ToDto(_mapper);
            return songDto;
        }

        public SongDto Add(SongDto songDto)
        {
            Song songToCreate = songDto.ToEntity(_albumRepository, _artistRepository, _mapper);
            _songRepository.LinkSongToAlbum(songToCreate);
            _songRepository.LinkSongToArtists(songToCreate);
            _songRepository.AddSongLink(songToCreate);
            _songRepository.AddUserSong(songToCreate);
            SongDto createdSongDto = _songRepository.Add(songToCreate).ToDto(_mapper);
            return createdSongDto;
        }

        public SongDto UpdateById(SongDto songDto, Guid id)
        {
            Song songToUpdate = songDto.ToEntity(_albumRepository, _artistRepository, _mapper);
            SongDto updatedSongDto = _songRepository.UpdateById(songToUpdate, id).ToDto(_mapper);
            return updatedSongDto;
        }

        public SongDto DeleteById(Guid id)
        {
            SongDto deletedSongDto = _songRepository.DeleteById(id).ToDto(_mapper);
            return deletedSongDto;
        }

        public IList<SongDto> GetRecommendations(int nrRecommendations)
        {
            IList<SongDto> recommendationDtos = _context.Songs
                .Include(song => song.Album)
                .Include(song => song.Artists)
                .Include(song => song.SimilarSongs)
                .Where(song => song.DeletedAt == null)
                .OrderBy(song => Guid.NewGuid())
                .Take(Math.Max(0, nrRecommendations))
                .ToList()
                .ToDtos(_mapper);
            return recommendationDtos;
        }

        public SongStatisticsDto GetStatistics(DateTime? startDate, DateTime? endDate)
        {
            SongStatisticsDto statistics = new SongStatisticsDto
            (
                TotalSongs: _songRepository.CountByDateRangeAndGenre(startDate, endDate, null),
                NrPop: _songRepository.CountByDateRangeAndGenre(startDate, endDate, GenreType.Pop),
                NrRock: _songRepository.CountByDateRangeAndGenre(startDate, endDate, GenreType.Rock),
                NrRnb: _songRepository.CountByDateRangeAndGenre(startDate, endDate, GenreType.Rnb),
                NrHipHop: _songRepository.CountByDateRangeAndGenre(startDate, endDate, GenreType.HipHop),
                NrDance: _songRepository.CountByDateRangeAndGenre(startDate, endDate, GenreType.Dance),
                NrTechno: _songRepository.CountByDateRangeAndGenre(startDate, endDate, GenreType.Techno),
                NrLatino: _songRepository.CountByDateRangeAndGenre(startDate, endDate, GenreType.Latino),
                NrHindi: _songRepository.CountByDateRangeAndGenre(startDate, endDate, GenreType.Hindi),
                NrReggae: _songRepository.CountByDateRangeAndGenre(startDate, endDate, GenreType.Reggae),
                NrJazz: _songRepository.CountByDateRangeAndGenre(startDate, endDate, GenreType.Jazz),
                NrClassical: _songRepository.CountByDateRangeAndGenre(startDate, endDate, GenreType.Classical),
                NrCountry: _songRepository.CountByDateRangeAndGenre(startDate, endDate, GenreType.Country),
                NrElectronic: _songRepository.CountByDateRangeAndGenre(startDate, endDate, GenreType.Electronic)
            );
            return statistics;
        }

        public void Play(Guid songId, Guid userId)
        {
            Song song = _songRepository.GetById(songId);
            if (song == null)
                throw new ResourceNotFoundException(string.Format(SongNotFound, songId));
            UserSong userSong = _context.UserSongs.First(userSong => userSong.SongId.Equals(songId) && userSong.UserId.Equals(userId));
            userSong.PlayCount++;
            _context.SaveChanges();
        }

        public int GetPlayCount(Guid songId, Guid userId)
        {
            Song song = _songRepository.GetById(songId);
            if (song == null)
                throw new ResourceNotFoundException(string.Format(SongNotFound, songId));
            UserSong userSong = _context.UserSongs.First(userSong => userSong.SongId.Equals(songId) && userSong.UserId.Equals(userId));
            int playCount = userSong.PlayCount;
            return playCount;
        }
    }
}