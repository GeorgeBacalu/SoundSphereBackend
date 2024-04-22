using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Repositories
{
    public class SongRepositoryTest
    {
        private readonly Mock<DbSet<Song>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly ISongRepository _songRepository;

        private readonly Song _song1 = SongMock.GetMockedSong1();
        private readonly Song _song2 = SongMock.GetMockedSong2();
        private readonly IList<Song> _songs = SongMock.GetMockedSongs();
        private readonly IList<Song> _activeSongs = SongMock.GetMockedActiveSongs();

        public SongRepositoryTest()
        {
            IQueryable<Song> queryableSongs = _songs.AsQueryable();
            _dbSetMock.As<IQueryable<Song>>().Setup(mock => mock.Provider).Returns(queryableSongs.Provider);
            _dbSetMock.As<IQueryable<Song>>().Setup(mock => mock.Expression).Returns(queryableSongs.Expression);
            _dbSetMock.As<IQueryable<Song>>().Setup(mock => mock.ElementType).Returns(queryableSongs.ElementType);
            _dbSetMock.As<IQueryable<Song>>().Setup(mock => mock.GetEnumerator()).Returns(queryableSongs.GetEnumerator());
            _dbContextMock.Setup(mock => mock.Songs).Returns(_dbSetMock.Object);
            _songRepository = new SongRepository(_dbContextMock.Object);
        }

        [Fact] public void FindAll_Test() => _songRepository.FindAll().Should().BeEquivalentTo(_songs);

        [Fact] public void FindAllActive_Test() => _songRepository.FindAllActive().Should().BeEquivalentTo(_activeSongs);

        [Fact] public void FindById_ValidId_Test() => _songRepository.FindById(Constants.ValidSongGuid).Should().Be(_song1);

        [Fact] public void FindById_InvalidId_Test() => _songRepository
            .Invoking(repository => repository.FindById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.SongNotFound, Constants.InvalidGuid));

        [Fact] public void Save_Test()
        {
            _songRepository.Save(_song1).Should().Be(_song1);
            _dbSetMock.Verify(mock => mock.Add(It.IsAny<Song>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Song updatedSong = CreateTestSong(_song2, _song1.IsActive);
            _songRepository.UpdateById(_song2, Constants.ValidSongGuid).Should().Be(updatedSong);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _songRepository
            .Invoking(repository => repository.UpdateById(_song2, Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.SongNotFound, Constants.InvalidGuid));

        [Fact] public void DisableById_ValidId_Test()
        {
            Song disabledSong = CreateTestSong(_song1, false);
            _songRepository.DisableById(Constants.ValidSongGuid).Should().Be(disabledSong);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DisableById_InvalidId_Test() => _songRepository
            .Invoking(repository => repository.DisableById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.SongNotFound, Constants.InvalidGuid));

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