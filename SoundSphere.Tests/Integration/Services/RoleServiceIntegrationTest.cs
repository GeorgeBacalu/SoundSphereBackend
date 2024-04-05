using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Services;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Integration.Services
{
    public class RoleServiceIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly IMapper _mapper;

        private readonly IList<Role> _roles = RoleMock.GetMockedRoles();
        private readonly RoleDto _roleDto1 = RoleMock.GetMockedRoleDto1();
        private readonly IList<RoleDto> _roleDtos = RoleMock.GetMockedRoleDtos();

        public RoleServiceIntegrationTest(DbFixture fixture)
        {
            _fixture = fixture;
            _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Role, RoleDto>();
                config.CreateMap<RoleDto, Role>();
            }).CreateMapper();
        }

        private void Execute(Action<RoleService, SoundSphereContext> action)
        {
            using var context = _fixture.CreateContext();
            var roleService = new RoleService(new RoleRepository(context), _mapper);
            using var transaction = context.Database.BeginTransaction();
            context.AddRange(_roles);
            context.SaveChanges();
            action(roleService, context);
        }

        [Fact] public void FindAll_Test() => Execute((roleService, context) => roleService.FindAll().Should().BeEquivalentTo(_roleDtos));

        [Fact] public void FindById_Test() => Execute((roleService, context) => roleService.FindById(Constants.ValidRoleGuid).Should().BeEquivalentTo(_roleDto1));

        [Fact] public void Save_Test() => Execute((roleService, context) =>
        {
            RoleDto newRoleDto = RoleMock.GetMockedRoleDto1();
            roleService.Invoking(service => service.Save(newRoleDto)).Should().Throw<InvalidOperationException>();
        });
    }
}