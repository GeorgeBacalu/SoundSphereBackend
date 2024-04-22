using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Repositories
{
    public class AlbumRepositoryTest
    {
        private readonly Mock<DbSet<Album>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IAlbumRepository _albumRepository;

        private readonly Album _album1 = AlbumMock.GetMockedAlbum1();
        private readonly Album _album2 = AlbumMock.GetMockedAlbum2();
        private readonly IList<Album> _albums = AlbumMock.GetMockedAlbums();
        private readonly IList<Album> _activeAlbums = AlbumMock.GetMockedActiveAlbums();

        public AlbumRepositoryTest()
        {
            IQueryable<Album> queryableAlbums = _albums.AsQueryable();
            _dbSetMock.As<IQueryable<Album>>().Setup(mock => mock.Provider).Returns(queryableAlbums.Provider);
            _dbSetMock.As<IQueryable<Album>>().Setup(mock => mock.Expression).Returns(queryableAlbums.Expression);
            _dbSetMock.As<IQueryable<Album>>().Setup(mock => mock.ElementType).Returns(queryableAlbums.ElementType);
            _dbSetMock.As<IQueryable<Album>>().Setup(mock => mock.GetEnumerator()).Returns(queryableAlbums.GetEnumerator());
            _dbContextMock.Setup(mock => mock.Albums).Returns(_dbSetMock.Object);
            _albumRepository = new AlbumRepository(_dbContextMock.Object);
        }

        [Fact] public void FindAll_Test() => _albumRepository.FindAll().Should().BeEquivalentTo(_albums);

        [Fact] public void FindAllActive_Test() => _albumRepository.FindAllActive().Should().BeEquivalentTo(_activeAlbums);

        [Fact] public void FindById_ValidId_Test() => _albumRepository.FindById(Constants.ValidAlbumGuid).Should().Be(_album1);

        [Fact] public void FindById_InvalidId_Test() => _albumRepository
            .Invoking(repository => repository.FindById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.AlbumNotFound, Constants.InvalidGuid));

        [Fact] public void Save_Test()
        {
            _albumRepository.Save(_album1).Should().Be(_album1);
            _dbSetMock.Verify(mock => mock.Add(It.IsAny<Album>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Album updatedAlbum = CreateTestAlbum(_album2, _album1.IsActive);
            _albumRepository.UpdateById(_album2, Constants.ValidAlbumGuid).Should().Be(updatedAlbum);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _albumRepository
            .Invoking(repository => repository.UpdateById(_album2, Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.AlbumNotFound, Constants.InvalidGuid));

        [Fact] public void DisableById_ValidId_Test()
        {
            Album disabledAlbum = CreateTestAlbum(_album1, false);
            _albumRepository.DisableById(Constants.ValidAlbumGuid).Should().Be(disabledAlbum);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DisableById_InvalidId_Test() => _albumRepository
            .Invoking(repository => repository.DisableById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.AlbumNotFound, Constants.InvalidGuid));

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