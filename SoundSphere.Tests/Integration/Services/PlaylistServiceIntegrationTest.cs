using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Services;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos;
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

        public PlaylistServiceIntegrationTest(DbFixture fixture)
        {
            _fixture = fixture;
            _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Playlist, PlaylistDto>();
                config.CreateMap<PlaylistDto, Playlist>();
            }).CreateMapper();
        }

        private void Execute(Action<PlaylistService, SoundSphereContext> action)
        {
            using var context = _fixture.CreateContext();
            var playlistService = new PlaylistService(new PlaylistRepository(context), new UserRepository(context), new SongRepository(context), _mapper);
            using var transaction = context.Database.BeginTransaction();
            context.AddRange(_playlists);
            context.SaveChanges();
            action(playlistService, context);
        }

        [Fact] public void FindAll_Test() => Execute((playlistService, context) => playlistService.FindAll().Should().BeEquivalentTo(_playlistDtos));

        [Fact] public void FindById_Test() => Execute((playlistService, context) => playlistService.FindById(Constants.ValidPlaylistGuid).Should().BeEquivalentTo(_playlistDto1));

        [Fact] public void Save_Test() => Execute((playlistService, context) =>
        {
            PlaylistDto newPlaylistDto = PlaylistMock.GetMockedPlaylistDto3();
            playlistService.Save(newPlaylistDto);
            context.Playlists.Find(newPlaylistDto.Id).Should().BeEquivalentTo(PlaylistMock.GetMockedPlaylist3(), options => options.Excluding(playlist => playlist.CreatedAt));
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
            PlaylistDto updatedPlaylistDto = playlistService.ConvertToDto(updatedPlaylist);
            playlistService.UpdateById(_playlistDto2, Constants.ValidPlaylistGuid);
            context.Playlists.Find(Constants.ValidPlaylistGuid).Should().BeEquivalentTo(updatedPlaylist);
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
            PlaylistDto disabledPlaylistDto = playlistService.ConvertToDto(disabledPlaylist);
            playlistService.DisableById(Constants.ValidPlaylistGuid);
            context.Playlists.Find(Constants.ValidPlaylistGuid).Should().BeEquivalentTo(disabledPlaylist);
        });
    }
}