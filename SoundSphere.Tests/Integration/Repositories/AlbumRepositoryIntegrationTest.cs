using FluentAssertions;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class AlbumRepositoryIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        private readonly Album _album1 = AlbumMock.GetMockedAlbum1();
        private readonly Album _album2 = AlbumMock.GetMockedAlbum2();
        private readonly IList<Album> _albums = AlbumMock.GetMockedAlbums();
        private readonly IList<Album> _activeAlbums = AlbumMock.GetMockedActiveAlbums();

        public AlbumRepositoryIntegrationTest(DbFixture fixture) => _fixture = fixture;

        private void Execute(Action<AlbumRepository, SoundSphereContext> action)
        {
            using var context = _fixture.CreateContext();
            var albumRepository = new AlbumRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Albums.AddRange(_albums);
            context.SaveChanges();
            action(albumRepository, context);
        }

        [Fact] public void FindAll_Test() => Execute((albumRepository, context) => albumRepository.FindAll().Should().BeEquivalentTo(_albums));

        [Fact] public void FindAllActive_Test() => Execute((albumRepository, context) => albumRepository.FindAllActive().Should().BeEquivalentTo(_activeAlbums));

        [Fact] public void FindById_ValidId_Test() => Execute((albumRepository, context) => albumRepository.FindById(Constants.ValidAlbumGuid).Should().BeEquivalentTo(_album1));

        [Fact] public void FindById_InvalidId_Test() => Execute((albumRepository, context) => 
            albumRepository.Invoking(repository => repository.FindById(Constants.InvalidGuid))
                           .Should().Throw<ResourceNotFoundException>()
                           .WithMessage($"Album with id {Constants.InvalidGuid} not found!"));

        [Fact] public void Save_Test() => Execute((albumRepository, context) =>
        {
            Album newAlbum = AlbumMock.GetMockedAlbum3();
            albumRepository.Save(newAlbum);
            context.Albums.Find(newAlbum.Id).Should().BeEquivalentTo(newAlbum);
        });

        [Fact] public void UpdateById_ValidId_Test() => Execute((albumRepository, context) =>
        {
            Album updatedAlbum = CreateTestAlbum(_album2, _album1.IsActive);
            albumRepository.UpdateById(_album2, Constants.ValidAlbumGuid);
            context.Albums.Find(Constants.ValidAlbumGuid).Should().BeEquivalentTo(updatedAlbum);
        });

        [Fact] public void UpdateById_InvalidId_Test() => Execute((albumRepository, context) =>
            albumRepository.Invoking(repository => repository.UpdateById(_album2, Constants.InvalidGuid))
                           .Should().Throw<ResourceNotFoundException>()
                           .WithMessage($"Album with id {Constants.InvalidGuid} not found!"));

        [Fact] public void DisableById_ValidId_Test() => Execute((albumRepository, context) =>
        {
            Album disabledAlbum = CreateTestAlbum(_album1, false);
            albumRepository.DisableById(Constants.ValidAlbumGuid);
            context.Albums.Find(Constants.ValidAlbumGuid).Should().BeEquivalentTo(disabledAlbum);
        });

        [Fact] public void DisableById_InvalidId_Test() => Execute((albumRepository, context) =>
            albumRepository.Invoking(repository => repository.DisableById(Constants.InvalidGuid))
                           .Should().Throw<ResourceNotFoundException>()
                           .WithMessage($"Album with id {Constants.InvalidGuid} not found!"));

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