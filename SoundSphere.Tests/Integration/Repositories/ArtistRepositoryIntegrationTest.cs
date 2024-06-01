using FluentAssertions;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.ArtistMock;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class ArtistRepositoryIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        private readonly Artist _artist1 = GetMockedArtist1();
        private readonly Artist _artist2 = GetMockedArtist2();
        private readonly IList<Artist> _artists = GetMockedArtists();
        private readonly IList<Artist> _activeArtists = GetMockedActiveArtists();
        private readonly IList<Artist> _paginatedArtists = GetMockedPaginatedArtists();
        private readonly IList<Artist> _activePaginatedArtists = GetMockedActivePaginatedArtists();
        private readonly ArtistPaginationRequest _paginationRequest = GetMockedArtistsPaginationRequest();

        public ArtistRepositoryIntegrationTest(DbFixture fixture) => _fixture = fixture;

        private void Execute(Action<ArtistRepository, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var artistRepository = new ArtistRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Artists.AddRange(_artists);
            context.SaveChanges();
            action(artistRepository, context);
            transaction.Rollback();
        }

        [Fact] public void GetAll_Test() => Execute((artistRepository, context) => artistRepository.GetAll().Should().BeEquivalentTo(_artists));

        [Fact] public void GetAllActive_Test() => Execute((artistRepository, context) => artistRepository.GetAllActive().Should().BeEquivalentTo(_activeArtists));

        [Fact] public void GetAllPagination_Test() => Execute((artistRepository, context) => artistRepository.GetAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedArtists));

        [Fact] public void GetAllActivePagination_Test() => Execute((artistRepository, context) => artistRepository.GetAllActivePagination(_paginationRequest).Should().BeEquivalentTo(_activePaginatedArtists));
        
        [Fact] public void GetById_ValidId_Test() => Execute((artistRepository, context) => artistRepository.GetById(ValidArtistGuid).Should().Be(_artist1));

        [Fact] public void GetById_InvalidId_Test() => Execute((artistRepository, context) => artistRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(ArtistNotFound, InvalidGuid)));

        [Fact] public void Add_Test() => Execute((artistRepository, context) =>
        {
            Artist newArtist = GetMockedArtist51();
            artistRepository.Add(newArtist);
            context.Artists.Find(newArtist.Id).Should().Be(newArtist);
        });

        [Fact] public void UpdateById_ValidId_Test() => Execute((artistRepository, context) =>
        {
            Artist updatedArtist = GetArtist(_artist2, _artist1.IsActive);
            artistRepository.UpdateById(_artist2, ValidArtistGuid);
            context.Artists.Find(ValidArtistGuid).Should().Be(updatedArtist);
        });

        [Fact] public void UpdateById_InvalidId_Test() => Execute((artistRepository, context) => artistRepository
            .Invoking(repository => repository.UpdateById(_artist2, InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(ArtistNotFound, InvalidGuid)));

        [Fact] public void DeleteById_ValidId_Test() => Execute((artistRepository, context) =>
        {
            Artist deletedArtist = GetArtist(_artist1, false);
            artistRepository.DeleteById(ValidArtistGuid);
            context.Artists.Find(ValidArtistGuid).Should().Be(deletedArtist);
        });

        [Fact] public void DeleteById_InvalidId_Test() => Execute((artistRepository, context) => artistRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(ArtistNotFound, InvalidGuid)));

        private Artist GetArtist(Artist artist, bool isActive) => new Artist
        {
            Id = ValidArtistGuid,
            Name = artist.Name,
            ImageUrl = artist.ImageUrl,
            Bio = artist.Bio,
            SimilarArtists = artist.SimilarArtists,
            IsActive = isActive,
        };
    }
}