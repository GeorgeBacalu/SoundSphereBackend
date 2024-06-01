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
using static SoundSphere.Tests.Mocks.AlbumMock;

namespace SoundSphere.Tests.Unit.Services
{
    public class AlbumServiceTest
    {
        private readonly Mock<IAlbumRepository> _albumRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly IAlbumService _albumService;

        private readonly Album _album1 = GetMockedAlbum1();
        private readonly Album _album2 = GetMockedAlbum2();
        private readonly IList<Album> _albums = GetMockedAlbums();
        private readonly IList<Album> _activeAlbums = GetMockedActiveAlbums();
        private readonly IList<Album> _paginatedAlbums = GetMockedPaginatedAlbums();
        private readonly IList<Album> _activePaginatedAlbums = GetMockedActivePaginatedAlbums();
        private readonly AlbumDto _albumDto1 = GetMockedAlbumDto1();
        private readonly AlbumDto _albumDto2 = GetMockedAlbumDto2();
        private readonly IList<AlbumDto> _albumDtos = GetMockedAlbumDtos();
        private readonly IList<AlbumDto> _activeAlbumDtos = GetMockedActiveAlbumDtos();
        private readonly IList<AlbumDto> _paginatedAlbumDtos = GetMockedPaginatedAlbumDtos();
        private readonly IList<AlbumDto> _activePaginatedAlbumDtos = GetMockedActivePaginatedAlbumDtos();
        private readonly AlbumPaginationRequest _paginationRequest = GetMockedAlbumsPaginationRequest();

        public AlbumServiceTest()
        {
            _mapperMock.Setup(mock => mock.Map<AlbumDto>(_album1)).Returns(_albumDto1);
            _mapperMock.Setup(mock => mock.Map<AlbumDto>(_album2)).Returns(_albumDto2);
            _mapperMock.Setup(mock => mock.Map<Album>(_albumDto1)).Returns(_album1);
            _mapperMock.Setup(mock => mock.Map<Album>(_albumDto2)).Returns(_album2);
            _albumService = new AlbumService(_albumRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact] public void GetAll_Test()
        {
            _albumRepositoryMock.Setup(mock => mock.GetAll()).Returns(_albums);
            _albumService.GetAll().Should().BeEquivalentTo(_albumDtos);
        }

        [Fact] public void GetAllActive_Test()
        {
            _albumRepositoryMock.Setup(mock => mock.GetAllActive()).Returns(_activeAlbums);
            _albumService.GetAllActive().Should().BeEquivalentTo(_activeAlbumDtos);
        }

        [Fact] public void GetAllPagination_Test()
        {
            _albumRepositoryMock.Setup(mock => mock.GetAllPagination(_paginationRequest)).Returns(_paginatedAlbums);
            _albumService.GetAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedAlbumDtos);
        }

        [Fact] public void GetAllActivePagination_Test()
        {
            _albumRepositoryMock.Setup(mock => mock.GetAllActivePagination(_paginationRequest)).Returns(_activePaginatedAlbums);
            _albumService.GetAllActivePagination(_paginationRequest).Should().BeEquivalentTo(_activePaginatedAlbumDtos);
        }

        [Fact] public void GetById_Test()
        {
            _albumRepositoryMock.Setup(mock => mock.GetById(ValidAlbumGuid)).Returns(_album1);
            _albumService.GetById(ValidAlbumGuid).Should().Be(_albumDto1);
        }
            
        [Fact] public void Add_Test()
        {
            _albumRepositoryMock.Setup(mock => mock.Add(_album1)).Returns(_album1);
            _albumService.Add(_albumDto1).Should().Be(_albumDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            Album updatedAlbum = GetAlbum(_album2, _album1.IsActive);
            AlbumDto updatedAlbumDto = ToDto(updatedAlbum);
            _mapperMock.Setup(mock => mock.Map<AlbumDto>(updatedAlbum)).Returns(updatedAlbumDto);
            _albumRepositoryMock.Setup(mock => mock.UpdateById(_album2, ValidAlbumGuid)).Returns(updatedAlbum);
            _albumService.UpdateById(_albumDto2, ValidAlbumGuid).Should().Be(updatedAlbumDto);
        }

        [Fact] public void DeleteById_Test()
        {
            Album deletedAlbum = GetAlbum(_album1, false);
            AlbumDto deletedAlbumDto = ToDto(deletedAlbum);
            _mapperMock.Setup(mock => mock.Map<AlbumDto>(deletedAlbum)).Returns(deletedAlbumDto);
            _albumRepositoryMock.Setup(mock => mock.DeleteById(ValidAlbumGuid)).Returns(deletedAlbum);
            _albumService.DeleteById(ValidAlbumGuid).Should().Be(deletedAlbumDto);
        }

        private Album GetAlbum(Album album, bool isActive) => new Album
        {
            Id = ValidAlbumGuid,
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