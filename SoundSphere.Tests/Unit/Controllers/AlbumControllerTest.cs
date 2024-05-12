using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoundSphere.Api.Controllers;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Controllers
{
    public class AlbumControllerTest
    {
        private readonly Mock<IAlbumService> _albumServiceMock = new();
        private readonly AlbumController _albumController;

        private readonly AlbumDto _albumDto1 = AlbumMock.GetMockedAlbumDto1();
        private readonly AlbumDto _albumDto2 = AlbumMock.GetMockedAlbumDto2();
        private readonly IList<AlbumDto> _albumDtos = AlbumMock.GetMockedAlbumDtos();
        private readonly IList<AlbumDto> _activeAlbumDtos = AlbumMock.GetMockedActiveAlbumDtos();
        private readonly IList<AlbumDto> _paginatedAlbumDtos = AlbumMock.GetMockedPaginatedAlbumDtos();
        private readonly IList<AlbumDto> _activePaginatedAlbumDtos = AlbumMock.GetMockedActivePaginatedAlbumDtos();
        private readonly AlbumPaginationRequest _paginationRequest = AlbumMock.GetMockedPaginationRequest();

        public AlbumControllerTest() => _albumController = new(_albumServiceMock.Object);

        [Fact] public void FindAll_Test()
        {
            _albumServiceMock.Setup(mock => mock.FindAll()).Returns(_albumDtos);
            OkObjectResult? result = _albumController.FindAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_albumDtos);
        }

        [Fact] public void FindAllActive_Test()
        {
            _albumServiceMock.Setup(mock => mock.FindAllActive()).Returns(_activeAlbumDtos);
            OkObjectResult? result = _albumController.FindAllActive() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_activeAlbumDtos);
        }

        [Fact] public void FindAllPagination_Test()
        {
            _albumServiceMock.Setup(mock => mock.FindAllPagination(_paginationRequest)).Returns(_paginatedAlbumDtos);
            OkObjectResult? result = _albumController.FindAllPagination(_paginationRequest) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_paginatedAlbumDtos);
        }

        [Fact] public void FindAllActivePagination_Test()
        {
            _albumServiceMock.Setup(mock => mock.FindAllActivePagination(_paginationRequest)).Returns(_activePaginatedAlbumDtos);
            OkObjectResult? result = _albumController.FindAllActivePagination(_paginationRequest) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_activePaginatedAlbumDtos);
        }

        [Fact] public void FindById_Test()
        {
            _albumServiceMock.Setup(mock => mock.FindById(Constants.ValidAlbumGuid)).Returns(_albumDto1);
            OkObjectResult? result = _albumController.FindById(Constants.ValidAlbumGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_albumDto1);
        }

        [Fact] public void Save_Test()
        {
            _albumServiceMock.Setup(mock => mock.Save(_albumDto1)).Returns(_albumDto1);
            CreatedAtActionResult? result = _albumController.Save(_albumDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status201Created);
            result?.Value.Should().Be(_albumDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            AlbumDto updatedAlbumDto = GetAlbumDto(_albumDto2, _albumDto1.IsActive);
            _albumServiceMock.Setup(mock => mock.UpdateById(_albumDto2, Constants.ValidAlbumGuid)).Returns(updatedAlbumDto);
            OkObjectResult? result = _albumController.UpdateById(_albumDto2, Constants.ValidAlbumGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(updatedAlbumDto);
        }

        [Fact] public void DisableById_Test()
        {
            AlbumDto disabledAlbumDto = GetAlbumDto(_albumDto1, false);
            _albumServiceMock.Setup(mock => mock.DisableById(Constants.ValidAlbumGuid)).Returns(disabledAlbumDto);
            OkObjectResult? result = _albumController.DisableById(Constants.ValidAlbumGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(disabledAlbumDto);
        }

        private AlbumDto GetAlbumDto(AlbumDto albumDto, bool isActive) => new AlbumDto
        {
            Id = Constants.ValidAlbumGuid,
            Title = albumDto.Title,
            ImageUrl = albumDto.ImageUrl,
            ReleaseDate = albumDto.ReleaseDate,
            SimilarAlbumsIds = albumDto.SimilarAlbumsIds,
            IsActive = isActive
        };
    }
}