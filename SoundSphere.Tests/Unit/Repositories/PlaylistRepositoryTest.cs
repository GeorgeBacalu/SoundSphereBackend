using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Repositories
{
    public class PlaylistRepositoryTest
    {
        private readonly Mock<DbSet<Playlist>> _setMock = new();
        private readonly Mock<SoundSphereContext> _contextMock = new();
        private readonly IPlaylistRepository _playlistRepository;

        private readonly Playlist _playlist1 = PlaylistMock.GetMockedPlaylist1();
        private readonly Playlist _playlist2 = PlaylistMock.GetMockedPlaylist2();
        private readonly IList<Playlist> _playlists = PlaylistMock.GetMockedPlaylists();

        public PlaylistRepositoryTest()
        {
            var queryablePlaylists = _playlists.AsQueryable();
            _setMock.As<IQueryable<Playlist>>().Setup(mock => mock.Provider).Returns(queryablePlaylists.Provider);
            _setMock.As<IQueryable<Playlist>>().Setup(mock => mock.Expression).Returns(queryablePlaylists.Expression);
            _setMock.As<IQueryable<Playlist>>().Setup(mock => mock.ElementType).Returns(queryablePlaylists.ElementType);
            _setMock.As<IQueryable<Playlist>>().Setup(mock => mock.GetEnumerator()).Returns(queryablePlaylists.GetEnumerator());
            _contextMock.Setup(mock => mock.Playlists).Returns(_setMock.Object);
            _playlistRepository = new PlaylistRepository(_contextMock.Object);
        }

        [Fact] public void FindAll_Test() => _playlistRepository.FindAll().Should().BeEquivalentTo(_playlists);

        [Fact] public void FindById_ValidId_Test() => _playlistRepository.FindById(Constants.ValidPlaylistGuid).Should().BeEquivalentTo(_playlist1);

        [Fact] public void FindById_InvalidId_Test() =>
            _playlistRepository.Invoking(repository => repository.FindById(Constants.InvalidGuid))
                               .Should().Throw<ResourceNotFoundException>()
                               .WithMessage($"Playlist with id {Constants.InvalidGuid} not found!");

        [Fact] public void Save_Test()
        {
            _playlistRepository.Save(_playlist1).Should().BeEquivalentTo(_playlist1);
            _setMock.Verify(mock => mock.Add(It.IsAny<Playlist>()));
            _contextMock.Verify(mock => mock.SaveChanges());
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
            _playlistRepository.UpdateById(_playlist2, Constants.ValidPlaylistGuid).Should().BeEquivalentTo(updatedPlaylist);
            _contextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() =>
            _playlistRepository.Invoking(repository => repository.UpdateById(_playlist2, Constants.InvalidGuid))
                               .Should().Throw<ResourceNotFoundException>()
                               .WithMessage($"Playlist with id {Constants.InvalidGuid} not found!");

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
            _playlistRepository.DisableById(Constants.ValidPlaylistGuid).Should().BeEquivalentTo(disabledPlaylist);
            _contextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DisableById_InvalidId_Test() =>
            _playlistRepository.Invoking(repository => repository.DisableById(Constants.InvalidGuid))
                               .Should().Throw<ResourceNotFoundException>()
                               .WithMessage($"Playlist with id {Constants.InvalidGuid} not found!");
    }
}