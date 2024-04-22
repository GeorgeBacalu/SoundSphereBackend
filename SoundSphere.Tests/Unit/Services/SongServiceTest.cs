﻿using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Services;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database;
using SoundSphere.Database.Dtos;
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
        private readonly Song _song3 = SongMock.GetMockedSong3();
        private readonly Song _song4 = SongMock.GetMockedSong4();
        private readonly IList<Song> _songs = SongMock.GetMockedSongs();
        private readonly IList<Song> _activeSongs = SongMock.GetMockedActiveSongs();
        private readonly SongDto _songDto1 = SongMock.GetMockedSongDto1();
        private readonly SongDto _songDto2 = SongMock.GetMockedSongDto2();
        private readonly SongDto _songDto3 = SongMock.GetMockedSongDto3();
        private readonly SongDto _songDto4 = SongMock.GetMockedSongDto4();
        private readonly IList<SongDto> _songDtos = SongMock.GetMockedSongDtos();
        private readonly IList<SongDto> _activeSongDtos = SongMock.GetMockedActiveSongDtos();
        private readonly Album _album1 = AlbumMock.GetMockedAlbum1();
        private readonly IList<Artist> _artists1 = new List<Artist> { ArtistMock.GetMockedArtist1() };

        public SongServiceTest()
        {
            _mapperMock.Setup(mock => mock.Map<SongDto>(_song1)).Returns(_songDto1);
            _mapperMock.Setup(mock => mock.Map<SongDto>(_song2)).Returns(_songDto2);
            _mapperMock.Setup(mock => mock.Map<SongDto>(_song3)).Returns(_songDto3);
            _mapperMock.Setup(mock => mock.Map<SongDto>(_song4)).Returns(_songDto4);
            _mapperMock.Setup(mock => mock.Map<Song>(_songDto1)).Returns(_song1);
            _mapperMock.Setup(mock => mock.Map<Song>(_songDto2)).Returns(_song2);
            _mapperMock.Setup(mock => mock.Map<Song>(_songDto3)).Returns(_song3);
            _mapperMock.Setup(mock => mock.Map<Song>(_songDto4)).Returns(_song4);
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
            Song updatedSong = CreateTestSong(_song2, _song1.IsActive);
            SongDto updatedSongDto = ConvertToDto(updatedSong);
            _mapperMock.Setup(mock => mock.Map<SongDto>(updatedSong)).Returns(updatedSongDto);
            _songRepositoryMock.Setup(mock => mock.UpdateById(_song2, Constants.ValidSongGuid)).Returns(updatedSong);
            _songService.UpdateById(_songDto2, Constants.ValidSongGuid).Should().Be(updatedSongDto);
        }

        [Fact] public void DisableById_Test()
        {
            Song disabledSong = CreateTestSong(_song1, false);
            SongDto disabledSongDto = ConvertToDto(disabledSong);
            _mapperMock.Setup(mock => mock.Map<SongDto>(disabledSong)).Returns(disabledSongDto);
            _songRepositoryMock.Setup(mock => mock.DisableById(Constants.ValidSongGuid)).Returns(disabledSong);
            _songService.DisableById(Constants.ValidSongGuid).Should().Be(disabledSongDto);
        }

        private Song CreateTestSong(Song song, bool isActive) => new Song
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

        private SongDto ConvertToDto(Song song) => new SongDto
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