using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.SongMock;

namespace SoundSphere.Tests.Unit.Repositories
{
    public class SongRepositoryTest
    {
        private readonly Mock<DbSet<Song>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly ISongRepository _songRepository;

        private readonly Song _song1 = GetMockedSong1();
        private readonly Song _song2 = GetMockedSong2();
        private readonly IList<Song> _songs = GetMockedSongs();
        private readonly IList<Song> _paginatedSongs = GetMockedPaginatedSongs();
        private readonly SongPaginationRequest _paginationRequest = GetMockedSongsPaginationRequest();

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

        [Fact] public void GetAll_Test() => _songRepository.GetAll(_paginationRequest).Should().BeEquivalentTo(_paginatedSongs);

        [Fact] public void GetById_ValidId_Test() => _songRepository.GetById(ValidSongGuid).Should().Be(_song1);

        [Fact] public void GetById_InvalidId_Test() => _songRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(SongNotFound, InvalidGuid));

        [Fact] public void Add_Test()
        {
            _songRepository.Add(_song1).Should().Be(_song1);
            _dbSetMock.Verify(mock => mock.Add(It.IsAny<Song>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Song updatedSong = GetSong(_song2, true);
            _songRepository.UpdateById(_song2, ValidSongGuid).Should().Be(updatedSong);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _songRepository
            .Invoking(repository => repository.UpdateById(_song2, InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(SongNotFound, InvalidGuid));

        [Fact] public void DeleteById_ValidId_Test()
        {
            Song deletedSong = GetSong(_song1, false);
            _songRepository.DeleteById(ValidSongGuid).Should().Be(deletedSong);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DeleteById_InvalidId_Test() => _songRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(SongNotFound, InvalidGuid));

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
            SimilarSongs = song.SimilarSongs
        };
    }
}