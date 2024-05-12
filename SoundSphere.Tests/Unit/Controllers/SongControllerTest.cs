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
    public class SongControllerTest
    {
        private readonly Mock<ISongService> _songServiceMock = new();
        private readonly SongController _songController;

        private readonly SongDto _songDto1 = SongMock.GetMockedSongDto1();
        private readonly SongDto _songDto2 = SongMock.GetMockedSongDto2();
        private readonly IList<SongDto> _songDtos = SongMock.GetMockedSongDtos();
        private readonly IList<SongDto> _activeSongDtos = SongMock.GetMockedActiveSongDtos();
        private readonly IList<SongDto> _paginatedSongDtos = SongMock.GetMockedPaginatedSongDtos();
        private readonly IList<SongDto> _activePaginatedSongDtos = SongMock.GetMockedActivePaginatedSongDtos();
        private readonly SongPaginationRequest _paginationRequest = SongMock.GetMockedPaginationRequest();

        public SongControllerTest() => _songController = new(_songServiceMock.Object);

        [Fact] public void FindAll_Test()
        {
            _songServiceMock.Setup(mock => mock.FindAll()).Returns(_songDtos);
            OkObjectResult? result = _songController.FindAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_songDtos);
        }

        [Fact] public void FindAllActive_Test()
        {
            _songServiceMock.Setup(mock => mock.FindAllActive()).Returns(_activeSongDtos);
            OkObjectResult? result = _songController.FindAllActive() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_activeSongDtos);
        }

        [Fact] public void FindAllPagination_Test()
        {
            _songServiceMock.Setup(mock => mock.FindAllPagination(_paginationRequest)).Returns(_paginatedSongDtos);
            OkObjectResult? result = _songController.FindAllPagination(_paginationRequest) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_paginatedSongDtos);
        }

        [Fact] public void FindAllActivePagination_Test()
        {
            _songServiceMock.Setup(mock => mock.FindAllActivePagination(_paginationRequest)).Returns(_activePaginatedSongDtos);
            OkObjectResult? result = _songController.FindAllActivePagination(_paginationRequest) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_activePaginatedSongDtos);
        }

        [Fact] public void FindById_Test()
        {
            _songServiceMock.Setup(mock => mock.FindById(Constants.ValidSongGuid)).Returns(_songDto1);
            OkObjectResult? result = _songController.FindById(Constants.ValidSongGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_songDto1);
        }

        [Fact] public void Save_Test()
        {
            _songServiceMock.Setup(mock => mock.Save(_songDto1)).Returns(_songDto1);
            CreatedAtActionResult? result = _songController.Save(_songDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status201Created);
            result?.Value.Should().Be(_songDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            SongDto updatedSongDto = GetSongDto(_songDto2, _songDto1.IsActive);
            _songServiceMock.Setup(mock => mock.UpdateById(_songDto2, Constants.ValidSongGuid)).Returns(updatedSongDto);
            OkObjectResult? result = _songController.UpdateById(_songDto2, Constants.ValidSongGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(updatedSongDto);
        }

        [Fact] public void DisableById_Test()
        {
            SongDto disabledSongDto = GetSongDto(_songDto1, false);
            _songServiceMock.Setup(mock => mock.DisableById(Constants.ValidSongGuid)).Returns(disabledSongDto);
            OkObjectResult? result = _songController.DisableById(Constants.ValidSongGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(disabledSongDto);
        }

        private SongDto GetSongDto(SongDto songDto, bool IsActive) => new SongDto
        {
            Id = Constants.ValidSongGuid,
            Title = songDto.Title,
            ImageUrl = songDto.ImageUrl,
            Genre = songDto.Genre,
            ReleaseDate = songDto.ReleaseDate,
            DurationSeconds = songDto.DurationSeconds,
            AlbumId = songDto.AlbumId,
            ArtistsIds = songDto.ArtistsIds,
            SimilarSongsIds = songDto.SimilarSongsIds,
            IsActive = IsActive
        };
    }
}