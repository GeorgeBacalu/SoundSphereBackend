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
    public class AlbumControllerTest
    {
        private readonly Mock<IAlbumService> _albumService = new();
        private readonly AlbumController _albumController;

        private readonly AlbumDto _albumDto1 = AlbumMock.GetMockedAlbumDto1();
        private readonly AlbumDto _albumDto2 = AlbumMock.GetMockedAlbumDto2();
        private readonly IList<AlbumDto> _albumDtos = AlbumMock.GetMockedAlbumDtos();

        public AlbumControllerTest() => _albumController = new(_albumService.Object);

        [Fact] public void FindAll_Test()
        {
            _albumService.Setup(mock => mock.FindAll()).Returns(_albumDtos);
            var result = _albumController.FindAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_albumDtos);
        }

        [Fact] public void FindById_Test()
        {
            _albumService.Setup(mock => mock.FindById(Constants.ValidAlbumGuid)).Returns(_albumDto1);
            var result = _albumController.FindById(Constants.ValidAlbumGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_albumDto1);
        }

        [Fact] public void Save_Test()
        {
            _albumService.Setup(mock => mock.Save(_albumDto1)).Returns(_albumDto1);
            var result = _albumController.Save(_albumDto1) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status201Created);
            result?.Value.Should().BeEquivalentTo(_albumDto1);
        }

        [Fact] public void UpdateById_Test()
        {
            AlbumDto updatedAlbumDto = CreateTestAlbumDto(_albumDto2, _albumDto1.IsActive);
            _albumService.Setup(mock => mock.UpdateById(_albumDto2, Constants.ValidAlbumGuid)).Returns(updatedAlbumDto);
            var result = _albumController.UpdateById(_albumDto2, Constants.ValidAlbumGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(updatedAlbumDto);
        }

        [Fact] public void DisableById_Test()
        {
            AlbumDto disabledAlbumDto = CreateTestAlbumDto(_albumDto1, false);
            _albumService.Setup(mock => mock.DisableById(Constants.ValidAlbumGuid)).Returns(disabledAlbumDto);
            var result = _albumController.DisableById(Constants.ValidAlbumGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(disabledAlbumDto);
        }

        private AlbumDto CreateTestAlbumDto(AlbumDto albumDto, bool isActive) => new AlbumDto
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