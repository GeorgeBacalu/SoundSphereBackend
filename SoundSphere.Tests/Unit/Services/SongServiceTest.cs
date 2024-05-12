using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Services;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Services
{
    public class SongServiceTest
    {
        private readonly Mock<ISongRepository> _songRepositoryMock = new();
        private readonly Mock<IAlbumRepository> _albumRepositoryMock = new();
        private readonly Mock<IArtistRepository> _artistRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly ISongService _songService;

        private readonly Song _song1 = SongMock.GetMockedSong1();
        private readonly Song _song2 = SongMock.GetMockedSong2();
        private readonly IList<Song> _songs = SongMock.GetMockedSongs();
        private readonly IList<Song> _activeSongs = SongMock.GetMockedActiveSongs();
        private readonly IList<Song> _paginatedSongs = SongMock.GetMockedPaginatedSongs();
        private readonly IList<Song> _activePaginatedSongs = SongMock.GetMockedActivePaginatedSongs();
        private readonly SongDto _songDto1 = SongMock.GetMockedSongDto1();
        private readonly SongDto _songDto2 = SongMock.GetMockedSongDto2();
        private readonly IList<SongDto> _songDtos = SongMock.GetMockedSongDtos();
        private readonly IList<SongDto> _activeSongDtos = SongMock.GetMockedActiveSongDtos();
        private readonly IList<SongDto> _paginatedSongDtos = SongMock.GetMockedPaginatedSongDtos();
        private readonly IList<SongDto> _activePaginatedSongDtos = SongMock.GetMockedActivePaginatedSongDtos();
        private readonly SongPaginationRequest _paginationRequest = SongMock.GetMockedPaginationRequest();
        private readonly Album _album1 = AlbumMock.GetMockedAlbum1();
        private readonly IList<Artist> _artists1 = new List<Artist> { ArtistMock.GetMockedArtist1() };

        public SongServiceTest()
        {
            _mapperMock.Setup(mock => mock.Map<SongDto>(_song1)).Returns(_songDto1);
            _mapperMock.Setup(mock => mock.Map<SongDto>(_song2)).Returns(_songDto2);
            _mapperMock.Setup(mock => mock.Map<Song>(_songDto1)).Returns(_song1);
            _mapperMock.Setup(mock => mock.Map<Song>(_songDto2)).Returns(_song2);
            _songService = new SongService(_songRepositoryMock.Object, _albumRepositoryMock.Object, _artistRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact] public void FindAll_Test()
        {
            _songRepositoryMock.Setup(mock => mock.FindAll()).Returns(_songs);
            _songService.FindAll().Should().BeEquivalentTo(_songDtos);
        }

        [Fact] public void FindAllActive_Test()
        {
            _songRepositoryMock.Setup(mock => mock.FindAllActive()).Returns(_activeSongs);
            _songService.FindAllActive().Should().BeEquivalentTo(_activeSongDtos);
        }

        [Fact] public void FindAllPagination_Test()
        {
            _songRepositoryMock.Setup(mock => mock.FindAllPagination(_paginationRequest)).Returns(_paginatedSongs);
            _songService.FindAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedSongDtos);
        }

        [Fact] public void FindAllActivePagination_Test()
        {
            _songRepositoryMock.Setup(mock => mock.FindAllActivePagination(_paginationRequest)).Returns(_activePaginatedSongs);
            _songService.FindAllActivePagination(_paginationRequest).Should().BeEquivalentTo(_activePaginatedSongDtos);
        }

        [Fact] public void FindById_Test()
        {
            _songRepositoryMock.Setup(mock => mock.FindById(Constants.ValidSongGuid)).Returns(_song1);
            _songService.FindById(Constants.ValidSongGuid).Should().Be(_songDto1);
        }

        [Fact] public void Save_Test()
        {
            _songDto1.ArtistsIds.ToList().ForEach(id => _artistRepositoryMock.Setup(mock => mock.FindById(id)).Returns(_artists1.First(artist => artist.Id == id)));
            _albumRepositoryMock.Setup(mock => mock.FindById(Constants.ValidAlbumGuid)).Returns(_album1);
            _songRepositoryMock.Setup(mock => mock.Save(_song1)).Returns(_song1);
            _songService.Save(_songDto1).Should().Be(_songDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            Song updatedSong = GetSong(_song2, _song1.IsActive);
            SongDto updatedSongDto = ToDto(updatedSong);
            _mapperMock.Setup(mock => mock.Map<SongDto>(updatedSong)).Returns(updatedSongDto);
            _songRepositoryMock.Setup(mock => mock.UpdateById(_song2, Constants.ValidSongGuid)).Returns(updatedSong);
            _songService.UpdateById(_songDto2, Constants.ValidSongGuid).Should().Be(updatedSongDto);
        }

        [Fact] public void DisableById_Test()
        {
            Song disabledSong = GetSong(_song1, false);
            SongDto disabledSongDto = ToDto(disabledSong);
            _mapperMock.Setup(mock => mock.Map<SongDto>(disabledSong)).Returns(disabledSongDto);
            _songRepositoryMock.Setup(mock => mock.DisableById(Constants.ValidSongGuid)).Returns(disabledSong);
            _songService.DisableById(Constants.ValidSongGuid).Should().Be(disabledSongDto);
        }

        private Song GetSong(Song song, bool isActive) => new Song
        {
            Id = Constants.ValidSongGuid,
            Title = song.Title,
            ImageUrl = song.ImageUrl,
            Genre = song.Genre,
            ReleaseDate = song.ReleaseDate,
            DurationSeconds = song.DurationSeconds,
            Album = song.Album,
            Artists = song.Artists,
            SimilarSongs = song.SimilarSongs,
            IsActive = isActive
        };

        private SongDto ToDto(Song song) => new SongDto
        {
            Id = song.Id,
            Title = song.Title,
            ImageUrl = song.ImageUrl,
            Genre = song.Genre,
            ReleaseDate = song.ReleaseDate,
            DurationSeconds = song.DurationSeconds,
            AlbumId = song.Album.Id,
            ArtistsIds = song.Artists.Select(artist => artist.Id).ToList(),
            SimilarSongsIds = song.SimilarSongs.Select(songLink => songLink.SimilarSongId).ToList(),
            IsActive = song.IsActive
        };
    }
}