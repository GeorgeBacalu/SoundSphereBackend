using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoundSphere.Api.Controllers;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Dtos;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Controllers
{
    public class ArtistControllerTest
    {
        private readonly Mock<IArtistService> _artistService = new();
        private readonly ArtistController _artistController;

        private readonly ArtistDto _artistDto1 = ArtistMock.GetMockedArtistDto1();
        private readonly ArtistDto _artistDto2 = ArtistMock.GetMockedArtistDto2();
        private readonly IList<ArtistDto> _artistDtos = ArtistMock.GetMockedArtistDtos();
        private readonly IList<ArtistDto> _activeArtistDtos = ArtistMock.GetMockedActiveArtistDtos();

        public ArtistControllerTest() => _artistController = new(_artistService.Object);

        [Fact] public void FindAll_Test()
        {
            _artistService.Setup(mock => mock.FindAll()).Returns(_artistDtos);
            var result = _artistController.FindAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_artistDtos);
        }

        [Fact] public void FindAllActive_Test()
        {
            _artistService.Setup(mock => mock.FindAllActive()).Returns(_activeArtistDtos);
            var result = _artistController.FindAllActive() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_activeArtistDtos);
        }

        [Fact] public void FindById_Test()
        {
            _artistService.Setup(mock => mock.FindById(Constants.ValidArtistGuid)).Returns(_artistDto1);
            var result = _artistController.FindById(Constants.ValidArtistGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_artistDto1);
        }

        [Fact] public void Save_Test()
        {
            _artistService.Setup(mock => mock.Save(_artistDto1)).Returns(_artistDto1);
            var result = _artistController.Save(_artistDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status201Created);
            result?.Value.Should().BeEquivalentTo(_artistDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            ArtistDto updatedArtistDto = CreateTestArtistDto(_artistDto2, _artistDto1.IsActive);
            _artistService.Setup(mock => mock.UpdateById(_artistDto2, Constants.ValidArtistGuid)).Returns(updatedArtistDto);
            var result = _artistController.UpdateById(_artistDto2, Constants.ValidArtistGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(updatedArtistDto);
        }

        [Fact] public void DisableById_Test()
        {
            ArtistDto disabledArtistDto = CreateTestArtistDto(_artistDto1, false);
            _artistService.Setup(mock => mock.DisableById(Constants.ValidArtistGuid)).Returns(disabledArtistDto);
            var result = _artistController.DisableById(Constants.ValidArtistGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(disabledArtistDto);
        }

        private ArtistDto CreateTestArtistDto(ArtistDto artistDto, bool isActive) => new ArtistDto
        {
            Id = Constants.ValidArtistGuid,
            Name = artistDto.Name,
            ImageUrl = artistDto.ImageUrl,
            Bio = artistDto.Bio,
            SimilarArtistsIds = artistDto.SimilarArtistsIds,
            IsActive = isActive,
        };
    }
}