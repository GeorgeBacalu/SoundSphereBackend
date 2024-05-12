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
    public class AlbumServiceTest
    {
        private readonly Mock<IAlbumRepository> _albumRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly IAlbumService _albumService;

        private readonly Album _album1 = AlbumMock.GetMockedAlbum1();
        private readonly Album _album2 = AlbumMock.GetMockedAlbum2();
        private readonly IList<Album> _albums = AlbumMock.GetMockedAlbums();
        private readonly IList<Album> _activeAlbums = AlbumMock.GetMockedActiveAlbums();
        private readonly IList<Album> _paginatedAlbums = AlbumMock.GetMockedPaginatedAlbums();
        private readonly IList<Album> _activePaginatedAlbums = AlbumMock.GetMockedActivePaginatedAlbums();
        private readonly AlbumDto _albumDto1 = AlbumMock.GetMockedAlbumDto1();
        private readonly AlbumDto _albumDto2 = AlbumMock.GetMockedAlbumDto2();
        private readonly IList<AlbumDto> _albumDtos = AlbumMock.GetMockedAlbumDtos();
        private readonly IList<AlbumDto> _activeAlbumDtos = AlbumMock.GetMockedActiveAlbumDtos();
        private readonly IList<AlbumDto> _paginatedAlbumDtos = AlbumMock.GetMockedPaginatedAlbumDtos();
        private readonly IList<AlbumDto> _activePaginatedAlbumDtos = AlbumMock.GetMockedActivePaginatedAlbumDtos();
        private readonly AlbumPaginationRequest _paginationRequest = AlbumMock.GetMockedPaginationRequest();

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

        [Fact] public void FindAllPagination_Test()
        {
            _albumRepositoryMock.Setup(mock => mock.FindAllPagination(_paginationRequest)).Returns(_paginatedAlbums);
            _albumService.FindAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedAlbumDtos);
        }

        [Fact] public void FindAllActivePagination_Test()
        {
            _albumRepositoryMock.Setup(mock => mock.FindAllActivePagination(_paginationRequest)).Returns(_activePaginatedAlbums);
            _albumService.FindAllActivePagination(_paginationRequest).Should().BeEquivalentTo(_activePaginatedAlbumDtos);
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
            Album updatedAlbum = GetAlbum(_album2, _album1.IsActive);
            AlbumDto updatedAlbumDto = ToDto(updatedAlbum);
            _mapperMock.Setup(mock => mock.Map<AlbumDto>(updatedAlbum)).Returns(updatedAlbumDto);
            _albumRepositoryMock.Setup(mock => mock.UpdateById(_album2, Constants.ValidAlbumGuid)).Returns(updatedAlbum);
            _albumService.UpdateById(_albumDto2, Constants.ValidAlbumGuid).Should().Be(updatedAlbumDto);
        }

        [Fact] public void DisableById_Test()
        {
            Album disabledAlbum = GetAlbum(_album1, false);
            AlbumDto disabledAlbumDto = ToDto(disabledAlbum);
            _mapperMock.Setup(mock => mock.Map<AlbumDto>(disabledAlbum)).Returns(disabledAlbumDto);
            _albumRepositoryMock.Setup(mock => mock.DisableById(Constants.ValidAlbumGuid)).Returns(disabledAlbum);
            _albumService.DisableById(Constants.ValidAlbumGuid).Should().Be(disabledAlbumDto);
        }

        private Album GetAlbum(Album album, bool isActive) => new Album
        {
            Id = Constants.ValidAlbumGuid,
            Title = album.Title,
            ImageUrl = album.ImageUrl,
            ReleaseDate = album.ReleaseDate,
            SimilarAlbums = album.SimilarAlbums,
            IsActive = isActive
        };

        private AlbumDto ToDto(Album album) => new AlbumDto
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