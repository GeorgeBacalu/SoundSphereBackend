using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
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
        private readonly IList<AlbumDto> _activeAlbumDtos = AlbumMock.GetMockedActiveAlbumDtos();
        private readonly IList<AlbumDto> _paginatedAlbumDtos = AlbumMock.GetMockedPaginatedAlbumDtos();
        private readonly IList<AlbumDto> _activePaginatedAlbumDtos = AlbumMock.GetMockedActivePaginatedAlbumDtos();
        private readonly AlbumPaginationRequest _paginationRequest = AlbumMock.GetMockedPaginationRequest();

        public AlbumServiceIntegrationTest(DbFixture fixture) => (_fixture, _mapper) = (fixture, new MapperConfiguration(config => { config.CreateMap<Album, AlbumDto>(); config.CreateMap<AlbumDto, Album>(); }).CreateMapper());

        private void Execute(Action<AlbumService, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var albumService = new AlbumService(new AlbumRepository(context), _mapper);
            using var transaction = context.Database.BeginTransaction();
            context.AddRange(_albums);
            context.SaveChanges();
            action(albumService, context);
            transaction.Rollback();
        }

        [Fact] public void FindAll_Test() => Execute((albumService, context) => albumService.FindAll().Should().BeEquivalentTo(_albumDtos));

        [Fact] public void FindAllActive_Test() => Execute((albumService, context) => albumService.FindAllActive().Should().BeEquivalentTo(_activeAlbumDtos));

        [Fact] public void FindAllPagination_Test() => Execute((albumService, context) => albumService.FindAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedAlbumDtos));

        [Fact] public void FindAllActivePagination_Test() => Execute((albumService, context) => albumService.FindAllActivePagination(_paginationRequest).Should().BeEquivalentTo(_activePaginatedAlbumDtos));
        
        [Fact] public void FindById_Test() => Execute((albumService, context) => albumService.FindById(Constants.ValidAlbumGuid).Should().Be(_albumDto1));

        [Fact] public void Save_Test() => Execute((albumService, context) =>
        {
            AlbumDto newAlbumDto = AlbumMock.GetMockedAlbumDto51();
            albumService.Save(newAlbumDto);
            context.Albums.Find(newAlbumDto.Id).Should().Be(newAlbumDto);
        });

        [Fact] public void UpdateById_Test() => Execute((albumService, context) =>
        {
            Album updatedAlbum = GetAlbum(_album2, _album1.IsActive);
            AlbumDto updatedAlbumDto = updatedAlbum.ToDto(_mapper);
            albumService.UpdateById(_albumDto2, Constants.ValidAlbumGuid);
            context.Albums.Find(Constants.ValidAlbumGuid).Should().Be(updatedAlbum);
        });

        [Fact] public void DisableById_Test() => Execute((albumService, context) =>
        {
            Album disabledAlbum = GetAlbum(_album1, false);
            AlbumDto disabledAlbumDto = disabledAlbum.ToDto(_mapper);
            albumService.DisableById(Constants.ValidAlbumGuid);
            context.Albums.Find(Constants.ValidAlbumGuid).Should().Be(disabledAlbum);
        });

        private Album GetAlbum(Album album, bool isActive) => new Album
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