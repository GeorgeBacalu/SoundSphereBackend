using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Services;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.PlaylistMock;
using static SoundSphere.Tests.Mocks.UserMock;
using static SoundSphere.Tests.Mocks.SongMock;

namespace SoundSphere.Tests.Unit.Services
{
    public class PlaylistServiceTest
    {
        private readonly Mock<IPlaylistRepository> _playlistRepositoryMock = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<ISongRepository> _songRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly IPlaylistService _playlistService;

        private readonly Playlist _playlist1 = GetMockedPlaylist1();
        private readonly Playlist _playlist2 = GetMockedPlaylist2();
        private readonly IList<Playlist> _playlists = GetMockedPlaylists();
        private readonly IList<Playlist> _paginatedPlaylists = GetMockedPaginatedPlaylists();
        private readonly PlaylistDto _playlistDto1 = GetMockedPlaylistDto1();
        private readonly PlaylistDto _playlistDto2 = GetMockedPlaylistDto2();
        private readonly IList<PlaylistDto> _playlistDtos = GetMockedPlaylistDtos();
        private readonly IList<PlaylistDto> _paginatedPlaylistDtos = GetMockedPaginatedPlaylistDtos();
        private readonly PlaylistPaginationRequest _paginationRequest = GetMockedPlaylistsPaginationRequest();
        private readonly User _user1 = GetMockedUser1();
        private readonly IList<Song> _songs1 = GetMockedSongs1();

        public PlaylistServiceTest()
        {
            _mapperMock.Setup(mock => mock.Map<PlaylistDto>(_playlist1)).Returns(_playlistDto1);
            _mapperMock.Setup(mock => mock.Map<PlaylistDto>(_playlist2)).Returns(_playlistDto2);
            _mapperMock.Setup(mock => mock.Map<Playlist>(_playlistDto1)).Returns(_playlist1);
            _mapperMock.Setup(mock => mock.Map<Playlist>(_playlistDto2)).Returns(_playlist2);
            _playlistService = new PlaylistService(_playlistRepositoryMock.Object, _userRepositoryMock.Object, _songRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact] public void GetAll_Test()
        {
            _playlistRepositoryMock.Setup(mock => mock.GetAll(_paginationRequest)).Returns(_paginatedPlaylists);
            _playlistService.GetAll(_paginationRequest).Should().BeEquivalentTo(_paginatedPlaylistDtos);
        }

        [Fact] public void GetById_Test()
        {
            _playlistRepositoryMock.Setup(mock => mock.GetById(ValidPlaylistGuid)).Returns(_playlist1);
            _playlistService.GetById(ValidPlaylistGuid).Should().Be(_playlistDto1);
        }

        [Fact] public void Add_Test()
        {
            _playlistDto1.SongsIds.ToList().ForEach(id => _songRepositoryMock.Setup(mock => mock.GetById(id)).Returns(_songs1.First(song => song.Id == id)));
            _userRepositoryMock.Setup(mock => mock.GetById(ValidUserGuid)).Returns(_user1);
            _playlistRepositoryMock.Setup(mock => mock.Add(_playlist1)).Returns(_playlist1);
            _playlistService.Add(_playlistDto1).Should().Be(_playlistDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            Playlist updatedPlaylist = new Playlist
            {
                Id = ValidPlaylistGuid,
                Title = _playlist2.Title,
                User = _playlist1.User,
                Songs = _playlist1.Songs,
                CreatedAt = _playlist1.CreatedAt,
            };
            PlaylistDto updatedPlaylistDto = ToDto(updatedPlaylist);
            _mapperMock.Setup(mock => mock.Map<PlaylistDto>(updatedPlaylist)).Returns(updatedPlaylistDto);
            _playlistRepositoryMock.Setup(mock => mock.UpdateById(_playlist2, ValidPlaylistGuid)).Returns(updatedPlaylist);
            _playlistService.UpdateById(_playlistDto2, ValidPlaylistGuid).Should().Be(updatedPlaylistDto);
        }

        [Fact] public void DeleteById_Test()
        {
            Playlist deletedPlaylist = new Playlist
            {
                Id = ValidPlaylistGuid,
                Title = _playlist1.Title,
                User = _playlist1.User,
                Songs = _playlist1.Songs,
                CreatedAt = _playlist1.CreatedAt,
            };
            PlaylistDto deletedPlaylistDto = ToDto(deletedPlaylist);
            _mapperMock.Setup(mock => mock.Map<PlaylistDto>(deletedPlaylist)).Returns(deletedPlaylistDto);
            _playlistRepositoryMock.Setup(mock => mock.DeleteById(ValidPlaylistGuid)).Returns(deletedPlaylist);
            _playlistService.DeleteById(ValidPlaylistGuid).Should().Be(deletedPlaylistDto);
        }

        private PlaylistDto ToDto(Playlist playlist) => new PlaylistDto
        {
            Id = playlist.Id,
            Title = playlist.Title,
            UserId = playlist.User.Id,
            SongsIds = playlist.Songs.Select(song => song.Id).ToList(),
            CreatedAt = playlist.CreatedAt,
            UpdatedAt = playlist.UpdatedAt,
            DeletedAt = playlist.DeletedAt
        };
    }
}