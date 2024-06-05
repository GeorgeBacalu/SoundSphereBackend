using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.SongMock;

namespace SoundSphere.Tests.Integration.Services
{
    public class SongServiceIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly IMapper _mapper;

        private readonly Song _song1 = GetMockedSong1();
        private readonly Song _song2 = GetMockedSong2();
        private readonly IList<Song> _songs = GetMockedSongs();
        private readonly SongDto _songDto1 = GetMockedSongDto1();
        private readonly SongDto _songDto2 = GetMockedSongDto2();
        private readonly IList<SongDto> _songDtos = GetMockedSongDtos();
        private readonly IList<SongDto> _paginatedSongDtos = GetMockedPaginatedSongDtos();
        private readonly SongPaginationRequest _paginationRequest = GetMockedSongsPaginationRequest();

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

        [Fact] public void GetAll_Test() => Execute((songService, context) => songService.GetAll(_paginationRequest).Should().BeEquivalentTo(_paginatedSongDtos));
        
        [Fact] public void GetById_Test() => Execute((songService, context) => songService.GetById(ValidSongGuid).Should().Be(_songDto1));

        [Fact] public void Add_Test() => Execute((songService, context) =>
        {
            SongDto newSongDto = GetMockedSongDto90();
            SongDto result = songService.Add(newSongDto);
            context.Songs.Find(newSongDto.Id).Should().Be(newSongDto);
            result.Should().Be(newSongDto);
        });

        [Fact] public void UpdateById_Test() => Execute((songService, context) =>
        {
            Song updatedSong = GetSong(_song2, true);
            SongDto updatedSongDto = updatedSong.ToDto(_mapper);
            SongDto result = songService.UpdateById(_songDto2, ValidSongGuid);
            context.Songs.Find(ValidSongGuid).Should().Be(updatedSong);
            result.Should().Be(updatedSongDto);
        });

        [Fact] public void DeleteById_Test() => Execute((songService, context) =>
        {
            Song deletedSong = GetSong(_song1, false);
            SongDto deletedSongDto = deletedSong.ToDto(_mapper);
            SongDto result = songService.DeleteById(ValidSongGuid);
            context.Songs.Find(ValidSongGuid).Should().Be(deletedSong);
            result.Should().Be(deletedSongDto);
        });

        private Song GetSong(Song song, bool isActive) => new Song
        {
            Id = ValidSongGuid,
            Title = song.Title,
            ImageUrl = song.ImageUrl,
            Genre = song.Genre,
            ReleaseDate = song.ReleaseDate,
            DurationSeconds = song.DurationSeconds,
            Album = song.Album,
            Artists = song.Artists,
            SimilarSongs = song.SimilarSongs,
            CreatedAt = song.CreatedAt,
            UpdatedAt = song.UpdatedAt,
            DeletedAt = song.DeletedAt
        };
    }
}