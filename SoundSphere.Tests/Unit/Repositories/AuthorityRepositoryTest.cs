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
    public class AuthorityRepositoryTest
    {
        private readonly Mock<DbSet<Authority>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IAuthorityRepository _authorityRepository;

        private readonly Authority _authority1 = AuthorityMock.GetMockedAuthority1();
        private readonly IList<Authority> _authorities = AuthorityMock.GetMockedAuthorities();

        public AuthorityRepositoryTest()
        {
            IQueryable<Authority> queryableAuthorities = _authorities.AsQueryable();
            _dbSetMock.As<IQueryable<Authority>>().Setup(mock => mock.Provider).Returns(queryableAuthorities.Provider);
            _dbSetMock.As<IQueryable<Authority>>().Setup(mock => mock.Expression).Returns(queryableAuthorities.Expression);
            _dbSetMock.As<IQueryable<Authority>>().Setup(mock => mock.ElementType).Returns(queryableAuthorities.ElementType);
            _dbSetMock.As<IQueryable<Authority>>().Setup(mock => mock.GetEnumerator()).Returns(queryableAuthorities.GetEnumerator());
            _dbContextMock.Setup(mock => mock.Authorities).Returns(_dbSetMock.Object);
            _authorityRepository = new AuthorityRepository(_dbContextMock.Object);
        }

        [Fact] public void FindAll_Test() => _authorityRepository.FindAll().Should().BeEquivalentTo(_authorities);

        [Fact] public void FindById_ValidId_Test() => _authorityRepository.FindById(Constants.ValidAuthorityGuid).Should().Be(_authority1);

        [Fact] public void FindById_InvalidId_Test() => _authorityRepository
            .Invoking(repository => repository.FindById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.AuthorityNotFound, Constants.InvalidGuid));

        [Fact] public void Save_Test()
        {
            _authorityRepository.Save(_authority1).Should().Be(_authority1);
            _dbSetMock.Verify(mock => mock.Add(It.IsAny<Authority>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }
    }
}