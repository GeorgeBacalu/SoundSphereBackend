using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Services;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.RoleMock;

namespace SoundSphere.Tests.Unit.Services
{
    public class RoleServiceTest
    {
        private readonly Mock<IRoleRepository> _roleRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly IRoleService _roleService;

        private readonly Role _role1 = GetMockedRole1();
        private readonly Role _role2 = GetMockedRole2();
        private readonly Role _role3 = GetMockedRole3();
        private readonly IList<Role> _roles = GetMockedRoles();
        private readonly RoleDto _roleDto1 = GetMockedRoleDto1();
        private readonly RoleDto _roleDto2 = GetMockedRoleDto2();
        private readonly RoleDto _roleDto3 = GetMockedRoleDto3();
        private readonly IList<RoleDto> _roleDtos = GetMockedRoleDtos();

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

        [Fact] public void GetAll_Test()
        {
            _roleRepositoryMock.Setup(mock => mock.GetAll()).Returns(_roles);
            _roleService.GetAll().Should().BeEquivalentTo(_roleDtos);
        }

        [Fact] public void GetById_Test()
        {
            _roleRepositoryMock.Setup(mock => mock.GetById(ValidRoleGuid)).Returns(_role1);
            _roleService.GetById(ValidRoleGuid).Should().Be(_roleDto1);
        }

        [Fact] public void Add_Test()
        {
            _roleRepositoryMock.Setup(mock => mock.Add(_role1)).Returns(_role1);
            _roleService.Add(_roleDto1).Should().Be(_roleDto1);
        }
    }
}