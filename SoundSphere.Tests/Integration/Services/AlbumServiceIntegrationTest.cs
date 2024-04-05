using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Services;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Integration.Services
{
    public class AlbumServiceIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly IMapper _mapper;

        private readonly Album _album1 = AlbumMock.GetMockedAlbum1();
        private readonly Album _album2 = AlbumMock.GetMockedAlbum2();
        private readonly IList<Album> _albums = AlbumMock.GetMockedAlbums();
        private readonly AlbumDto _albumDto1 = AlbumMock.GetMockedAlbumDto1();
        private readonly AlbumDto _albumDto2 = AlbumMock.GetMockedAlbumDto2();
        private readonly IList<AlbumDto> _albumDtos = AlbumMock.GetMockedAlbumDtos();

        public AlbumServiceIntegrationTest(DbFixture fixture)
        {
            _fixture = fixture;
            _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Album, AlbumDto>();
                config.CreateMap<AlbumDto, Album>();
            }).CreateMapper();
        }

        private void Execute(Action<AlbumService, SoundSphereContext> action)
        {
            using var context = _fixture.CreateContext();
            var albumService = new AlbumService(new AlbumRepository(context), _mapper);
            using var transaction = context.Database.BeginTransaction();
            context.AddRange(_albums);
            context.SaveChanges();
            action(albumService, context);
        }

        [Fact] public void FindAll_Test() => Execute((albumService, context) => albumService.FindAll().Should().BeEquivalentTo(_albumDtos));

        [Fact] public void FindById_Test() => Execute((albumService, context) => albumService.FindById(Constants.ValidAlbumGuid).Should().BeEquivalentTo(_albumDto1));

        [Fact] public void Save_Test() => Execute((albumService, context) =>
        {
            AlbumDto newAlbumDto = AlbumMock.GetMockedAlbumDto3();
            albumService.Save(newAlbumDto);
            context.Albums.Find(newAlbumDto.Id).Should().BeEquivalentTo(AlbumMock.GetMockedAlbum3());
        });

        [Fact] public void UpdateById_Test() => Execute((albumService, context) =>
        {
            Album updatedAlbum = CreateTestAlbum(_album2, _album1.IsActive);
            AlbumDto updatedAlbumDto = albumService.ConvertToDto(updatedAlbum);
            albumService.UpdateById(_albumDto2, Constants.ValidAlbumGuid);
            context.Albums.Find(Constants.ValidAlbumGuid).Should().BeEquivalentTo(updatedAlbum);
        });

        [Fact] public void DisableById_Test() => Execute((albumService, context) =>
        {
            Album disabledAlbum = CreateTestAlbum(_album1, false);
            AlbumDto disabledAlbumDto = albumService.ConvertToDto(disabledAlbum);
            albumService.DisableById(Constants.ValidAlbumGuid);
            context.Albums.Find(Constants.ValidAlbumGuid).Should().BeEquivalentTo(disabledAlbum);
        });

        private Album CreateTestAlbum(Album album, bool isActive) => new Album
        {
            Id = Constants.ValidAlbumGuid,
            Title = album.Title,
            ImageUrl = album.ImageUrl,
            ReleaseDate = album.ReleaseDate,
            SimilarAlbums = album.SimilarAlbums,
            IsActive = isActive
        };
    }
}