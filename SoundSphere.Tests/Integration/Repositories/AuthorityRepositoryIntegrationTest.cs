using FluentAssertions;
using SoundSphere.Database.Constants;
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

        private void Execute(Action<AuthorityRepository, SoundSphereContext> action)
        {
            using var context = _fixture.CreateContext();
            var authorityRepository = new AuthorityRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Authorities.AddRange(_authorities);
            context.SaveChanges();
            action(authorityRepository, context);
        }

        [Fact] public void FindAll_Test() => Execute((authorityRepository, context) => authorityRepository.FindAll().Should().BeEquivalentTo(_authorities));

        [Fact] public void FindById_ValidId_Test() => Execute((authorityRepository, context) => authorityRepository.FindById(Constants.ValidAuthorityGuid).Should().BeEquivalentTo(_authority1));

        [Fact] public void FindById_InvalidId_Test() => Execute((authorityRepository, context) => 
            authorityRepository.Invoking(repository => repository.FindById(Constants.InvalidGuid))
                               .Should().Throw<ResourceNotFoundException>()
                               .WithMessage($"Authority with id {Constants.InvalidGuid} not found!"));

        [Fact] public void Save_Test() => Execute((authorityRepository, context) =>
        {
            Authority newAuthority = AuthorityMock.GetMockedAuthority1();
            authorityRepository.Invoking(repository => repository.Save(newAuthority)).Should().Throw<InvalidOperationException>();
        });
    }
}