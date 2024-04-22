using FluentAssertions;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class AuthorityRepositoryIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        private readonly Authority _authority1 = AuthorityMock.GetMockedAuthority1();
        private readonly IList<Authority> _authorities = AuthorityMock.GetMockedAuthorities();

        public AuthorityRepositoryIntegrationTest(DbFixture fixture) => _fixture = fixture;

        private void Execute(Action<AuthorityRepository, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var authorityRepository = new AuthorityRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Authorities.AddRange(_authorities);
            context.SaveChanges();
            action(authorityRepository, context);
        }

        [Fact] public void FindAll_Test() => Execute((authorityRepository, context) => authorityRepository.FindAll().Should().BeEquivalentTo(_authorities));

        [Fact] public void FindById_ValidId_Test() => Execute((authorityRepository, context) => authorityRepository.FindById(Constants.ValidAuthorityGuid).Should().Be(_authority1));

        [Fact] public void FindById_InvalidId_Test() => Execute((authorityRepository, context) => authorityRepository
            .Invoking(repository => repository.FindById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.AuthorityNotFound, Constants.InvalidGuid)));

        [Fact] public void Save_Test() => Execute((authorityRepository, context) => authorityRepository
            .Invoking(repository => repository.Save(_authority1))
            .Should().Throw<InvalidOperationException>());
    }
}