using FluentAssertions;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.AuthorityMock;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class AuthorityRepositoryIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        private readonly Authority _authority1 = GetMockedAuthority1();
        private readonly IList<Authority> _authorities = GetMockedAuthoritiesAdmin();

        public AuthorityRepositoryIntegrationTest(DbFixture fixture) => _fixture = fixture;

        private void Execute(Action<AuthorityRepository, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var authorityRepository = new AuthorityRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Authorities.AddRange(_authorities);
            context.SaveChanges();
            action(authorityRepository, context);
            transaction.Rollback();
        }

        [Fact] public void GetAll_Test() => Execute((authorityRepository, context) => authorityRepository.GetAll().Should().BeEquivalentTo(_authorities));

        [Fact] public void GetById_ValidId_Test() => Execute((authorityRepository, context) => authorityRepository.GetById(ValidAuthorityGuid).Should().Be(_authority1));

        [Fact] public void GetById_InvalidId_Test() => Execute((authorityRepository, context) => authorityRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(AuthorityNotFound, InvalidGuid)));

        [Fact] public void Add_Test() => Execute((authorityRepository, context) => authorityRepository
            .Invoking(repository => repository.Add(_authority1))
            .Should().Throw<InvalidOperationException>());
    }
}