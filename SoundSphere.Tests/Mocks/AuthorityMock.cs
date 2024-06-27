using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Tests.Mocks
{
    public class AuthorityMock
    {
        private AuthorityMock() { }

        public static IList<Authority> GetMockedAuthoritiesAdmin() => new List<Authority> { GetMockedAuthority1(), GetMockedAuthority2(), GetMockedAuthority3(), GetMockedAuthority4() };

        public static IList<AuthorityDto> GetMockedAuthorityDtosAdmin() => new List<AuthorityDto> { GetMockedAuthorityDto1(), GetMockedAuthorityDto2(), GetMockedAuthorityDto3(), GetMockedAuthorityDto4() };

        public static IList<Authority> GetMockedAuthoritiesModerator() => new List<Authority> { GetMockedAuthority1(), GetMockedAuthority2(), GetMockedAuthority3() };

        public static IList<AuthorityDto> GetMockedAuthorityDtosModerator() => new List<AuthorityDto> { GetMockedAuthorityDto1(), GetMockedAuthorityDto2(), GetMockedAuthorityDto3() };

        public static IList<Authority> GetMockedAuthoritiesListener() => new List<Authority> { GetMockedAuthority1() };

        public static IList<AuthorityDto> GetMockedAuthorityDtosListener() => new List<AuthorityDto> { GetMockedAuthorityDto1() };

        public static Authority GetMockedAuthority1() => new Authority { Id = Guid.Parse("75e924c3-34e7-46ef-b521-7331e36caadd"), Type = AuthorityType.Create };

        public static Authority GetMockedAuthority2() => new Authority { Id = Guid.Parse("362b20cf-3636-49ed-9489-d2700339efce"), Type = AuthorityType.Read };

        public static Authority GetMockedAuthority3() => new Authority { Id = Guid.Parse("3cc47e2d-b14e-472f-9868-fbb90b15f18e"), Type = AuthorityType.Update };

        public static Authority GetMockedAuthority4() => new Authority { Id = Guid.Parse("59525baa-6eaa-42d8-b213-9094af0d604b"), Type = AuthorityType.Delete };

        public static AuthorityDto GetMockedAuthorityDto1() => new AuthorityDto { Id = Guid.Parse("75e924c3-34e7-46ef-b521-7331e36caadd"), Type = AuthorityType.Create };

        public static AuthorityDto GetMockedAuthorityDto2() => new AuthorityDto { Id = Guid.Parse("362b20cf-3636-49ed-9489-d2700339efce"), Type = AuthorityType.Read };

        public static AuthorityDto GetMockedAuthorityDto3() => new AuthorityDto { Id = Guid.Parse("3cc47e2d-b14e-472f-9868-fbb90b15f18e"), Type = AuthorityType.Update };

        public static AuthorityDto GetMockedAuthorityDto4() => new AuthorityDto { Id = Guid.Parse("59525baa-6eaa-42d8-b213-9094af0d604b"), Type = AuthorityType.Delete };
    }
}