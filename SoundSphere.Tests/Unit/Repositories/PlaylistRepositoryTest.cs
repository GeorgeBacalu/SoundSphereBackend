using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Repositories
{
    public class PlaylistRepositoryTest
    {
        private readonly Mock<DbSet<Playlist>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IPlaylistRepository _playlistRepository;

        private readonly Playlist _playlist1 = PlaylistMock.GetMockedPlaylist1();
        private readonly Playlist _playlist2 = PlaylistMock.GetMockedPlaylist2();
        private readonly IList<Playlist> _playlists = PlaylistMock.GetMockedPlaylists();
        private readonly IList<Playlist> _activePlaylists = PlaylistMock.GetMockedActivePlaylists();
        private readonly IList<Playlist> _paginatedPlaylists = PlaylistMock.GetMockedPaginatedPlaylists();
        private readonly IList<Playlist> _activePaginatedPlaylists = PlaylistMock.GetMockedActivePaginatedPlaylists();
        private readonly PlaylistPaginationRequest _paginationRequest = PlaylistMock.GetMockedPaginationRequest();

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

        [Fact] public void FindAll_Test() => _playlistRepository.FindAll().Should().BeEquivalentTo(_playlists);

        [Fact] public void FindAllActive_Test() => _playlistRepository.FindAllActive().Should().BeEquivalentTo(_activePlaylists);

        [Fact] public void FindAllPagination_Test() => _playlistRepository.FindAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedPlaylists);

        [Fact] public void FindAllActivePagination_Test() => _playlistRepository.FindAllActivePagination(_paginationRequest).Should().BeEquivalentTo(_activePaginatedPlaylists);

        [Fact] public void FindById_ValidId_Test() => _playlistRepository.FindById(Constants.ValidPlaylistGuid).Should().Be(_playlist1);

        [Fact] public void FindById_InvalidId_Test() => _playlistRepository
            .Invoking(repository => repository.FindById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.PlaylistNotFound, Constants.InvalidGuid));

        [Fact] public void Save_Test()
        {
            _playlistRepository.Save(_playlist1).Should().Be(_playlist1);
            _dbSetMock.Verify(mock => mock.Add(It.IsAny<Playlist>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Playlist updatedPlaylist = new Playlist
            {
                Id = Constants.ValidPlaylistGuid,
                Title = _playlist2.Title,
                User = _playlist1.User,
                Songs = _playlist1.Songs,
                CreatedAt = _playlist1.CreatedAt,
                IsActive = _playlist1.IsActive
            };
            _playlistRepository.UpdateById(_playlist2, Constants.ValidPlaylistGuid).Should().Be(updatedPlaylist);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _playlistRepository
            .Invoking(repository => repository.UpdateById(_playlist2, Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.PlaylistNotFound, Constants.InvalidGuid));

        [Fact] public void DisableById_ValidId_Test()
        {
            Playlist disabledPlaylist = new Playlist
            {
                Id = Constants.ValidPlaylistGuid,
                Title = _playlist1.Title,
                User = _playlist1.User,
                Songs = _playlist1.Songs,
                CreatedAt = _playlist1.CreatedAt,
                IsActive = false
            };
            _playlistRepository.DisableById(Constants.ValidPlaylistGuid).Should().Be(disabledPlaylist);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DisableById_InvalidId_Test() => _playlistRepository
            .Invoking(repository => repository.DisableById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.PlaylistNotFound, Constants.InvalidGuid));
    }
}