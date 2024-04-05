using FluentAssertions;
using Newtonsoft.Json.Bson;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Context;
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

        public SongRepositoryIntegrationTest(DbFixture fixture) => _fixture = fixture;

        private void Execute(Action<SongRepository, SoundSphereContext> action)
        {
            using var context = _fixture.CreateContext();
            var songRepository = new SongRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Songs.AddRange(_songs);
            context.SaveChanges();
            action(songRepository, context);
        }

        [Fact] public void FindAll_Test() => Execute((songRepository, context) => songRepository.FindAll().Should().BeEquivalentTo(_songs));

        [Fact] public void FindAllActive_Test() => Execute((songRepository, context) => songRepository.FindAllActive().Should().BeEquivalentTo(_activeSongs));

        [Fact] public void FindById_ValidId_Test() => Execute((songRepository, context) => songRepository.FindById(Constants.ValidSongGuid).Should().BeEquivalentTo(_song1));

        [Fact] public void FindById_InvalidId_Test() => Execute((songRepository, context) => 
            songRepository.Invoking(repository => repository.FindById(Constants.InvalidGuid))
                          .Should().Throw<ResourceNotFoundException>()
                          .WithMessage($"Song with id {Constants.InvalidGuid} not found!"));

        [Fact] public void Save_Test() => Execute((songRepository, context) =>
        {
            Song newSong = SongMock.GetMockedSong5();
            songRepository.Save(newSong);
            context.Songs.Find(newSong.Id).Should().BeEquivalentTo(newSong);
        });

        [Fact] public void UpdateById_ValidId_Test() => Execute((songRepository, context) =>
        {
            Song updatedSong = CreateTestSong(_song2, _song1.IsActive);
            songRepository.UpdateById(_song2, Constants.ValidSongGuid);
            context.Songs.Find(Constants.ValidSongGuid).Should().BeEquivalentTo(updatedSong);
        });

        [Fact] public void UpdateById_InvalidId_Test() => Execute((songRepository, context) =>
            songRepository.Invoking(repository => repository.UpdateById(_song2, Constants.InvalidGuid))
                          .Should().Throw<ResourceNotFoundException>()
                          .WithMessage($"Song with id {Constants.InvalidGuid} not found!"));

        [Fact] public void DisableById_ValidId_Test() => Execute((songRepository, context) =>
        {
            Song disabledSong = CreateTestSong(_song1, false);
            songRepository.DisableById(Constants.ValidSongGuid);
            context.Songs.Find(Constants.ValidSongGuid).Should().BeEquivalentTo(disabledSong);
        });

        [Fact] public void DisableById_InvalidId_Test() => Execute((songRepository, context) =>
            songRepository.Invoking(repository => repository.DisableById(Constants.InvalidGuid))
                          .Should().Throw<ResourceNotFoundException>()
                          .WithMessage($"Song with id {Constants.InvalidGuid} not found!"));

        private Song CreateTestSong(Song song, bool isActive) => new Song
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