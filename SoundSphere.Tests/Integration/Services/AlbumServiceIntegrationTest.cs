using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.AlbumMock;

namespace SoundSphere.Tests.Integration.Services
{
    public class AlbumServiceIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly IMapper _mapper;

        private readonly Album _album1 = GetMockedAlbum1();
        private readonly Album _album2 = GetMockedAlbum2();
        private readonly IList<Album> _albums = GetMockedAlbums();
        private readonly AlbumDto _albumDto1 = GetMockedAlbumDto1();
        private readonly AlbumDto _albumDto2 = GetMockedAlbumDto2();
        private readonly IList<AlbumDto> _albumDtos = GetMockedAlbumDtos();
        private readonly IList<AlbumDto> _paginatedAlbumDtos = GetMockedPaginatedAlbumDtos();
        private readonly AlbumPaginationRequest _paginationRequest = GetMockedAlbumsPaginationRequest();

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

        [Fact] public void GetAll_Test() => Execute((albumService, context) => albumService.GetAll(_paginationRequest).Should().BeEquivalentTo(_paginatedAlbumDtos));
        
        [Fact] public void GetById_Test() => Execute((albumService, context) => albumService.GetById(ValidAlbumGuid).Should().Be(_albumDto1));

        [Fact] public void Add_Test() => Execute((albumService, context) =>
        {
            AlbumDto newAlbumDto = GetMockedAlbumDto51();
            AlbumDto result = albumService.Add(newAlbumDto);
            context.Albums.Find(newAlbumDto.Id).Should().Be(newAlbumDto);
            result.Should().Be(newAlbumDto);
        });

        [Fact] public void UpdateById_Test() => Execute((albumService, context) =>
        {
            Album updatedAlbum = GetAlbum(_album2, true);
            AlbumDto updatedAlbumDto = updatedAlbum.ToDto(_mapper);
            AlbumDto result = albumService.UpdateById(_albumDto2, ValidAlbumGuid);
            context.Albums.Find(ValidAlbumGuid).Should().Be(updatedAlbum);
            result.Should().Be(updatedAlbumDto);
        });

        [Fact] public void DeleteById_Test() => Execute((albumService, context) =>
        {
            Album deletedAlbum = GetAlbum(_album1, false);
            AlbumDto deletedAlbumDto = deletedAlbum.ToDto(_mapper);
            AlbumDto result = albumService.DeleteById(ValidAlbumGuid);
            context.Albums.Find(ValidAlbumGuid).Should().Be(deletedAlbum);
            result.Should().Be(deletedAlbumDto);
        });

        private Album GetAlbum(Album album, bool isActive) => new Album
        {
            Id = ValidAlbumGuid,
            Title = album.Title,
            ImageUrl = album.ImageUrl,
            ReleaseDate = album.ReleaseDate,
            SimilarAlbums = album.SimilarAlbums,
            CreatedAt = album.CreatedAt,
            UpdatedAt = album.UpdatedAt,
            DeletedAt = album.DeletedAt
        };
    }
}