using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.PlaylistMock;

namespace SoundSphere.Tests.Unit.Repositories
{
    public class PlaylistRepositoryTest
    {
        private readonly Mock<DbSet<Playlist>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IPlaylistRepository _playlistRepository;

        private readonly Playlist _playlist1 = GetMockedPlaylist1();
        private readonly Playlist _playlist2 = GetMockedPlaylist2();
        private readonly IList<Playlist> _playlists = GetMockedPlaylists();
        private readonly IList<Playlist> _paginatedPlaylists = GetMockedPaginatedPlaylists();
        private readonly PlaylistPaginationRequest _paginationRequest = GetMockedPlaylistsPaginationRequest();

        public PlaylistRepositoryTest()
        {
            IQueryable<Playlist> queryablePlaylists = _playlists.AsQueryable();
            _dbSetMock.As<IQueryable<Playlist>>().Setup(mock => mock.Provider).Returns(queryablePlaylists.Provider);
            _dbSetMock.As<IQueryable<Playlist>>().Setup(mock => mock.Expression).Returns(queryablePlaylists.Expression);
            _dbSetMock.As<IQueryable<Playlist>>().Setup(mock => mock.ElementType).Returns(queryablePlaylists.ElementType);
            _dbSetMock.As<IQueryable<Playlist>>().Setup(mock => mock.GetEnumerator()).Returns(queryablePlaylists.GetEnumerator());
            _dbContextMock.Setup(mock => mock.Playlists).Returns(_dbSetMock.Object);
            _playlistRepository = new PlaylistRepository(_dbContextMock.Object);
        }

        [Fact] public void GetAll_Test() => _playlistRepository.GetAll(_paginationRequest).Should().BeEquivalentTo(_paginatedPlaylists);

        [Fact] public void GetById_ValidId_Test() => _playlistRepository.GetById(ValidPlaylistGuid).Should().Be(_playlist1);

        [Fact] public void GetById_InvalidId_Test() => _playlistRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(PlaylistNotFound, InvalidGuid));

        [Fact] public void Add_Test()
        {
            _playlistRepository.Add(_playlist1).Should().Be(_playlist1);
            _dbSetMock.Verify(mock => mock.Add(It.IsAny<Playlist>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Playlist updatedPlaylist = new Playlist
            {
                Id = ValidPlaylistGuid,
                Title = _playlist2.Title,
                User = _playlist1.User,
                Songs = _playlist1.Songs,
                CreatedAt = _playlist1.CreatedAt
            };
            _playlistRepository.UpdateById(_playlist2, ValidPlaylistGuid).Should().Be(updatedPlaylist);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _playlistRepository
            .Invoking(repository => repository.UpdateById(_playlist2, InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(PlaylistNotFound, InvalidGuid));

        [Fact] public void DeleteById_ValidId_Test()
        {
            Playlist deletedPlaylist = new Playlist
            {
                Id = ValidPlaylistGuid,
                Title = _playlist1.Title,
                User = _playlist1.User,
                Songs = _playlist1.Songs,
                CreatedAt = _playlist1.CreatedAt,
            };
            _playlistRepository.DeleteById(ValidPlaylistGuid).Should().Be(deletedPlaylist);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DeleteById_InvalidId_Test() => _playlistRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(PlaylistNotFound, InvalidGuid));
    }
}