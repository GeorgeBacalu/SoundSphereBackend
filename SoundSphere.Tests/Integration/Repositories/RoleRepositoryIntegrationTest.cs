using FluentAssertions;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.RoleMock;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class RoleRepositoryIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        private readonly Role _role1 = GetMockedRole1();
        private readonly IList<Role> _roles = GetMockedRoles();

        public RoleRepositoryIntegrationTest(DbFixture fixture) => _fixture = fixture;

        private void Execute(Action<RoleRepository, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var roleRepository = new RoleRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Roles.AddRange(_roles);
            context.SaveChanges();
            action(roleRepository, context);
            transaction.Rollback();
        }

        [Fact] public void GetAll_Test() => Execute((roleRepository, context) => roleRepository.GetAll().Should().BeEquivalentTo(_roles));

        [Fact] public void GetById_ValidId_Test() => Execute((roleRepository, context) => roleRepository.GetById(ValidRoleGuid).Should().Be(_role1));

        [Fact] public void GetById_InvalidId_Test() => Execute((roleRepository, context) => roleRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(RoleNotFound, InvalidGuid)));

        [Fact] public void Add_Test() => Execute((roleRepository, context) => roleRepository
            .Invoking(repository => repository.Add(_role1))
            .Should().Throw<InvalidOperationException>());
    }
}