using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.ArtistMock;

namespace SoundSphere.Tests.Integration.Services
{
    public class ArtistServiceIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly IMapper _mapper;

        private readonly Artist _artist1 = GetMockedArtist1();
        private readonly Artist _artist2 = GetMockedArtist2();
        private readonly IList<Artist> _artists = GetMockedArtists();
        private readonly ArtistDto _artistDto1 = GetMockedArtistDto1();
        private readonly ArtistDto _artistDto2 = GetMockedArtistDto2();
        private readonly IList<ArtistDto> _artistDtos = GetMockedArtistDtos();
        private readonly IList<ArtistDto> _paginatedArtistDtos = GetMockedPaginatedArtistDtos();
        private readonly ArtistPaginationRequest _paginationRequest = GetMockedArtistsPaginationRequest();

        public ArtistServiceIntegrationTest(DbFixture fixture) => (_fixture, _mapper) = (fixture, new MapperConfiguration(config => { config.CreateMap<Artist, ArtistDto>(); config.CreateMap<ArtistDto, Artist>(); }).CreateMapper());

        private void Execute(Action<ArtistService, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var artistService = new ArtistService(new ArtistRepository(context), _mapper);
            using var transaction = context.Database.BeginTransaction();
            context.AddRange(_artists);
            context.SaveChanges();
            action(artistService, context);
            transaction.Rollback();
        }

        [Fact] public void GetAll_Test() => Execute((artistService, context) => artistService.GetAll(_paginationRequest).Should().BeEquivalentTo(_paginatedArtistDtos));
        
        [Fact] public void GetById_Test() => Execute((artistService, context) => artistService.GetById(ValidArtistGuid).Should().Be(_artistDto1));

        [Fact] public void Add_Test() => Execute((artistService, context) =>
        {
            ArtistDto newArtistDto = GetMockedArtistDto51();
            ArtistDto result = artistService.Add(newArtistDto);
            context.Artists.Find(newArtistDto.Id).Should().Be(newArtistDto);
            result.Should().Be(newArtistDto);
        });

        [Fact] public void UpdateById_Test() => Execute((artistService, context) =>
        {
            Artist updatedArtist = GetArtist(_artist2, true);
            ArtistDto updatedArtistDto = updatedArtist.ToDto(_mapper);
            ArtistDto result = artistService.UpdateById(_artistDto2, ValidArtistGuid);
            context.Artists.Find(ValidArtistGuid).Should().Be(updatedArtist);
            result.Should().Be(updatedArtistDto);
        });

        [Fact] public void DeleteById_Test() => Execute((artistService, context) =>
        {
            Artist deletedArtist = GetArtist(_artist1, false);
            ArtistDto deletedArtistDto = deletedArtist.ToDto(_mapper);
            ArtistDto result = artistService.DeleteById(ValidArtistGuid);
            context.Artists.Find(ValidArtistGuid).Should().Be(deletedArtist);
            result.Should().Be(deletedArtistDto);
        });

        private Artist GetArtist(Artist artist, bool isActive) => new Artist
        {
            Id = ValidArtistGuid,
            Name = artist.Name,
            ImageUrl = artist.ImageUrl,
            Bio = artist.Bio,
            SimilarArtists = artist.SimilarArtists,
            CreatedAt = artist.CreatedAt,
            UpdatedAt = artist.UpdatedAt,
            DeletedAt = artist.DeletedAt
        };
    }
}