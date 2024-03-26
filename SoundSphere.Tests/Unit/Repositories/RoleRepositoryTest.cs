using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Repositories
{
    public class RoleRepositoryTest
    {
        private readonly Mock<DbSet<Role>> _setMock = new();
        private readonly Mock<SoundSphereContext> _contextMock = new();
        private readonly IRoleRepository _roleRepository;

        private readonly Role _role1 = RoleMock.GetMockedRole1();
        private readonly IList<Role> _roles = RoleMock.GetMockedRoles();

        public RoleRepositoryTest()
        {
            var queryableRoles = _roles.AsQueryable();
            _setMock.As<IQueryable<Role>>().Setup(mock => mock.Provider).Returns(queryableRoles.Provider);
            _setMock.As<IQueryable<Role>>().Setup(mock => mock.Expression).Returns(queryableRoles.Expression);
            _setMock.As<IQueryable<Role>>().Setup(mock => mock.ElementType).Returns(queryableRoles.ElementType);
            _setMock.As<IQueryable<Role>>().Setup(mock => mock.GetEnumerator()).Returns(queryableRoles.GetEnumerator());
            _contextMock.Setup(mock => mock.Roles).Returns(_setMock.Object);
            _roleRepository = new RoleRepository(_contextMock.Object);
        }

        [Fact] public void FindAll_Test() => _roleRepository.FindAll().Should().BeEquivalentTo(_roles);

        [Fact] public void FindById_ValidId_Test() => _roleRepository.FindById(Constants.ValidRoleGuid).Should().BeEquivalentTo(_role1);

        [Fact] public void FindById_InvalidId_Test() =>
            _roleRepository.Invoking(repository => repository.FindById(Constants.InvalidGuid))
                           .Should().Throw<ResourceNotFoundException>()
                           .WithMessage($"Role with id {Constants.InvalidGuid} not found!");

        [Fact] public void Save_Test()
        {
            _roleRepository.Save(_role1).Should().BeEquivalentTo(_role1);
            _setMock.Verify(mock => mock.Add(It.IsAny<Role>()));
            _contextMock.Verify(mock => mock.SaveChanges());
        }
    }
}