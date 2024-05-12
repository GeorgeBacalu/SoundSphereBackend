﻿using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Integration.Services
{
    public class SongServiceIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly IMapper _mapper;

        private readonly Song _song1 = SongMock.GetMockedSong1();
        private readonly Song _song2 = SongMock.GetMockedSong2();
        private readonly IList<Song> _songs = SongMock.GetMockedSongs();
        private readonly SongDto _songDto1 = SongMock.GetMockedSongDto1();
        private readonly SongDto _songDto2 = SongMock.GetMockedSongDto2();
        private readonly IList<SongDto> _songDtos = SongMock.GetMockedSongDtos();
        private readonly IList<SongDto> _activeSongDtos = SongMock.GetMockedActiveSongDtos();
        private readonly IList<SongDto> _paginatedSongDtos = SongMock.GetMockedPaginatedSongDtos();
        private readonly IList<SongDto> _activePaginatedSongDtos = SongMock.GetMockedActivePaginatedSongDtos();
        private readonly SongPaginationRequest _paginationRequest = SongMock.GetMockedPaginationRequest();

        public SongServiceIntegrationTest(DbFixture fixture) => (_fixture, _mapper) = (fixture, new MapperConfiguration(config => { config.CreateMap<Song, SongDto>(); config.CreateMap<SongDto, Song>(); }).CreateMapper());

        private void Execute(Action<SongService, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var songService = new SongService(new SongRepository(context), new AlbumRepository(context), new ArtistRepository(context), _mapper);
            using var transaction = context.Database.BeginTransaction();
            context.AddRange(_songs);
            context.SaveChanges();
            action(songService, context);
            transaction.Rollback();
        }

        [Fact] public void FindAll_Test() => Execute((songService, context) => songService.FindAll().Should().BeEquivalentTo(_songDtos));

        [Fact] public void FindAllActive_Test() => Execute((songService, context) => songService.FindAllActive().Should().BeEquivalentTo(_activeSongDtos));

        [Fact] public void FindAllPagination_Test() => Execute((songService, context) => songService.FindAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedSongDtos));

        [Fact] public void FindAllActivePagination_Test() => Execute((songService, context) => songService.FindAllActivePagination(_paginationRequest).Should().BeEquivalentTo(_activePaginatedSongDtos));
        
        [Fact] public void FindById_Test() => Execute((songService, context) => songService.FindById(Constants.ValidSongGuid).Should().Be(_songDto1));

        [Fact] public void Save_Test() => Execute((songService, context) =>
        {
            SongDto newSongDto = SongMock.GetMockedSongDto90();
            songService.Save(newSongDto);
            context.Songs.Find(newSongDto.Id).Should().Be(newSongDto);
        });

        [Fact] public void UpdateById_Test() => Execute((songService, context) =>
        {
            Song updatedSong = GetSong(_song2, _song1.IsActive);
            SongDto updatedSongDto = updatedSong.ToDto(_mapper);
            songService.UpdateById(_songDto2, Constants.ValidSongGuid);
            context.Songs.Find(Constants.ValidSongGuid).Should().Be(updatedSong);
        });

        [Fact] public void DisableById_Test() => Execute((songService, context) =>
        {
            Song disabledSong = GetSong(_song1, false);
            SongDto disabledSongDto = disabledSong.ToDto(_mapper);
            songService.DisableById(Constants.ValidSongGuid);
            context.Songs.Find(Constants.ValidSongGuid).Should().Be(disabledSong);
        });

        private Song GetSong(Song song, bool isActive) => new Song
        {
            Id = Constants.ValidSongGuid,
            Title = song.Title,
            ImageUrl = song.ImageUrl,
            Genre = song.Genre,
            ReleaseDate = song.ReleaseDate,
            DurationSeconds = song.DurationSeconds,
            Album = song.Album,
            Artists = song.Artists,
            SimilarSongs = song.SimilarSongs,
            IsActive = isActive
        };
    }
}