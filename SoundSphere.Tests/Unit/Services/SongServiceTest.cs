using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Services;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.SongMock;
using static SoundSphere.Tests.Mocks.AlbumMock;
using static SoundSphere.Tests.Mocks.ArtistMock;
using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;

namespace SoundSphere.Tests.Unit.Services
{
    public class SongServiceTest
    {
        private readonly Mock<ISongRepository> _songRepositoryMock = new();
        private readonly Mock<IAlbumRepository> _albumRepositoryMock = new();
        private readonly Mock<IArtistRepository> _artistRepositoryMock = new();
        private readonly Mock<DbSet<Album>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly ISongService _songService;

        private readonly Song _song1 = GetMockedSong1();
        private readonly Song _song2 = GetMockedSong2();
        private readonly IList<Song> _songs = GetMockedSongs();
        private readonly IList<Song> _paginatedSongs = GetMockedPaginatedSongs();
        private readonly SongDto _songDto1 = GetMockedSongDto1();
        private readonly SongDto _songDto2 = GetMockedSongDto2();
        private readonly IList<SongDto> _songDtos = GetMockedSongDtos();
        private readonly IList<SongDto> _paginatedSongDtos = GetMockedPaginatedSongDtos();
        private readonly SongPaginationRequest _paginationRequest = GetMockedSongsPaginationRequest();
        private readonly Album _album1 = GetMockedAlbum1();
        private readonly IList<Artist> _artists1 = new List<Artist> { GetMockedArtist1() };

        public SongServiceTest()
        {
            _mapperMock.Setup(mock => mock.Map<SongDto>(_song1)).Returns(_songDto1);
            _mapperMock.Setup(mock => mock.Map<SongDto>(_song2)).Returns(_songDto2);
            _mapperMock.Setup(mock => mock.Map<Song>(_songDto1)).Returns(_song1);
            _mapperMock.Setup(mock => mock.Map<Song>(_songDto2)).Returns(_song2);
            _songService = new SongService(_songRepositoryMock.Object, _albumRepositoryMock.Object, _artistRepositoryMock.Object, _dbContextMock.Object, _mapperMock.Object);
        }

        [Fact] public void GetAll_Test()
        {
            _songRepositoryMock.Setup(mock => mock.GetAll(_paginationRequest)).Returns(_paginatedSongs);
            _songService.GetAll(_paginationRequest).Should().BeEquivalentTo(_paginatedSongDtos);
        }

        [Fact] public void GetById_Test()
        {
            _songRepositoryMock.Setup(mock => mock.GetById(ValidSongGuid)).Returns(_song1);
            _songService.GetById(ValidSongGuid).Should().Be(_songDto1);
        }

        [Fact] public void Add_Test()
        {
            _songDto1.ArtistsIds.ToList().ForEach(id => _artistRepositoryMock.Setup(mock => mock.GetById(id)).Returns(_artists1.First(artist => artist.Id == id)));
            _albumRepositoryMock.Setup(mock => mock.GetById(ValidAlbumGuid)).Returns(_album1);
            _songRepositoryMock.Setup(mock => mock.Add(_song1)).Returns(_song1);
            _songService.Add(_songDto1).Should().Be(_songDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            Song updatedSong = GetSong(_song2, true);
            SongDto updatedSongDto = ToDto(updatedSong);
            _mapperMock.Setup(mock => mock.Map<SongDto>(updatedSong)).Returns(updatedSongDto);
            _songRepositoryMock.Setup(mock => mock.UpdateById(_song2, ValidSongGuid)).Returns(updatedSong);
            _songService.UpdateById(_songDto2, ValidSongGuid).Should().Be(updatedSongDto);
        }

        [Fact] public void DeleteById_Test()
        {
            Song deletedSong = GetSong(_song1, false);
            SongDto deletedSongDto = ToDto(deletedSong);
            _mapperMock.Setup(mock => mock.Map<SongDto>(deletedSong)).Returns(deletedSongDto);
            _songRepositoryMock.Setup(mock => mock.DeleteById(ValidSongGuid)).Returns(deletedSong);
            _songService.DeleteById(ValidSongGuid).Should().Be(deletedSongDto);
        }

        private Song GetSong(Song song, bool isActive) => new Song
        {
            Id = ValidSongGuid,
            Title = song.Title,
            ImageUrl = song.ImageUrl,
            Genre = song.Genre,
            ReleaseDate = song.ReleaseDate,
            DurationSeconds = song.DurationSeconds,
            Album = song.Album,
            Artists = song.Artists,
            SimilarSongs = song.SimilarSongs
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
            CreatedAt = song.CreatedAt,
            UpdatedAt = song.UpdatedAt,
            DeletedAt = song.DeletedAt
        };
    }
}