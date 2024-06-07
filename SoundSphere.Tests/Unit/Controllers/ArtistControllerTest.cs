using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoundSphere.Api.Controllers;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.ArtistMock;

namespace SoundSphere.Tests.Unit.Controllers
{
    public class ArtistControllerTest
    {
        private readonly Mock<IArtistService> _artistServiceMock = new();
        private readonly ArtistController _artistController;

        private readonly ArtistDto _artistDto1 = GetMockedArtistDto1();
        private readonly ArtistDto _artistDto2 = GetMockedArtistDto2();
        private readonly IList<ArtistDto> _artistDtos = GetMockedArtistDtos();
        private readonly IList<ArtistDto> _paginatedArtistDtos = GetMockedPaginatedArtistDtos();
        private readonly ArtistPaginationRequest _paginationRequest = GetMockedArtistsPaginationRequest();

        public ArtistControllerTest() => _artistController = new(_artistServiceMock.Object);

        [Fact] public void GetAll_Test()
        {
            _artistServiceMock.Setup(mock => mock.GetAll(_paginationRequest)).Returns(_paginatedArtistDtos);
            OkObjectResult? result = _artistController.GetAll(_paginationRequest) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_paginatedArtistDtos);
        }

        [Fact] public void GetById_Test()
        {
            _artistServiceMock.Setup(mock => mock.GetById(ValidArtistGuid)).Returns(_artistDto1);
            OkObjectResult? result = _artistController.GetById(ValidArtistGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_artistDto1);
        }

        [Fact] public void Add_Test()
        {
            _artistServiceMock.Setup(mock => mock.Add(_artistDto1)).Returns(_artistDto1);
            CreatedAtActionResult? result = _artistController.Add(_artistDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status201Created);
            result?.Value.Should().Be(_artistDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            ArtistDto updatedArtistDto = GetArtistDto(_artistDto2, true);
            _artistServiceMock.Setup(mock => mock.UpdateById(_artistDto2, ValidArtistGuid)).Returns(updatedArtistDto);
            OkObjectResult? result = _artistController.UpdateById(_artistDto2, ValidArtistGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(updatedArtistDto);
        }

        [Fact] public void DeleteById_Test()
        {
            ArtistDto deletedArtistDto = GetArtistDto(_artistDto1, false);
            _artistServiceMock.Setup(mock => mock.DeleteById(ValidArtistGuid)).Returns(deletedArtistDto);
            OkObjectResult? result = _artistController.DeleteById(ValidArtistGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(deletedArtistDto);
        }

        private ArtistDto GetArtistDto(ArtistDto artistDto, bool isActive) => new ArtistDto
        {
            Id = ValidArtistGuid,
            Name = artistDto.Name,
            ImageUrl = artistDto.ImageUrl,
            Bio = artistDto.Bio,
            SimilarArtistsIds = artistDto.SimilarArtistsIds,
            CreatedAt = artistDto.CreatedAt,
            UpdatedAt = artistDto.UpdatedAt,
            DeletedAt = artistDto.DeletedAt
        };
    }
}