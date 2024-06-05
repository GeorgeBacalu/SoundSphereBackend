using FluentAssertions;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.SongMock;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class SongRepositoryIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        private readonly Song _song1 = GetMockedSong1();
        private readonly Song _song2 = GetMockedSong2();
        private readonly IList<Song> _songs = GetMockedSongs();
        private readonly IList<Song> _paginatedSongs = GetMockedPaginatedSongs();
        private readonly SongPaginationRequest _paginationRequest = GetMockedSongsPaginationRequest();

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

        [Fact] public void GetAll_Test() => Execute((songRepository, context) => songRepository.GetAll(_paginationRequest).Should().BeEquivalentTo(_paginatedSongs));
        
        [Fact] public void GetById_ValidId_Test() => Execute((songRepository, context) => songRepository.GetById(ValidSongGuid).Should().Be(_song1));

        [Fact] public void GetById_InvalidId_Test() => Execute((songRepository, context) => songRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(SongNotFound, InvalidGuid)));

        [Fact] public void Add_Test() => Execute((songRepository, context) =>
        {
            Song newSong = GetMockedSong90();
            songRepository.Add(newSong);
            context.Songs.Find(newSong.Id).Should().Be(newSong);
        });

        [Fact] public void UpdateById_ValidId_Test() => Execute((songRepository, context) =>
        {
            Song updatedSong = GetSong(_song2, true);
            songRepository.UpdateById(_song2, ValidSongGuid);
            context.Songs.Find(ValidSongGuid).Should().Be(updatedSong);
        });

        [Fact] public void UpdateById_InvalidId_Test() => Execute((songRepository, context) => songRepository
            .Invoking(repository => repository.UpdateById(_song2, InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(SongNotFound, InvalidGuid)));

        [Fact] public void DeleteById_ValidId_Test() => Execute((songRepository, context) =>
        {
            Song deletedSong = GetSong(_song1, false);
            songRepository.DeleteById(ValidSongGuid);
            context.Songs.Find(ValidSongGuid).Should().Be(deletedSong);
        });

        [Fact] public void DeleteById_InvalidId_Test() => Execute((songRepository, context) => songRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(SongNotFound, InvalidGuid)));

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