using FluentAssertions;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.AlbumMock;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class AlbumRepositoryIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        private readonly Album _album1 = GetMockedAlbum1();
        private readonly Album _album2 = GetMockedAlbum2();
        private readonly IList<Album> _albums = GetMockedAlbums();
        private readonly IList<Album> _activeAlbums = GetMockedActiveAlbums();
        private readonly IList<Album> _paginedAlbums = GetMockedPaginatedAlbums();
        private readonly IList<Album> _activePaginatedAlbums = GetMockedActivePaginatedAlbums();
        private readonly AlbumPaginationRequest _paginationRequest = GetMockedAlbumsPaginationRequest();

        public AlbumRepositoryIntegrationTest(DbFixture fixture) => _fixture = fixture;

        private void Execute(Action<AlbumRepository, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var albumRepository = new AlbumRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Albums.AddRange(_albums);
            context.SaveChanges();
            action(albumRepository, context);
            transaction.Rollback();
        }

        [Fact] public void GetAll_Test() => Execute((albumRepository, context) => albumRepository.GetAll().Should().BeEquivalentTo(_albums));

        [Fact] public void GetAllActive_Test() => Execute((albumRepository, context) => albumRepository.GetAllActive().Should().BeEquivalentTo(_activeAlbums));

        [Fact] public void GetAllPagination_Test() => Execute((albumRepository, context) => albumRepository.GetAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginedAlbums));

        [Fact] public void GetAllActivePagination_Test() => Execute((albumRepository, context) => albumRepository.GetAllActivePagination(_paginationRequest).Should().BeEquivalentTo(_activePaginatedAlbums));

        [Fact] public void GetById_ValidId_Test() => Execute((albumRepository, context) => albumRepository.GetById(ValidAlbumGuid).Should().Be(_album1));

        [Fact] public void GetById_InvalidId_Test() => Execute((albumRepository, context) => albumRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(AlbumNotFound, InvalidGuid)));

        [Fact] public void Add_Test() => Execute((albumRepository, context) =>
        {
            Album newAlbum = GetMockedAlbum51();
            albumRepository.Add(newAlbum);
            context.Albums.Find(newAlbum.Id).Should().Be(newAlbum);
        });

        [Fact] public void UpdateById_ValidId_Test() => Execute((albumRepository, context) =>
        {
            Album updatedAlbum = GetAlbum(_album2, _album1.IsActive);
            albumRepository.UpdateById(_album2, ValidAlbumGuid);
            context.Albums.Find(ValidAlbumGuid).Should().Be(updatedAlbum);
        });

        [Fact] public void UpdateById_InvalidId_Test() => Execute((albumRepository, context) => albumRepository
            .Invoking(repository => repository.UpdateById(_album2, InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(AlbumNotFound, InvalidGuid)));

        [Fact] public void DeleteById_ValidId_Test() => Execute((albumRepository, context) =>
        {
            Album deletedAlbum = GetAlbum(_album1, false);
            albumRepository.DeleteById(ValidAlbumGuid);
            context.Albums.Find(ValidAlbumGuid).Should().Be(deletedAlbum);
        });

        [Fact] public void DeleteById_InvalidId_Test() => Execute((albumRepository, context) => albumRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(AlbumNotFound, InvalidGuid)));

        private Album GetAlbum(Album album, bool isActive) => new Album
        {
            Id = ValidAlbumGuid,
            Title = album.Title,
            ImageUrl = album.ImageUrl,
            ReleaseDate = album.ReleaseDate,
            SimilarAlbums = album.SimilarAlbums,
            IsActive = isActive
        };
    }
}