using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoundSphere.Api.Controllers;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.PlaylistMock;

namespace SoundSphere.Tests.Unit.Controllers
{
    public class PlaylistControllerTest
    {
        private readonly Mock<IPlaylistService> _playlistServiceMock = new();
        private readonly PlaylistController _playlistController;

        private readonly PlaylistDto _playlistDto1 = GetMockedPlaylistDto1();
        private readonly PlaylistDto _playlistDto2 = GetMockedPlaylistDto2();
        private readonly IList<PlaylistDto> _playlistDtos = GetMockedPlaylistDtos();
        private readonly IList<PlaylistDto> _paginatedPlaylistDtos = GetMockedPaginatedPlaylistDtos();
        private readonly PlaylistPaginationRequest _paginationRequest = GetMockedPlaylistsPaginationRequest();

        public PlaylistControllerTest() => _playlistController = new(_playlistServiceMock.Object);

        [Fact] public void GetAllActivePagination_Test()
        {
            _playlistServiceMock.Setup(mock => mock.GetAll(_paginationRequest, ValidUserGuid)).Returns(_paginatedPlaylistDtos);
            OkObjectResult? result = _playlistController.GetAll(_paginationRequest) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_paginatedPlaylistDtos);
        }

        [Fact] public void GetById_Test()
        {
            _playlistServiceMock.Setup(mock => mock.GetById(ValidPlaylistGuid, ValidUserGuid)).Returns(_playlistDto1);
            OkObjectResult? result = _playlistController.GetById(ValidPlaylistGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_playlistDto1);
        }

        [Fact] public void Add_Test()
        {
            _playlistServiceMock.Setup(mock => mock.Add(_playlistDto1, ValidUserGuid)).Returns(_playlistDto1);
            CreatedAtActionResult? result = _playlistController.Add(_playlistDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status201Created);
            result?.Value.Should().Be(_playlistDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            PlaylistDto updatedPlaylistDto = new PlaylistDto
            {
                Id = ValidPlaylistGuid,
                Title = _playlistDto2.Title,
                UserId = _playlistDto1.UserId,
                SongsIds = _playlistDto1.SongsIds,
                CreatedAt = _playlistDto1.CreatedAt
            };
            _playlistServiceMock.Setup(mock => mock.UpdateById(_playlistDto2, ValidPlaylistGuid, ValidUserGuid2)).Returns(updatedPlaylistDto);
            OkObjectResult? result = _playlistController.UpdateById(_playlistDto2, ValidPlaylistGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(updatedPlaylistDto);
        }

        [Fact] public void DeleteById_Test()
        {
            PlaylistDto deletedPlaylistDto = new PlaylistDto
            {
                Id = ValidPlaylistGuid,
                Title = _playlistDto1.Title,
                UserId = _playlistDto1.UserId,
                SongsIds = _playlistDto1.SongsIds,
                CreatedAt = _playlistDto1.CreatedAt
            };
            _playlistServiceMock.Setup(mock => mock.DeleteById(ValidPlaylistGuid, ValidUserGuid)).Returns(deletedPlaylistDto);
            OkObjectResult? result = _playlistController.DeleteById(ValidPlaylistGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(deletedPlaylistDto);
        }
    }
}