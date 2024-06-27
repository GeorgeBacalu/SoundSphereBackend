using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.PlaylistMock;

namespace SoundSphere.Tests.Integration.Services
{
    public class PlaylistServiceIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly IMapper _mapper;

        private readonly Playlist _playlist1 = GetMockedPlaylist1();
        private readonly Playlist _playlist2 = GetMockedPlaylist2();
        private readonly IList<Playlist> _playlists = GetMockedPlaylists();
        private readonly PlaylistDto _playlistDto1 = GetMockedPlaylistDto1();
        private readonly PlaylistDto _playlistDto2 = GetMockedPlaylistDto2();
        private readonly IList<PlaylistDto> _playlistDtos = GetMockedPlaylistDtos();
        private readonly IList<PlaylistDto> _paginatedPlaylistDtos = GetMockedPaginatedPlaylistDtos();
        private readonly PlaylistPaginationRequest _paginationRequest = GetMockedPlaylistsPaginationRequest();

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

        [Fact] public void GetAll_Test() => Execute((playlistService, context) => playlistService.GetAll(_paginationRequest, ValidUserGuid).Should().BeEquivalentTo(_paginatedPlaylistDtos));
        
        [Fact] public void GetById_Test() => Execute((playlistService, context) => playlistService.GetById(ValidPlaylistGuid, ValidUserGuid).Should().Be(_playlistDto1));

        [Fact] public void Add_Test() => Execute((playlistService, context) =>
        {
            PlaylistDto newPlaylistDto = GetMockedPlaylistDto24();
            PlaylistDto result = playlistService.Add(newPlaylistDto, ValidUserGuid);
            context.Playlists.Find(newPlaylistDto.Id).Should().BeEquivalentTo(newPlaylistDto, options => options.Excluding(playlist => playlist.CreatedAt));
            result.Should().Be(newPlaylistDto);
        });

        [Fact] public void UpdateById_Test() => Execute((playlistService, context) =>
        {
            Playlist updatedPlaylist = new Playlist
            {
                Id = ValidPlaylistGuid,
                Title = _playlist2.Title,
                User = _playlist1.User,
                Songs = _playlist1.Songs,
                CreatedAt = _playlist1.CreatedAt
            };
            PlaylistDto updatedPlaylistDto = updatedPlaylist.ToDto(_mapper);
            PlaylistDto result = playlistService.UpdateById(_playlistDto2, ValidPlaylistGuid, ValidUserGuid2);
            context.Playlists.Find(ValidPlaylistGuid).Should().Be(updatedPlaylist);
            result.Should().Be(updatedPlaylistDto);
        });

        [Fact] public void DeleteById_Test() => Execute((playlistService, context) =>
        {
            Playlist deletePlaylist = new Playlist
            {
                Id = ValidPlaylistGuid,
                Title = _playlist1.Title,
                User = _playlist1.User,
                Songs = _playlist1.Songs,
                CreatedAt = _playlist1.CreatedAt
            };
            PlaylistDto deletedPlaylistDto = deletePlaylist.ToDto(_mapper);
            PlaylistDto result = playlistService.DeleteById(ValidPlaylistGuid, ValidUserGuid);
            context.Playlists.Find(ValidPlaylistGuid).Should().Be(deletedPlaylistDto);
            result.Should().Be(deletedPlaylistDto);
        });
    }
}