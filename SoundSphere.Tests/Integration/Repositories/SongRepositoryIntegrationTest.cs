using FluentAssertions;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class SongRepositoryIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        private readonly Song _song1 = SongMock.GetMockedSong1();
        private readonly Song _song2 = SongMock.GetMockedSong2();
        private readonly IList<Song> _songs = SongMock.GetMockedSongs();
        private readonly IList<Song> _activeSongs = SongMock.GetMockedActiveSongs();
        private readonly IList<Song> _paginatedSongs = SongMock.GetMockedPaginatedSongs();
        private readonly IList<Song> _activePaginatedSongs = SongMock.GetMockedActivePaginatedSongs();
        private readonly SongPaginationRequest _paginationRequest = SongMock.GetMockedPaginationRequest();

        public SongRepositoryIntegrationTest(DbFixture fixture) => _fixture = fixture;

        private void Execute(Action<SongRepository, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var songRepository = new SongRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Songs.AddRange(_songs);
            context.SaveChanges();
            action(songRepository, context);
            transaction.Rollback();
        }

        [Fact] public void FindAll_Test() => Execute((songRepository, context) => songRepository.FindAll().Should().BeEquivalentTo(_songs));

        [Fact] public void FindAllActive_Test() => Execute((songRepository, context) => songRepository.FindAllActive().Should().BeEquivalentTo(_activeSongs));

        [Fact] public void FindAllPagination_Test() => Execute((songRepository, context) => songRepository.FindAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedSongs));

        [Fact] public void FindAllActivePagination_Test() => Execute((songRepository, context) => songRepository.FindAllActivePagination(_paginationRequest).Should().BeEquivalentTo(_activePaginatedSongs));
        
        [Fact] public void FindById_ValidId_Test() => Execute((songRepository, context) => songRepository.FindById(Constants.ValidSongGuid).Should().Be(_song1));

        [Fact] public void FindById_InvalidId_Test() => Execute((songRepository, context) => songRepository
            .Invoking(repository => repository.FindById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.SongNotFound, Constants.InvalidGuid)));

        [Fact] public void Save_Test() => Execute((songRepository, context) =>
        {
            Song newSong = SongMock.GetMockedSong90();
            songRepository.Save(newSong);
            context.Songs.Find(newSong.Id).Should().Be(newSong);
        });

        [Fact] public void UpdateById_ValidId_Test() => Execute((songRepository, context) =>
        {
            Song updatedSong = GetSong(_song2, _song1.IsActive);
            songRepository.UpdateById(_song2, Constants.ValidSongGuid);
            context.Songs.Find(Constants.ValidSongGuid).Should().Be(updatedSong);
        });

        [Fact] public void UpdateById_InvalidId_Test() => Execute((songRepository, context) => songRepository
            .Invoking(repository => repository.UpdateById(_song2, Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.SongNotFound, Constants.InvalidGuid)));

        [Fact] public void DisableById_ValidId_Test() => Execute((songRepository, context) =>
        {
            Song disabledSong = GetSong(_song1, false);
            songRepository.DisableById(Constants.ValidSongGuid);
            context.Songs.Find(Constants.ValidSongGuid).Should().Be(disabledSong);
        });

        [Fact] public void DisableById_InvalidId_Test() => Execute((songRepository, context) => songRepository
            .Invoking(repository => repository.DisableById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.SongNotFound, Constants.InvalidGuid)));

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