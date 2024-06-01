using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoundSphere.Api.Controllers;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.SongMock;

namespace SoundSphere.Tests.Unit.Controllers
{
    public class SongControllerTest
    {
        private readonly Mock<ISongService> _songServiceMock = new();
        private readonly SongController _songController;

        private readonly SongDto _songDto1 = GetMockedSongDto1();
        private readonly SongDto _songDto2 = GetMockedSongDto2();
        private readonly IList<SongDto> _songDtos = GetMockedSongDtos();
        private readonly IList<SongDto> _activeSongDtos = GetMockedActiveSongDtos();
        private readonly IList<SongDto> _paginatedSongDtos = GetMockedPaginatedSongDtos();
        private readonly IList<SongDto> _activePaginatedSongDtos = GetMockedActivePaginatedSongDtos();
        private readonly SongPaginationRequest _paginationRequest = GetMockedSongsPaginationRequest();

        public SongControllerTest() => _songController = new(_songServiceMock.Object);

        [Fact] public void GetAll_Test()
        {
            _songServiceMock.Setup(mock => mock.GetAll()).Returns(_songDtos);
            OkObjectResult? result = _songController.GetAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_songDtos);
        }

        [Fact] public void GetAllActive_Test()
        {
            _songServiceMock.Setup(mock => mock.GetAllActive()).Returns(_activeSongDtos);
            OkObjectResult? result = _songController.GetAllActive() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_activeSongDtos);
        }

        [Fact] public void GetAllPagination_Test()
        {
            _songServiceMock.Setup(mock => mock.GetAllPagination(_paginationRequest)).Returns(_paginatedSongDtos);
            OkObjectResult? result = _songController.GetAllPagination(_paginationRequest) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_paginatedSongDtos);
        }

        [Fact] public void GetAllActivePagination_Test()
        {
            _songServiceMock.Setup(mock => mock.GetAllActivePagination(_paginationRequest)).Returns(_activePaginatedSongDtos);
            OkObjectResult? result = _songController.GetAllActivePagination(_paginationRequest) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_activePaginatedSongDtos);
        }

        [Fact] public void GetById_Test()
        {
            _songServiceMock.Setup(mock => mock.GetById(ValidSongGuid)).Returns(_songDto1);
            OkObjectResult? result = _songController.GetById(ValidSongGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_songDto1);
        }

        [Fact] public void Add_Test()
        {
            _songServiceMock.Setup(mock => mock.Add(_songDto1)).Returns(_songDto1);
            CreatedAtActionResult? result = _songController.Add(_songDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status201Created);
            result?.Value.Should().Be(_songDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            SongDto updatedSongDto = GetSongDto(_songDto2, _songDto1.IsActive);
            _songServiceMock.Setup(mock => mock.UpdateById(_songDto2, ValidSongGuid)).Returns(updatedSongDto);
            OkObjectResult? result = _songController.UpdateById(_songDto2, ValidSongGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(updatedSongDto);
        }

        [Fact] public void DeleteById_Test()
        {
            SongDto deletedSongDto = GetSongDto(_songDto1, false);
            _songServiceMock.Setup(mock => mock.DeleteById(ValidSongGuid)).Returns(deletedSongDto);
            OkObjectResult? result = _songController.DeleteById(ValidSongGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(deletedSongDto);
        }

        private SongDto GetSongDto(SongDto songDto, bool IsActive) => new SongDto
        {
            Id = ValidSongGuid,
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