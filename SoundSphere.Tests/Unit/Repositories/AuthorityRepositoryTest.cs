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
    public class AuthorityRepositoryTest
    {
        private readonly Mock<DbSet<Authority>> _setMock = new();
        private readonly Mock<SoundSphereContext> _contextMock = new();
        private readonly IAuthorityRepository _authorityRepository;

        private readonly Authority _authority1 = AuthorityMock.GetMockedAuthority1();
        private readonly IList<Authority> _authorities = AuthorityMock.GetMockedAuthorities();

        public AuthorityRepositoryTest()
        {
            var queryableAuthorities = _authorities.AsQueryable();
            _setMock.As<IQueryable<Authority>>().Setup(mock => mock.Provider).Returns(queryableAuthorities.Provider);
            _setMock.As<IQueryable<Authority>>().Setup(mock => mock.Expression).Returns(queryableAuthorities.Expression);
            _setMock.As<IQueryable<Authority>>().Setup(mock => mock.ElementType).Returns(queryableAuthorities.ElementType);
            _setMock.As<IQueryable<Authority>>().Setup(mock => mock.GetEnumerator()).Returns(queryableAuthorities.GetEnumerator());
            _contextMock.Setup(mock => mock.Authorities).Returns(_setMock.Object);
            _authorityRepository = new AuthorityRepository(_contextMock.Object);
        }

        [Fact] public void FindAll_Test() => _authorityRepository.FindAll().Should().BeEquivalentTo(_authorities);

        [Fact] public void FindById_ValidId_Test() => _authorityRepository.FindById(Constants.ValidAuthorityGuid).Should().BeEquivalentTo(_authority1);

        [Fact] public void FindById_InvalidId_Test() =>
            _authorityRepository.Invoking(repository => repository.FindById(Constants.InvalidGuid))
                                .Should().Throw<ResourceNotFoundException>()
                                .WithMessage($"Authority with id {Constants.InvalidGuid} not found!");

        [Fact] public void Save_Test()
        {
            _authorityRepository.Save(_authority1).Should().BeEquivalentTo(_authority1);
            _setMock.Verify(mock => mock.Add(It.IsAny<Authority>()));
            _contextMock.Verify(mock => mock.SaveChanges());
        }
    }
}