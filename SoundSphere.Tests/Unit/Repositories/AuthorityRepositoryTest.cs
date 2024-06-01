using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.AuthorityMock;

namespace SoundSphere.Tests.Unit.Repositories
{
    public class AuthorityRepositoryTest
    {
        private readonly Mock<DbSet<Authority>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IAuthorityRepository _authorityRepository;

        private readonly Authority _authority1 = GetMockedAuthority1();
        private readonly IList<Authority> _authorities = GetMockedAuthorities();

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

        [Fact] public void GetAll_Test() => _authorityRepository.GetAll().Should().BeEquivalentTo(_authorities);

        [Fact] public void GetById_ValidId_Test() => _authorityRepository.GetById(ValidAuthorityGuid).Should().Be(_authority1);

        [Fact] public void GetById_InvalidId_Test() => _authorityRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(AuthorityNotFound, InvalidGuid));

        [Fact] public void Add_Test()
        {
            _authorityRepository.Add(_authority1).Should().Be(_authority1);
            _dbSetMock.Verify(mock => mock.Add(It.IsAny<Authority>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }
    }
}