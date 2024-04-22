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
    public class ArtistServiceTest
    {
        private readonly Mock<IArtistRepository> _artistRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly IArtistService _artistService;

        private readonly Artist _artist1 = ArtistMock.GetMockedArtist1();
        private readonly Artist _artist2 = ArtistMock.GetMockedArtist2();
        private readonly IList<Artist> _artists = ArtistMock.GetMockedArtists();
        private readonly IList<Artist> _activeArtists = ArtistMock.GetMockedActiveArtists();
        private readonly ArtistDto _artistDto1 = ArtistMock.GetMockedArtistDto1();
        private readonly ArtistDto _artistDto2 = ArtistMock.GetMockedArtistDto2();
        private readonly IList<ArtistDto> _artistDtos = ArtistMock.GetMockedArtistDtos();
        private readonly IList<ArtistDto> _activeArtistDtos = ArtistMock.GetMockedActiveArtistDtos();

        public ArtistServiceTest()
        {
            _mapperMock.Setup(mock => mock.Map<ArtistDto>(_artist1)).Returns(_artistDto1);
            _mapperMock.Setup(mock => mock.Map<ArtistDto>(_artist2)).Returns(_artistDto2);
            _mapperMock.Setup(mock => mock.Map<Artist>(_artistDto1)).Returns(_artist1);
            _mapperMock.Setup(mock => mock.Map<Artist>(_artistDto2)).Returns(_artist2);
            _artistService = new ArtistService(_artistRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact] public void FindAll_Test()
        {
            _artistRepositoryMock.Setup(mock => mock.FindAll()).Returns(_artists);
            _artistService.FindAll().Should().BeEquivalentTo(_artistDtos);
        }

        [Fact] public void FindAllActive_Test()
        {
            _artistRepositoryMock.Setup(mock => mock.FindAllActive()).Returns(_activeArtists);
            _artistService.FindAllActive().Should().BeEquivalentTo(_activeArtistDtos);
        }

        [Fact] public void FindById_Test()
        {
            _artistRepositoryMock.Setup(mock => mock.FindById(Constants.ValidArtistGuid)).Returns(_artist1);
            _artistService.FindById(Constants.ValidArtistGuid).Should().Be(_artistDto1);
        }

        [Fact] public void Save_Test()
        {
            _artistRepositoryMock.Setup(mock => mock.Save(_artist1)).Returns(_artist1);
            _artistService.Save(_artistDto1).Should().Be(_artistDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            Artist updatedArtist = CreateTestArtist(_artist2, _artist1.IsActive);
            ArtistDto updatedArtistDto = ConvertToDto(updatedArtist);
            _mapperMock.Setup(mock => mock.Map<ArtistDto>(updatedArtist)).Returns(updatedArtistDto);
            _artistRepositoryMock.Setup(mock => mock.UpdateById(_artist2, Constants.ValidArtistGuid)).Returns(updatedArtist);
            _artistService.UpdateById(_artistDto2, Constants.ValidArtistGuid).Should().Be(updatedArtistDto);
        }

        [Fact] public void DisableById_Test()
        {
            Artist disabledArtist = CreateTestArtist(_artist1, false);
            ArtistDto disabledArtistDto = ConvertToDto(disabledArtist);
            _mapperMock.Setup(mock => mock.Map<ArtistDto>(disabledArtist)).Returns(disabledArtistDto);
            _artistRepositoryMock.Setup(mock => mock.DisableById(Constants.ValidArtistGuid)).Returns(disabledArtist);
            _artistService.DisableById(Constants.ValidArtistGuid).Should().Be(disabledArtistDto);
        }

        private Artist CreateTestArtist(Artist artist, bool isActive) => new Artist
        {
            Id = Constants.ValidArtistGuid,
            Name = artist.Name,
            ImageUrl = artist.ImageUrl,
            Bio = artist.Bio,
            SimilarArtists = artist.SimilarArtists,
            IsActive = isActive
        };

        private ArtistDto ConvertToDto(Artist artist) => new ArtistDto
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