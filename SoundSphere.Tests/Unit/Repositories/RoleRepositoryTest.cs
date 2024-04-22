using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database;
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
        private readonly Mock<DbSet<Role>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IRoleRepository _roleRepository;

        private readonly Role _role1 = RoleMock.GetMockedRole1();
        private readonly IList<Role> _roles = RoleMock.GetMockedRoles();

        public RoleRepositoryTest()
        {
            IQueryable<Role> queryableRoles = _roles.AsQueryable();
            _dbSetMock.As<IQueryable<Role>>().Setup(mock => mock.Provider).Returns(queryableRoles.Provider);
            _dbSetMock.As<IQueryable<Role>>().Setup(mock => mock.Expression).Returns(queryableRoles.Expression);
            _dbSetMock.As<IQueryable<Role>>().Setup(mock => mock.ElementType).Returns(queryableRoles.ElementType);
            _dbSetMock.As<IQueryable<Role>>().Setup(mock => mock.GetEnumerator()).Returns(queryableRoles.GetEnumerator());
            _dbContextMock.Setup(mock => mock.Roles).Returns(_dbSetMock.Object);
            _roleRepository = new RoleRepository(_dbContextMock.Object);
        }

        [Fact] public void FindAll_Test() => _roleRepository.FindAll().Should().BeEquivalentTo(_roles);

        [Fact] public void FindById_ValidId_Test() => _roleRepository.FindById(Constants.ValidRoleGuid).Should().Be(_role1);

        [Fact] public void FindById_InvalidId_Test() => _roleRepository
            .Invoking(repository => repository.FindById(Constants.InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(Constants.RoleNotFound, Constants.InvalidGuid));

        [Fact] public void Save_Test()
        {
            _roleRepository.Save(_role1).Should().Be(_role1);
            _dbSetMock.Verify(mock => mock.Add(It.IsAny<Role>()));
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }
    }
}