using FluentAssertions;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class ArtistRepositoryIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        private readonly Artist _artist1 = ArtistMock.GetMockedArtist1();
        private readonly Artist _artist2 = ArtistMock.GetMockedArtist2();
        private readonly IList<Artist> _artists = ArtistMock.GetMockedArtists();
        private readonly IList<Artist> _activeArtists = ArtistMock.GetMockedActiveArtists();

        public ArtistRepositoryIntegrationTest(DbFixture fixture) => _fixture = fixture;

        private void Execute(Action<ArtistRepository, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var artistRepository = new ArtistRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Artists.AddRange(_artists);
            context.SaveChanges();
            action(artistRepository, context);
        }

        [Fact] public void FindAll_Test() => Execute((artistRepository, context) => artistRepository.FindAll().Should().BeEquivalentTo(_artists));

        [Fact] public void FindAllActive_Test() => Execute((artistRepository, context) => artistRepository.FindAllActive().Should().BeEquivalentTo(_activeArtists));

        [Fact] public void FindById_ValidId_Test() => Execute((artistRepository, context) => artistRepository.FindById(Constants.ValidArtistGuid).Should().Be(_artist1));

        [Fact] public void FindById_InvalidId_Test() => Execute((artistRepository, context) => artistRepository
            .Invoking(repository => repository.FindById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.ArtistNotFound, Constants.InvalidGuid)));

        [Fact] public void Save_Test() => Execute((artistRepository, context) =>
        {
            Artist newArtist = ArtistMock.GetMockedArtist3();
            artistRepository.Save(newArtist);
            context.Artists.Find(newArtist.Id).Should().Be(newArtist);
        });

        [Fact] public void UpdateById_ValidId_Test() => Execute((artistRepository, context) =>
        {
            Artist updatedArtist = CreateTestArtist(_artist2, _artist1.IsActive);
            artistRepository.UpdateById(_artist2, Constants.ValidArtistGuid);
            context.Artists.Find(Constants.ValidArtistGuid).Should().Be(updatedArtist);
        });

        [Fact] public void UpdateById_InvalidId_Test() => Execute((artistRepository, context) => artistRepository
            .Invoking(repository => repository.UpdateById(_artist2, Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.ArtistNotFound, Constants.InvalidGuid)));

        [Fact] public void DisableById_ValidId_Test() => Execute((artistRepository, context) =>
        {
            Artist disabledArtist = CreateTestArtist(_artist1, false);
            artistRepository.DisableById(Constants.ValidArtistGuid);
            context.Artists.Find(Constants.ValidArtistGuid).Should().Be(disabledArtist);
        });

        [Fact] public void DisableById_InvalidId_Test() => Execute((artistRepository, context) => artistRepository
            .Invoking(repository => repository.DisableById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.ArtistNotFound, Constants.InvalidGuid)));

        private Artist CreateTestArtist(Artist artist, bool isActive) => new Artist
        {
            Id = Constants.ValidArtistGuid,
            Name = artist.Name,
            ImageUrl = artist.ImageUrl,
            Bio = artist.Bio,
            SimilarArtists = artist.SimilarArtists,
            IsActive = isActive,
        };
    }
}