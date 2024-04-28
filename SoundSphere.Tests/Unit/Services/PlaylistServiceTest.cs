using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Services;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Services
{
    public class PlaylistServiceTest
    {
        private readonly Mock<IPlaylistRepository> _playlistRepositoryMock = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<ISongRepository> _songRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly IPlaylistService _playlistService;

        private readonly Playlist _playlist1 = PlaylistMock.GetMockedPlaylist1();
        private readonly Playlist _playlist2 = PlaylistMock.GetMockedPlaylist2();
        private readonly IList<Playlist> _playlists = PlaylistMock.GetMockedPlaylists();
        private readonly IList<Playlist> _activePlaylists = PlaylistMock.GetMockedActivePlaylists();
        private readonly PlaylistDto _playlistDto1 = PlaylistMock.GetMockedPlaylistDto1();
        private readonly PlaylistDto _playlistDto2 = PlaylistMock.GetMockedPlaylistDto2();
        private readonly IList<PlaylistDto> _playlistDtos = PlaylistMock.GetMockedPlaylistDtos();
        private readonly IList<PlaylistDto> _activePlaylistDtos = PlaylistMock.GetMockedActivePlaylistDtos();
        private readonly User _user1 = UserMock.GetMockedUser1();
        private readonly IList<Song> _songs1 = SongMock.GetMockedSongs1();

        public PlaylistServiceTest()
        {
            _mapperMock.Setup(mock => mock.Map<PlaylistDto>(_playlist1)).Returns(_playlistDto1);
            _mapperMock.Setup(mock => mock.Map<PlaylistDto>(_playlist2)).Returns(_playlistDto2);
            _mapperMock.Setup(mock => mock.Map<Playlist>(_playlistDto1)).Returns(_playlist1);
            _mapperMock.Setup(mock => mock.Map<Playlist>(_playlistDto2)).Returns(_playlist2);
            _playlistService = new PlaylistService(_playlistRepositoryMock.Object, _userRepositoryMock.Object, _songRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact] public void FindAll_Test()
        {
            _playlistRepositoryMock.Setup(mock => mock.FindAll()).Returns(_playlists);
            _playlistService.FindAll().Should().BeEquivalentTo(_playlistDtos);
        }

        [Fact] public void FindAllActive_Test()
        {
            _playlistRepositoryMock.Setup(mock => mock.FindAllActive()).Returns(_activePlaylists);
            _playlistService.FindAllActive().Should().BeEquivalentTo(_activePlaylistDtos);
        }

        [Fact] public void FindById_Test()
        {
            _playlistRepositoryMock.Setup(mock => mock.FindById(Constants.ValidPlaylistGuid)).Returns(_playlist1);
            _playlistService.FindById(Constants.ValidPlaylistGuid).Should().Be(_playlistDto1);
        }

        [Fact] public void Save_Test()
        {
            _playlistDto1.SongsIds.ToList().ForEach(id => _songRepositoryMock.Setup(mock => mock.FindById(id)).Returns(_songs1.First(song => song.Id == id)));
            _userRepositoryMock.Setup(mock => mock.FindById(Constants.ValidUserGuid)).Returns(_user1);
            _playlistRepositoryMock.Setup(mock => mock.Save(_playlist1)).Returns(_playlist1);
            _playlistService.Save(_playlistDto1).Should().Be(_playlistDto1);
        }

        [Fact] public void UpdateById_Test()
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
            PlaylistDto updatedPlaylistDto = ConvertToDto(updatedPlaylist);
            _mapperMock.Setup(mock => mock.Map<PlaylistDto>(updatedPlaylist)).Returns(updatedPlaylistDto);
            _playlistRepositoryMock.Setup(mock => mock.UpdateById(_playlist2, Constants.ValidPlaylistGuid)).Returns(updatedPlaylist);
            _playlistService.UpdateById(_playlistDto2, Constants.ValidPlaylistGuid).Should().Be(updatedPlaylistDto);
        }

        [Fact] public void DisableById_Test()
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
            PlaylistDto disabledPlaylistDto = ConvertToDto(disabledPlaylist);
            _mapperMock.Setup(mock => mock.Map<PlaylistDto>(disabledPlaylist)).Returns(disabledPlaylistDto);
            _playlistRepositoryMock.Setup(mock => mock.DisableById(Constants.ValidPlaylistGuid)).Returns(disabledPlaylist);
            _playlistService.DisableById(Constants.ValidPlaylistGuid).Should().Be(disabledPlaylistDto);
        }

        private PlaylistDto ConvertToDto(Playlist playlist) => new PlaylistDto
        {
            Id = playlist.Id,
            Title = playlist.Title,
            UserId = playlist.User.Id,
            SongsIds = playlist.Songs.Select(song => song.Id).ToList(),
            CreatedAt = playlist.CreatedAt,
            IsActive = playlist.IsActive
        };
    }
}