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
    public class ArtistServiceIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly IMapper _mapper;

        private readonly Artist _artist1 = ArtistMock.GetMockedArtist1();
        private readonly Artist _artist2 = ArtistMock.GetMockedArtist2();
        private readonly IList<Artist> _artists = ArtistMock.GetMockedArtists();
        private readonly ArtistDto _artistDto1 = ArtistMock.GetMockedArtistDto1();
        private readonly ArtistDto _artistDto2 = ArtistMock.GetMockedArtistDto2();
        private readonly IList<ArtistDto> _artistDtos = ArtistMock.GetMockedArtistDtos();

        public ArtistServiceIntegrationTest(DbFixture fixture)
        {
            _fixture = fixture;
            _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Artist, ArtistDto>();
                config.CreateMap<ArtistDto, Artist>();
            }).CreateMapper();
        }

        private void Execute(Action<ArtistService, SoundSphereContext> action)
        {
            using var context = _fixture.CreateContext();
            var artistService = new ArtistService(new ArtistRepository(context), _mapper);
            using var transaction = context.Database.BeginTransaction();
            context.AddRange(_artists);
            context.SaveChanges();
            action(artistService, context);
        }

        [Fact] public void FindAll_Test() => Execute((artistService, context) => artistService.FindAll().Should().BeEquivalentTo(_artistDtos));

        [Fact] public void FindById_Test() => Execute((artistService, context) => artistService.FindById(Constants.ValidArtistGuid).Should().BeEquivalentTo(_artistDto1));

        [Fact] public void Save_Test() => Execute((artistService, context) =>
        {
            ArtistDto newArtistDto = ArtistMock.GetMockedArtistDto3();
            artistService.Save(newArtistDto);
            context.Artists.Find(newArtistDto.Id).Should().BeEquivalentTo(ArtistMock.GetMockedArtist3());
        });

        [Fact] public void UpdateById_Test() => Execute((artistService, context) =>
        {
            Artist updatedArtist = CreateTestArtist(_artist2, _artist1.IsActive);
            ArtistDto updatedArtistDto = artistService.ConvertToDto(updatedArtist);
            artistService.UpdateById(_artistDto2, Constants.ValidArtistGuid);
            context.Artists.Find(Constants.ValidArtistGuid).Should().BeEquivalentTo(updatedArtist);
        });

        [Fact] public void DisableById_Test() => Execute((artistService, context) =>
        {
            Artist disabledArtist = CreateTestArtist(_artist1, false);
            ArtistDto disabledArtistDto = artistService.ConvertToDto(disabledArtist);
            artistService.DisableById(Constants.ValidArtistGuid);
            context.Artists.Find(Constants.ValidArtistGuid).Should().BeEquivalentTo(disabledArtist);
        });

        private Artist CreateTestArtist(Artist artist, bool isActive) => new Artist
        {
            Id = Constants.ValidArtistGuid,
            Name = artist.Name,
            ImageUrl = artist.ImageUrl,
            Bio = artist.Bio,
            SimilarArtists = artist.SimilarArtists,
            IsActive = isActive
        };
    }
}