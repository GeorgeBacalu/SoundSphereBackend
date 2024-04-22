using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;

namespace SoundSphere.Tests.Mocks
{
    public class RoleMock
    {
        private RoleMock() { }

        public static IList<Role> GetMockedRoles() => new List<Role> { GetMockedRole1(), GetMockedRole2(), GetMockedRole3() };

        public static IList<RoleDto> GetMockedRoleDtos() => new List<RoleDto> { GetMockedRoleDto1(), GetMockedRoleDto2(), GetMockedRoleDto3() };

        public static Role GetMockedRole1() => new Role { Id = Guid.Parse("deaf35ba-fe71-4c21-8a3c-d8e5b32a06fe"), Type = RoleType.Administrator };

        public static Role GetMockedRole2() => new Role { Id = Guid.Parse("2fc8f207-5af0-402f-84d0-e1c7fa7336a6"), Type = RoleType.Moderator };

        public static Role GetMockedRole3() => new Role { Id = Guid.Parse("61ee6dda-e18a-4eb9-a736-3f95ba5537f7"), Type = RoleType.Listener };

        public static RoleDto GetMockedRoleDto1() => new RoleDto { Id = Guid.Parse("deaf35ba-fe71-4c21-8a3c-d8e5b32a06fe"), Type = RoleType.Administrator };

        public static RoleDto GetMockedRoleDto2() => new RoleDto { Id = Guid.Parse("2fc8f207-5af0-402f-84d0-e1c7fa7336a6"), Type = RoleType.Moderator };

        public static RoleDto GetMockedRoleDto3() => new RoleDto { Id = Guid.Parse("61ee6dda-e18a-4eb9-a736-3f95ba5537f7"), Type = RoleType.Listener };
    }
}