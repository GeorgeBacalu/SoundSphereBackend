using FluentAssertions;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Integration.Repositories
{
    public class RoleRepositoryIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        private readonly Role _role1 = RoleMock.GetMockedRole1();
        private readonly IList<Role> _roles = RoleMock.GetMockedRoles();

        public RoleRepositoryIntegrationTest(DbFixture fixture) => _fixture = fixture;

        private void Execute(Action<RoleRepository, SoundSphereContext> action)
        {
            using var context = _fixture.CreateContext();
            var roleRepository = new RoleRepository(context);
            using var transaction = context.Database.BeginTransaction();
            context.Roles.AddRange(_roles);
            context.SaveChanges();
            action(roleRepository, context);
        }

        [Fact] public void FindAll_Test() => Execute((roleRepository, context) => roleRepository.FindAll().Should().BeEquivalentTo(_roles));

        [Fact] public void FindById_ValidId_Test() => Execute((roleRepository, context) => roleRepository.FindById(Constants.ValidRoleGuid).Should().BeEquivalentTo(_role1));

        [Fact] public void FindById_InvalidId_Test() => Execute((roleRepository, context) => 
        roleRepository.Invoking(repository => repository.FindById(Constants.InvalidGuid))
                      .Should().Throw<ResourceNotFoundException>()
                      .WithMessage($"Role with id {Constants.InvalidGuid} not found!"));

        [Fact] public void Save_Test() => Execute((roleRepository, context) =>
        {
            Role newRole = RoleMock.GetMockedRole1();
            roleRepository.Invoking(repository => repository.Save(newRole))
                          .Should().Throw<InvalidOperationException>();
        });
    }
}