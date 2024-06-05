using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.AlbumMock;

namespace SoundSphere.Tests.Unit.Repositories
{
    public class AlbumRepositoryTest
    {
        private readonly Mock<DbSet<Album>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IAlbumRepository _albumRepository;

        private readonly Album _album1 = GetMockedAlbum1();
        private readonly Album _album2 = GetMockedAlbum2();
        private readonly IList<Album> _albums = GetMockedAlbums();
        private readonly IList<Album> _paginedAlbums = GetMockedPaginatedAlbums();
        private readonly AlbumPaginationRequest _paginationRequest = GetMockedAlbumsPaginationRequest();

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

        [Fact] public void GetAll_Test() => _albumRepository.GetAll(_paginationRequest).Should().BeEquivalentTo(_paginedAlbums);

        [Fact] public void GetById_ValidId_Test() => _albumRepository.GetById(ValidAlbumGuid).Should().Be(_album1);

        [Fact] public void GetById_InvalidId_Test() => _albumRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(AlbumNotFound, InvalidGuid));

        [Fact] public void Add_Test()
        {
            _albumRepository.Add(_album1).Should().Be(_album1);
            _dbSetMock.Verify(mock => mock.Add(It.IsAny<Album>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Album updatedAlbum = GetAlbum(_album2, true);
            _albumRepository.UpdateById(_album2, ValidAlbumGuid).Should().Be(updatedAlbum);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _albumRepository
            .Invoking(repository => repository.UpdateById(_album2, InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(AlbumNotFound, InvalidGuid));

        [Fact] public void DeleteById_ValidId_Test()
        {
            Album deletedAlbum = GetAlbum(_album1, false);
            _albumRepository.DeleteById(ValidAlbumGuid).Should().Be(deletedAlbum);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DeleteById_InvalidId_Test() => _albumRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(AlbumNotFound, InvalidGuid));

        private Album GetAlbum(Album album, bool isActive) => new Album
        {
            Id = ValidAlbumGuid,
            Title = album.Title,
            ImageUrl = album.ImageUrl,
            ReleaseDate = album.ReleaseDate,
            SimilarAlbums = album.SimilarAlbums
        };
    }
}