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
using static SoundSphere.Tests.Mocks.ArtistMock;

namespace SoundSphere.Tests.Unit.Repositories
{
    public class ArtistRepositoryTest
    {
        private readonly Mock<DbSet<Artist>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IArtistRepository _artistRepository;

        private readonly Artist _artist1 = GetMockedArtist1();
        private readonly Artist _artist2 = GetMockedArtist2();
        private readonly IList<Artist> _artists = GetMockedArtists();
        private readonly IList<Artist> _activeArtists = GetMockedActiveArtists();
        private readonly IList<Artist> _paginatedArtists = GetMockedPaginatedArtists();
        private readonly IList<Artist> _activePaginatedArtists = GetMockedActivePaginatedArtists();
        private readonly ArtistPaginationRequest _paginationRequest = GetMockedArtistsPaginationRequest();

        public ArtistRepositoryTest()
        {
            IQueryable<Artist> queryableArtists = _artists.AsQueryable();
            _dbSetMock.As<IQueryable<Artist>>().Setup(mock => mock.Provider).Returns(queryableArtists.Provider);
            _dbSetMock.As<IQueryable<Artist>>().Setup(mock => mock.Expression).Returns(queryableArtists.Expression);
            _dbSetMock.As<IQueryable<Artist>>().Setup(mock => mock.ElementType).Returns(queryableArtists.ElementType);
            _dbSetMock.As<IQueryable<Artist>>().Setup(mock => mock.GetEnumerator()).Returns(queryableArtists.GetEnumerator());
            _dbContextMock.Setup(mock => mock.Artists).Returns(_dbSetMock.Object);
            _artistRepository = new ArtistRepository(_dbContextMock.Object);
        }

        [Fact] public void GetAll_Test() => _artistRepository.GetAll().Should().BeEquivalentTo(_artists);

        [Fact] public void GetAllActive_Test() => _artistRepository.GetAllActive().Should().BeEquivalentTo(_activeArtists);

        [Fact] public void GetAllPagination_Test() => _artistRepository.GetAllPagination(_paginationRequest).Should().BeEquivalentTo(_paginatedArtists);

        [Fact] public void GetAllActivePagination_Test() => _artistRepository.GetAllActivePagination(_paginationRequest).Should().BeEquivalentTo(_activePaginatedArtists);

        [Fact] public void GetById_ValidId_Test() => _artistRepository.GetById(ValidArtistGuid).Should().Be(_artist1);

        [Fact] public void GetById_InvalidId_Test() => _artistRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(ArtistNotFound, InvalidGuid));

        [Fact] public void Add_Test()
        {
            _artistRepository.Add(_artist1).Should().Be(_artist1);
            _dbSetMock.Verify(mock => mock.Add(It.IsAny<Artist>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Artist updatedArtist = GetArtist(_artist2, _artist1.IsActive);
            _artistRepository.UpdateById(_artist2, ValidArtistGuid).Should().Be(updatedArtist);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _artistRepository
            .Invoking(repository => repository.UpdateById(_artist2, InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(ArtistNotFound, InvalidGuid));

        [Fact] public void DeleteById_ValidId_Test()
        {
            Artist deletedArtist = GetArtist(_artist1, false);
            _artistRepository.DeleteById(ValidArtistGuid).Should().Be(deletedArtist);
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DeleteById_InvalidId_Test() => _artistRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(ArtistNotFound, InvalidGuid));

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