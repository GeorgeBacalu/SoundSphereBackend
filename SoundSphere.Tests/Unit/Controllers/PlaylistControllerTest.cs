﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoundSphere.Api.Controllers;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database;
using SoundSphere.Database.Dtos;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Controllers
{
    public class PlaylistControllerTest
    {
        private readonly Mock<IPlaylistService> _playlistServiceMock = new();
        private readonly PlaylistController _playlistController;

        private readonly PlaylistDto _playlistDto1 = PlaylistMock.GetMockedPlaylistDto1();
        private readonly PlaylistDto _playlistDto2 = PlaylistMock.GetMockedPlaylistDto2();
        private readonly IList<PlaylistDto> _playlistDtos = PlaylistMock.GetMockedPlaylistDtos();
        private readonly IList<PlaylistDto> _activePlaylistDtos = PlaylistMock.GetMockedActivePlaylistDtos();

        public PlaylistControllerTest() => _playlistController = new(_playlistServiceMock.Object);

        [Fact] public void FindAll_Test()
        {
            _playlistServiceMock.Setup(mock => mock.FindAll()).Returns(_playlistDtos);
            OkObjectResult? result = _playlistController.FindAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_playlistDtos);
        }

        [Fact] public void FindAllActive_Test()
        {
            _playlistServiceMock.Setup(mock => mock.FindAllActive()).Returns(_activePlaylistDtos);
            OkObjectResult? result = _playlistController.FindAllActive() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_activePlaylistDtos);
        }

        [Fact] public void FindById_Test()
        {
            _playlistServiceMock.Setup(mock => mock.FindById(Constants.ValidPlaylistGuid)).Returns(_playlistDto1);
            OkObjectResult? result = _playlistController.FindById(Constants.ValidPlaylistGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(_playlistDto1);
        }

        [Fact] public void Save_Test()
        {
            _playlistServiceMock.Setup(mock => mock.Save(_playlistDto1)).Returns(_playlistDto1);
            CreatedAtActionResult? result = _playlistController.Save(_playlistDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status201Created);
            result?.Value.Should().Be(_playlistDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            PlaylistDto updatedPlaylistDto = new PlaylistDto
            {
                Id = Constants.ValidPlaylistGuid,
                Title = _playlistDto2.Title,
                UserId = _playlistDto1.UserId,
                SongsIds = _playlistDto1.SongsIds,
                CreatedAt = _playlistDto1.CreatedAt,
                IsActive = _playlistDto1.IsActive
            };
            _playlistServiceMock.Setup(mock => mock.UpdateById(_playlistDto2, Constants.ValidPlaylistGuid)).Returns(updatedPlaylistDto);
            OkObjectResult? result = _playlistController.UpdateById(_playlistDto2, Constants.ValidPlaylistGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(updatedPlaylistDto);
        }

        [Fact] public void DisableById_Test()
        {
            PlaylistDto disabledPlaylistDto = new PlaylistDto
            {
                Id = Constants.ValidPlaylistGuid,
                Title = _playlistDto1.Title,
                UserId = _playlistDto1.UserId,
                SongsIds = _playlistDto1.SongsIds,
                CreatedAt = _playlistDto1.CreatedAt,
                IsActive = false
            };
            _playlistServiceMock.Setup(mock => mock.DisableById(Constants.ValidPlaylistGuid)).Returns(disabledPlaylistDto);
            OkObjectResult? result = _playlistController.DisableById(Constants.ValidPlaylistGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().Be(disabledPlaylistDto);
        }
    }
}