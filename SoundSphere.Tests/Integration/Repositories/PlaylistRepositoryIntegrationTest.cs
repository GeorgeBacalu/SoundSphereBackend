using FluentAssertions;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class PlaylistRepositoryIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        private readonly Playlist _playlist1 = PlaylistMock.GetMockedPlaylist1();
        private readonly Playlist _playlist2 = PlaylistMock.GetMockedPlaylist2();
        private readonly IList<Playlist> _playlists = PlaylistMock.GetMockedPlaylists();
        private readonly IList<Playlist> _activePlaylists = PlaylistMock.GetMockedActivePlaylists();

        public PlaylistRepositoryIntegrationTest(DbFixture fixture) => _fixture = fixture;

        private void Execute(Action<PlaylistRepository, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var playlistRepository = new PlaylistRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Playlists.AddRange(_playlists);
            context.SaveChanges();
            action(playlistRepository, context);
        }

        [Fact] public void FindAll_Test() => Execute((playlistRepository, context) => playlistRepository.FindAll().Should().BeEquivalentTo(_playlists));

        [Fact] public void FindAllActive_Test() => Execute((playlistRepository, context) => playlistRepository.FindAllActive().Should().BeEquivalentTo(_activePlaylists));

        [Fact] public void FindById_ValidId_Test() => Execute((playlistRepository, context) => playlistRepository.FindById(Constants.ValidPlaylistGuid).Should().Be(_playlist1));

        [Fact] public void FindById_InvalidId_Test() => Execute((playlistRepository, context) => playlistRepository
            .Invoking(repository => repository.FindById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.PlaylistNotFound, Constants.InvalidGuid)));

        [Fact] public void Save_Test() => Execute((playlistRepository, context) =>
        {
            Playlist newPlaylist = PlaylistMock.GetMockedPlaylist3();
            playlistRepository.Save(newPlaylist);
            context.Playlists.Find(newPlaylist.Id).Should().BeEquivalentTo(newPlaylist, options => options.Excluding(playlist => playlist.CreatedAt));
        });

        [Fact] public void UpdateById_ValidId_Test() => Execute((playlistRepository, context) =>
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
            playlistRepository.UpdateById(_playlist2, Constants.ValidPlaylistGuid);
            context.Playlists.Find(Constants.ValidPlaylistGuid).Should().Be(updatedPlaylist);
        });

        [Fact] public void UpdateById_InvalidId_Test() => Execute((playlistRepository, context) => playlistRepository
            .Invoking(repository => repository.UpdateById(_playlist2, Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.PlaylistNotFound, Constants.InvalidGuid)));

        [Fact] public void DisableById_ValidId_Test() => Execute((playlistRepository, context) =>
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
            playlistRepository.DisableById(Constants.ValidPlaylistGuid);
            context.Playlists.Find(Constants.ValidPlaylistGuid).Should().Be(disabledPlaylist);
        });

        [Fact] public void DisableById_InvalidId_Test() => Execute((playlistRepository, context) => playlistRepository
            .Invoking(repository => repository.DisableById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.PlaylistNotFound, Constants.InvalidGuid)));
    }
}