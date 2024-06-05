using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Services;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.RoleMock;

namespace SoundSphere.Tests.Integration.Services
{
    public class RoleServiceIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly IMapper _mapper;

        private readonly IList<Role> _roles = GetMockedRoles();
        private readonly RoleDto _roleDto1 = GetMockedRoleDto1();
        private readonly IList<RoleDto> _roleDtos = GetMockedRoleDtos();

        public RoleServiceIntegrationTest(DbFixture fixture) => (_fixture, _mapper) = (fixture, new MapperConfiguration(config => { config.CreateMap<Role, RoleDto>(); config.CreateMap<RoleDto, Role>(); }).CreateMapper());

        private void Execute(Action<RoleService, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var roleService = new RoleService(new RoleRepository(context), _mapper);
            using var transaction = context.Database.BeginTransaction();
            context.AddRange(_roles);
            context.SaveChanges();
            action(roleService, context);
            transaction.Rollback();
        }

        [Fact] public void GetAll_Test() => Execute((roleService, context) => roleService.GetAll().Should().BeEquivalentTo(_roleDtos));

        [Fact] public void GetById_Test() => Execute((roleService, context) => roleService.GetById(ValidRoleGuid).Should().Be(_roleDto1));

        [Fact] public void Add_Test() => Execute((roleService, context) => roleService
            .Invoking(service => service.Add(_roleDto1))
            .Should().Throw<InvalidOperationException>());
    }
}