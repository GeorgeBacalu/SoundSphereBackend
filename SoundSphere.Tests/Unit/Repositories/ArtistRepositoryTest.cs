using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Repositories
{
    public class ArtistRepositoryTest
    {
        private readonly Mock<DbSet<Artist>> _setMock = new();
        private readonly Mock<SoundSphereContext> _contextMock = new();
        private readonly IArtistRepository _artistRepository;

        private readonly Artist _artist1 = ArtistMock.GetMockedArtist1();
        private readonly Artist _artist2 = ArtistMock.GetMockedArtist2();
        private readonly IList<Artist> _artists = ArtistMock.GetMockedArtists();

        public ArtistRepositoryTest()
        {
            var queryableArtists = _artists.AsQueryable();
            _setMock.As<IQueryable<Artist>>().Setup(mock => mock.Provider).Returns(queryableArtists.Provider);
            _setMock.As<IQueryable<Artist>>().Setup(mock => mock.Expression).Returns(queryableArtists.Expression);
            _setMock.As<IQueryable<Artist>>().Setup(mock => mock.ElementType).Returns(queryableArtists.ElementType);
            _setMock.As<IQueryable<Artist>>().Setup(mock => mock.GetEnumerator()).Returns(queryableArtists.GetEnumerator());
            _contextMock.Setup(mock => mock.Artists).Returns(_setMock.Object);
            _artistRepository = new ArtistRepository(_contextMock.Object);
        }

        [Fact] public void FindAll_Test() => _artistRepository.FindAll().Should().BeEquivalentTo(_artists);

        [Fact] public void FindById_ValidId_Test() => _artistRepository.FindById(Constants.ValidArtistGuid).Should().BeEquivalentTo(_artist1);

        [Fact] public void FindById_InvalidId_Test() =>
            _artistRepository.Invoking(repository => repository.FindById(Constants.InvalidGuid))
                             .Should().Throw<ResourceNotFoundException>()
                             .WithMessage($"Artist with id {Constants.InvalidGuid} not found!");

        [Fact] public void Save_Test()
        {
            _artistRepository.Save(_artist1).Should().BeEquivalentTo(_artist1);
            _setMock.Verify(mock => mock.Add(It.IsAny<Artist>()));
            _contextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Artist updatedArtist = CreateTestArtist(_artist2, _artist1.IsActive);
            _artistRepository.UpdateById(_artist2, Constants.ValidArtistGuid).Should().BeEquivalentTo(updatedArtist);
            _contextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() =>
            _artistRepository.Invoking(repository => repository.UpdateById(_artist2, Constants.InvalidGuid))
                             .Should().Throw<ResourceNotFoundException>()
                             .WithMessage($"Artist with id {Constants.InvalidGuid} not found!");

        [Fact] public void DisableById_ValidId_Test()
        {
            Artist disabledArtist = CreateTestArtist(_artist1, false);
            _artistRepository.DisableById(Constants.ValidArtistGuid).Should().BeEquivalentTo(disabledArtist);
            _contextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DisableById_InvalidId_Test() =>
            _artistRepository.Invoking(repository => repository.DisableById(Constants.InvalidGuid))
                             .Should().Throw<ResourceNotFoundException>()
                             .WithMessage($"Artist with id {Constants.InvalidGuid} not found!");

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