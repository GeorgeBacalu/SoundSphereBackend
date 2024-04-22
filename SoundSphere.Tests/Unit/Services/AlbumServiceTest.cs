using AutoMapper;
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
    public class AlbumServiceTest
    {
        private readonly Mock<IAlbumRepository> _albumRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly IAlbumService _albumService;

        private readonly Album _album1 = AlbumMock.GetMockedAlbum1();
        private readonly Album _album2 = AlbumMock.GetMockedAlbum2();
        private readonly IList<Album> _albums = AlbumMock.GetMockedAlbums();
        private readonly IList<Album> _activeAlbums = AlbumMock.GetMockedActiveAlbums();
        private readonly AlbumDto _albumDto1 = AlbumMock.GetMockedAlbumDto1();
        private readonly AlbumDto _albumDto2 = AlbumMock.GetMockedAlbumDto2();
        private readonly IList<AlbumDto> _albumDtos = AlbumMock.GetMockedAlbumDtos();
        private readonly IList<AlbumDto> _activeAlbumDtos = AlbumMock.GetMockedActiveAlbumDtos();

        public AlbumServiceTest()
        {
            _mapperMock.Setup(mock => mock.Map<AlbumDto>(_album1)).Returns(_albumDto1);
            _mapperMock.Setup(mock => mock.Map<AlbumDto>(_album2)).Returns(_albumDto2);
            _mapperMock.Setup(mock => mock.Map<Album>(_albumDto1)).Returns(_album1);
            _mapperMock.Setup(mock => mock.Map<Album>(_albumDto2)).Returns(_album2);
            _albumService = new AlbumService(_albumRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact] public void FindAll_Test()
        {
            _albumRepositoryMock.Setup(mock => mock.FindAll()).Returns(_albums);
            _albumService.FindAll().Should().BeEquivalentTo(_albumDtos);
        }

        [Fact] public void FindAllActive_Test()
        {
            _albumRepositoryMock.Setup(mock => mock.FindAllActive()).Returns(_activeAlbums);
            _albumService.FindAllActive().Should().BeEquivalentTo(_activeAlbumDtos);
        }

        [Fact] public void FindById_Test()
        {
            _albumRepositoryMock.Setup(mock => mock.FindById(Constants.ValidAlbumGuid)).Returns(_album1);
            _albumService.FindById(Constants.ValidAlbumGuid).Should().Be(_albumDto1);
        }
            
        [Fact] public void Save_Test()
        {
            _albumRepositoryMock.Setup(mock => mock.Save(_album1)).Returns(_album1);
            _albumService.Save(_albumDto1).Should().Be(_albumDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            Album updatedAlbum = CreateTestAlbum(_album2, _album1.IsActive);
            AlbumDto updatedAlbumDto = ConvertToDto(updatedAlbum);
            _mapperMock.Setup(mock => mock.Map<AlbumDto>(updatedAlbum)).Returns(updatedAlbumDto);
            _albumRepositoryMock.Setup(mock => mock.UpdateById(_album2, Constants.ValidAlbumGuid)).Returns(updatedAlbum);
            _albumService.UpdateById(_albumDto2, Constants.ValidAlbumGuid).Should().Be(updatedAlbumDto);
        }

        [Fact] public void DisableById_Test()
        {
            Album disabledAlbum = CreateTestAlbum(_album1, false);
            AlbumDto disabledAlbumDto = ConvertToDto(disabledAlbum);
            _mapperMock.Setup(mock => mock.Map<AlbumDto>(disabledAlbum)).Returns(disabledAlbumDto);
            _albumRepositoryMock.Setup(mock => mock.DisableById(Constants.ValidAlbumGuid)).Returns(disabledAlbum);
            _albumService.DisableById(Constants.ValidAlbumGuid).Should().Be(disabledAlbumDto);
        }

        private Album CreateTestAlbum(Album album, bool isActive) => new Album
        {
            Id = Constants.ValidAlbumGuid,
            Title = album.Title,
            ImageUrl = album.ImageUrl,
            ReleaseDate = album.ReleaseDate,
            SimilarAlbums = album.SimilarAlbums,
            IsActive = isActive
        };

        private AlbumDto ConvertToDto(Album album) => new AlbumDto
        {
            Id = album.Id,
            Title = album.Title,
            ImageUrl = album.ImageUrl,
            ReleaseDate = album.ReleaseDate,
            SimilarAlbumsIds = album.SimilarAlbums.Select(albumLink => albumLink.SimilarAlbumId).ToList(),
            IsActive = album.IsActive
        };
    }
}