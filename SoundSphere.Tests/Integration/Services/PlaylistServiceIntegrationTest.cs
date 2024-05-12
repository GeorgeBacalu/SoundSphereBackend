using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Integration.Services
{
    public class PlaylistServiceIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly IMapper _mapper;

        private readonly Playlist _playlist1 = PlaylistMock.GetMockedPlaylist1();
        private readonly Playlist _playlist2 = PlaylistMock.GetMockedPlaylist2();
        private readonly IList<Playlist> _playlists = PlaylistMock.GetMockedPlaylists();
        private readonly PlaylistDto _playlistDto1 = PlaylistMock.GetMockedPlaylistDto1();
        private readonly PlaylistDto _playlistDto2 = PlaylistMock.GetMockedPlaylistDto2();
        private readonly IList<PlaylistDto> _playlistDtos = PlaylistMock.GetMockedPlaylistDtos();
        private readonly IList<PlaylistDto> _activePlaylistDtos = PlaylistMock.GetMockedActivePlaylistDtos();
        private readonly IList<PlaylistDto> _paginatedPlaylistDtos = PlaylistMock.GetMockedPaginatedPlaylistDtos();
        private readonly IList<PlaylistDto> _activePaginatedPlaylistDtos = PlaylistMock.GetMockedActivePaginatedPlaylistDtos();
        private readonly PlaylistPaginationRequest _paginationRequest = PlaylistMock.GetMockedPaginationRequest();

        public PlaylistServiceIntegrationTest(DbFixture fixture) => (_fixture, _mapper) = (fixture, new MapperConfiguration(config => { config.CreateMap<Playlist, PlaylistDto>(); config.CreateMap<PlaylistDto, Playlist>(); }).CreateMapper());

        private void Execute(Action<PlaylistService, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var playlistService = new PlaylistService(new PlaylistRepository(context), new UserRepository(context), new SongRepository(context), _mapper);
            using var transaction = context.Database.BeginTransaction();
            context.AddRange(_playlists);
            context.SaveChanges();
            action(playlistService, context);
            transaction.Rollback();
        }

        [Fact] public void FindAll_Test() => Execute((playlistService, context) => playlistService.FindAll().Should().BeEquivalentTo(_playlistDtos));

        [Fact] public void FindAllActive_Test() => Execute((playlistService, context) => playlistService.FindAllActive().Should().BeEquivalentTo(_activePlaylistDtos));

        [Fact] public void FindAllPagination_Test() => Execute((playlistService, context) => playlistService.FindAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedPlaylistDtos));

        [Fact] public void FindAllActivePagination_Test() => Execute((playlistService, context) => playlistService.FindAllActivePagination(_paginationRequest).Should().BeEquivalentTo(_activePaginatedPlaylistDtos));
        
        [Fact] public void FindById_Test() => Execute((playlistService, context) => playlistService.FindById(Constants.ValidPlaylistGuid).Should().Be(_playlistDto1));

        [Fact] public void Save_Test() => Execute((playlistService, context) =>
        {
            PlaylistDto newPlaylistDto = PlaylistMock.GetMockedPlaylistDto24();
            playlistService.Save(newPlaylistDto);
            context.Playlists.Find(newPlaylistDto.Id).Should().BeEquivalentTo(newPlaylistDto, options => options.Excluding(playlist => playlist.CreatedAt));
        });

        [Fact] public void UpdateById_Test() => Execute((playlistService, context) =>
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
            PlaylistDto updatedPlaylistDto = updatedPlaylist.ToDto(_mapper);
            playlistService.UpdateById(_playlistDto2, Constants.ValidPlaylistGuid);
            context.Playlists.Find(Constants.ValidPlaylistGuid).Should().Be(updatedPlaylist);
        });

        [Fact] public void DisableById_Test() => Execute((playlistService, context) =>
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
            PlaylistDto disabledPlaylistDto = disabledPlaylist.ToDto(_mapper);
            playlistService.DisableById(Constants.ValidPlaylistGuid);
            context.Playlists.Find(Constants.ValidPlaylistGuid).Should().Be(disabledPlaylist);
        });
    }
}