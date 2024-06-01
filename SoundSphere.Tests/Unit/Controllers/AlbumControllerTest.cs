﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoundSphere.Api.Controllers;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.AlbumMock;

namespace SoundSphere.Tests.Unit.Controllers
{
    public class AlbumControllerTest
    {
        private readonly Mock<IAlbumService> _albumServiceMock = new();
        private readonly AlbumController _albumController;

        private readonly AlbumDto _albumDto1 = GetMockedAlbumDto1();
        private readonly AlbumDto _albumDto2 = GetMockedAlbumDto2();
        private readonly IList<AlbumDto> _albumDtos = GetMockedAlbumDtos();
        private readonly IList<AlbumDto> _activeAlbumDtos = GetMockedActiveAlbumDtos();
        private readonly IList<AlbumDto> _paginatedAlbumDtos = GetMockedPaginatedAlbumDtos();
        private readonly IList<AlbumDto> _activePaginatedAlbumDtos = GetMockedActivePaginatedAlbumDtos();
        private readonly AlbumPaginationRequest _paginationRequest = GetMockedAlbumsPaginationRequest();

        public AlbumControllerTest() => _albumController = new(_albumServiceMock.Object);

        [Fact] public void GetAll_Test()
        {
            _albumServiceMock.Setup(mock => mock.GetAll()).Returns(_albumDtos);
            OkObjectResult? result = _albumController.GetAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_albumDtos);
        }

        [Fact] public void GetAllActive_Test()
        {
            _albumServiceMock.Setup(mock => mock.GetAllActive()).Returns(_activeAlbumDtos);
            OkObjectResult? result = _albumController.GetAllActive() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_activeAlbumDtos);
        }

        [Fact] public void GetAllPagination_Test()
        {
            _albumServiceMock.Setup(mock => mock.GetAllPagination(_paginationRequest)).Returns(_paginatedAlbumDtos);
            OkObjectResult? result = _albumController.GetAllPagination(_paginationRequest) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_paginatedAlbumDtos);
        }

        [Fact] public void GetAllActivePagination_Test()
        {
            _albumServiceMock.Setup(mock => mock.GetAllActivePagination(_paginationRequest)).Returns(_activePaginatedAlbumDtos);
            OkObjectResult? result = _albumController.GetAllActivePagination(_paginationRequest) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_activePaginatedAlbumDtos);
        }

        [Fact] public void GetById_Test()
        {
            _albumServiceMock.Setup(mock => mock.GetById(ValidAlbumGuid)).Returns(_albumDto1);
            OkObjectResult? result = _albumController.GetById(ValidAlbumGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(_albumDto1);
        }

        [Fact] public void Add_Test()
        {
            _albumServiceMock.Setup(mock => mock.Add(_albumDto1)).Returns(_albumDto1);
            CreatedAtActionResult? result = _albumController.Add(_albumDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status201Created);
            result?.Value.Should().Be(_albumDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            AlbumDto updatedAlbumDto = GetAlbumDto(_albumDto2, _albumDto1.IsActive);
            _albumServiceMock.Setup(mock => mock.UpdateById(_albumDto2, ValidAlbumGuid)).Returns(updatedAlbumDto);
            OkObjectResult? result = _albumController.UpdateById(_albumDto2, ValidAlbumGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(updatedAlbumDto);
        }

        [Fact] public void DeleteById_Test()
        {
            AlbumDto deletedAlbumDto = GetAlbumDto(_albumDto1, false);
            _albumServiceMock.Setup(mock => mock.DeleteById(ValidAlbumGuid)).Returns(deletedAlbumDto);
            OkObjectResult? result = _albumController.DeleteById(ValidAlbumGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(Status200OK);
            result?.Value.Should().Be(deletedAlbumDto);
        }

        private AlbumDto GetAlbumDto(AlbumDto albumDto, bool isActive) => new AlbumDto
        {
            Id = ValidAlbumGuid,
            Title = albumDto.Title,
            ImageUrl = albumDto.ImageUrl,
            ReleaseDate = albumDto.ReleaseDate,
            SimilarAlbumsIds = albumDto.SimilarAlbumsIds,
            IsActive = isActive
        };
    }
}