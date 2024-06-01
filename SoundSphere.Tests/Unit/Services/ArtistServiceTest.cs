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
using static SoundSphere.Tests.Mocks.ArtistMock;

namespace SoundSphere.Tests.Unit.Services
{
    public class ArtistServiceTest
    {
        private readonly Mock<IArtistRepository> _artistRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly IArtistService _artistService;

        private readonly Artist _artist1 = GetMockedArtist1();
        private readonly Artist _artist2 = GetMockedArtist2();
        private readonly IList<Artist> _artists = GetMockedArtists();
        private readonly IList<Artist> _activeArtists = GetMockedActiveArtists();
        private readonly IList<Artist> _paginatedArtists = GetMockedPaginatedArtists();
        private readonly IList<Artist> _activePaginatedArtists = GetMockedActivePaginatedArtists();
        private readonly ArtistDto _artistDto1 = GetMockedArtistDto1();
        private readonly ArtistDto _artistDto2 = GetMockedArtistDto2();
        private readonly IList<ArtistDto> _artistDtos = GetMockedArtistDtos();
        private readonly IList<ArtistDto> _activeArtistDtos = GetMockedActiveArtistDtos();
        private readonly IList<ArtistDto> _paginatedArtistDtos = GetMockedPaginatedArtistDtos();
        private readonly IList<ArtistDto> _activePaginatedArtistDtos = GetMockedActivePaginatedArtistDtos();
        private readonly ArtistPaginationRequest _paginationRequest = GetMockedArtistsPaginationRequest();

        public ArtistServiceTest()
        {
            _mapperMock.Setup(mock => mock.Map<ArtistDto>(_artist1)).Returns(_artistDto1);
            _mapperMock.Setup(mock => mock.Map<ArtistDto>(_artist2)).Returns(_artistDto2);
            _mapperMock.Setup(mock => mock.Map<Artist>(_artistDto1)).Returns(_artist1);
            _mapperMock.Setup(mock => mock.Map<Artist>(_artistDto2)).Returns(_artist2);
            _artistService = new ArtistService(_artistRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact] public void GetAll_Test()
        {
            _artistRepositoryMock.Setup(mock => mock.GetAll()).Returns(_artists);
            _artistService.GetAll().Should().BeEquivalentTo(_artistDtos);
        }

        [Fact] public void GetAllActive_Test()
        {
            _artistRepositoryMock.Setup(mock => mock.GetAllActive()).Returns(_activeArtists);
            _artistService.GetAllActive().Should().BeEquivalentTo(_activeArtistDtos);
        }

        [Fact] public void GetAllPagination_Test()
        {
            _artistRepositoryMock.Setup(mock => mock.GetAllPagination(_paginationRequest)).Returns(_paginatedArtists);
            _artistService.GetAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedArtistDtos);
        }

        [Fact] public void GetAllActivePagination_Test()
        {
            _artistRepositoryMock.Setup(mock => mock.GetAllActivePagination(_paginationRequest)).Returns(_activePaginatedArtists);
            _artistService.GetAllActivePagination(_paginationRequest).Should().BeEquivalentTo(_activePaginatedArtistDtos);
        }

        [Fact] public void GetById_Test()
        {
            _artistRepositoryMock.Setup(mock => mock.GetById(ValidArtistGuid)).Returns(_artist1);
            _artistService.GetById(ValidArtistGuid).Should().Be(_artistDto1);
        }

        [Fact] public void Add_Test()
        {
            _artistRepositoryMock.Setup(mock => mock.Add(_artist1)).Returns(_artist1);
            _artistService.Add(_artistDto1).Should().Be(_artistDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            Artist updatedArtist = GetArtist(_artist2, _artist1.IsActive);
            ArtistDto updatedArtistDto = ToDto(updatedArtist);
            _mapperMock.Setup(mock => mock.Map<ArtistDto>(updatedArtist)).Returns(updatedArtistDto);
            _artistRepositoryMock.Setup(mock => mock.UpdateById(_artist2, ValidArtistGuid)).Returns(updatedArtist);
            _artistService.UpdateById(_artistDto2, ValidArtistGuid).Should().Be(updatedArtistDto);
        }

        [Fact] public void DeleteById_Test()
        {
            Artist deletedArtist = GetArtist(_artist1, false);
            ArtistDto deletedArtistDto = ToDto(deletedArtist);
            _mapperMock.Setup(mock => mock.Map<ArtistDto>(deletedArtist)).Returns(deletedArtistDto);
            _artistRepositoryMock.Setup(mock => mock.DeleteById(ValidArtistGuid)).Returns(deletedArtist);
            _artistService.DeleteById(ValidArtistGuid).Should().Be(deletedArtistDto);
        }

        private Artist GetArtist(Artist artist, bool isActive) => new Artist
        {
            Id = ValidArtistGuid,
            Name = artist.Name,
            ImageUrl = artist.ImageUrl,
            Bio = artist.Bio,
            SimilarArtists = artist.SimilarArtists,
            IsActive = isActive
        };

        private ArtistDto ToDto(Artist artist) => new ArtistDto
        {
            Id = artist.Id,
            Name = artist.Name,
            ImageUrl = artist.ImageUrl,
            Bio = artist.Bio,
            SimilarArtistsIds = artist.SimilarArtists.Select(artistLink => artistLink.SimilarArtistId).ToList(),
            IsActive = artist.IsActive
        };
    }
}