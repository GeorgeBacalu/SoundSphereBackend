using FluentAssertions;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.PlaylistMock;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class PlaylistRepositoryIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        private readonly Playlist _playlist1 = GetMockedPlaylist1();
        private readonly Playlist _playlist2 = GetMockedPlaylist2();
        private readonly IList<Playlist> _playlists = GetMockedPlaylists();
        private readonly IList<Playlist> _activePlaylists = GetMockedActivePlaylists();
        private readonly IList<Playlist> _paginatedPlaylists = GetMockedPaginatedPlaylists();
        private readonly IList<Playlist> _activePaginatedPlaylists = GetMockedActivePaginatedPlaylists();
        private readonly PlaylistPaginationRequest _paginationRequest = GetMockedPlaylistsPaginationRequest();

        public PlaylistRepositoryIntegrationTest(DbFixture fixture) => _fixture = fixture;

        private void Execute(Action<PlaylistRepository, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var playlistRepository = new PlaylistRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Playlists.AddRange(_playlists);
            context.SaveChanges();
            action(playlistRepository, context);
            transaction.Rollback();
        }

        [Fact] public void GetAll_Test() => Execute((playlistRepository, context) => playlistRepository.GetAll().Should().BeEquivalentTo(_playlists));

        [Fact] public void GetAllActive_Test() => Execute((playlistRepository, context) => playlistRepository.GetAllActive().Should().BeEquivalentTo(_activePlaylists));

        [Fact] public void GetAllPagination_Test() => Execute((playlistRepository, context) => playlistRepository.GetAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedPlaylists));

        [Fact] public void GetAllActivePagination_Test() => Execute((playlistRepository, context) => playlistRepository.GetAllActivePagination(_paginationRequest).Should().BeEquivalentTo(_activePaginatedPlaylists));
        
        [Fact] public void GetById_ValidId_Test() => Execute((playlistRepository, context) => playlistRepository.GetById(ValidPlaylistGuid).Should().Be(_playlist1));

        [Fact] public void GetById_InvalidId_Test() => Execute((playlistRepository, context) => playlistRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(PlaylistNotFound, InvalidGuid)));

        [Fact] public void Add_Test() => Execute((playlistRepository, context) =>
        {
            Playlist newPlaylist = GetMockedPlaylist24();
            playlistRepository.Add(newPlaylist);
            context.Playlists.Find(newPlaylist.Id).Should().BeEquivalentTo(newPlaylist, options => options.Excluding(playlist => playlist.CreatedAt));
        });

        [Fact] public void UpdateById_ValidId_Test() => Execute((playlistRepository, context) =>
        {
            Playlist updatedPlaylist = new Playlist
            {
                Id = ValidPlaylistGuid,
                Title = _playlist2.Title,
                User = _playlist1.User,
                Songs = _playlist1.Songs,
                CreatedAt = _playlist1.CreatedAt,
                IsActive = _playlist1.IsActive
            };
            playlistRepository.UpdateById(_playlist2, ValidPlaylistGuid);
            context.Playlists.Find(ValidPlaylistGuid).Should().Be(updatedPlaylist);
        });

        [Fact] public void UpdateById_InvalidId_Test() => Execute((playlistRepository, context) => playlistRepository
            .Invoking(repository => repository.UpdateById(_playlist2, InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(PlaylistNotFound, InvalidGuid)));

        [Fact] public void DeleteById_ValidId_Test() => Execute((playlistRepository, context) =>
        {
            Playlist deletedPlaylist = new Playlist
            {
                Id = ValidPlaylistGuid,
                Title = _playlist1.Title,
                User = _playlist1.User,
                Songs = _playlist1.Songs,
                CreatedAt = _playlist1.CreatedAt,
                IsActive = false
            };
            playlistRepository.DeleteById(ValidPlaylistGuid);
            context.Playlists.Find(ValidPlaylistGuid).Should().Be(deletedPlaylist);
        });

        [Fact] public void DeleteById_InvalidId_Test() => Execute((playlistRepository, context) => playlistRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(PlaylistNotFound, InvalidGuid)));
    }
}