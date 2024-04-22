using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Services;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Services
{
    public class RoleServiceTest
    {
        private readonly Mock<IRoleRepository> _roleRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly IRoleService _roleService;

        private readonly Role _role1 = RoleMock.GetMockedRole1();
        private readonly Role _role2 = RoleMock.GetMockedRole2();
        private readonly Role _role3 = RoleMock.GetMockedRole3();
        private readonly IList<Role> _roles = RoleMock.GetMockedRoles();
        private readonly RoleDto _roleDto1 = RoleMock.GetMockedRoleDto1();
        private readonly RoleDto _roleDto2 = RoleMock.GetMockedRoleDto2();
        private readonly RoleDto _roleDto3 = RoleMock.GetMockedRoleDto3();
        private readonly IList<RoleDto> _roleDtos = RoleMock.GetMockedRoleDtos();

        public RoleServiceTest()
        {
            _mapperMock.Setup(mock => mock.Map<RoleDto>(_role1)).Returns(_roleDto1);
            _mapperMock.Setup(mock => mock.Map<RoleDto>(_role2)).Returns(_roleDto2);
            _mapperMock.Setup(mock => mock.Map<RoleDto>(_role3)).Returns(_roleDto3);
            _mapperMock.Setup(mock => mock.Map<Role>(_roleDto1)).Returns(_role1);
            _mapperMock.Setup(mock => mock.Map<Role>(_roleDto2)).Returns(_role2);
            _mapperMock.Setup(mock => mock.Map<Role>(_roleDto3)).Returns(_role3);
            _roleService = new RoleService(_roleRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact] public void FindAll_Test()
        {
            _roleRepositoryMock.Setup(mock => mock.FindAll()).Returns(_roles);
            _roleService.FindAll().Should().BeEquivalentTo(_roleDtos);
        }

        [Fact] public void FindById_Test()
        {
            _roleRepositoryMock.Setup(mock => mock.FindById(Constants.ValidRoleGuid)).Returns(_role1);
            _roleService.FindById(Constants.ValidRoleGuid).Should().Be(_roleDto1);
        }

        [Fact] public void Save_Test()
        {
            _roleRepositoryMock.Setup(mock => mock.Save(_role1)).Returns(_role1);
            _roleService.Save(_roleDto1).Should().Be(_roleDto1);
        }
    }
}