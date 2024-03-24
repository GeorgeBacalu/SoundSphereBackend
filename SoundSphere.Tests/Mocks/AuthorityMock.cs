using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;

namespace SoundSphere.Tests.Mocks
{
    public class AuthorityMock
    {
        private AuthorityMock() { }

        public static IList<Authority> GetMockedAuthorities() => GetMockedAuthorities1().Concat(GetMockedAuthorities2()).ToList();

        public static IList<AuthorityDto> GetMockedAuthorityDtos() => GetMockedAuthorityDtos1().Concat(GetMockedAuthorityDtos2()).ToList();

        public static IList<Authority> GetMockedAuthorities1() => new List<Authority> { GetMockedAuthority1(), GetMockedAuthority2() };

        public static IList<Authority> GetMockedAuthorities2() => new List<Authority> { GetMockedAuthority3(), GetMockedAuthority4() };

        public static IList<AuthorityDto> GetMockedAuthorityDtos1() => new List<AuthorityDto> { GetMockedAuthorityDto1(), GetMockedAuthorityDto2() };

        public static IList<AuthorityDto> GetMockedAuthorityDtos2() => new List<AuthorityDto> { GetMockedAuthorityDto3(), GetMockedAuthorityDto4() };
    
        public static Authority GetMockedAuthority1() => new Authority
        {
            Id = Guid.Parse("75e924c3-34e7-46ef-b521-7331e36caadd"),
            Type = AuthorityType.Create
        };

        public static Authority GetMockedAuthority2() => new Authority
        {
            Id = Guid.Parse("362b20cf-3636-49ed-9489-d2700339efce"),
            Type = AuthorityType.Read
        };

        public static Authority GetMockedAuthority3() => new Authority
        {
            Id = Guid.Parse("3cc47e2d-b14e-472f-9868-fbb90b15f18e"),
            Type = AuthorityType.Update
        };

        public static Authority GetMockedAuthority4() => new Authority
        {
            Id = Guid.Parse("59525baa-6eaa-42d8-b213-9094af0d604b"),
            Type = AuthorityType.Delete
        };

        public static AuthorityDto GetMockedAuthorityDto1() => new AuthorityDto
        {
            Id = Guid.Parse("75e924c3-34e7-46ef-b521-7331e36caadd"),
            Type = AuthorityType.Create
        };

        public static AuthorityDto GetMockedAuthorityDto2() => new AuthorityDto
        {
            Id = Guid.Parse("362b20cf-3636-49ed-9489-d2700339efce"),
            Type = AuthorityType.Read
        };

        public static AuthorityDto GetMockedAuthorityDto3() => new AuthorityDto
        {
            Id = Guid.Parse("3cc47e2d-b14e-472f-9868-fbb90b15f18e"),
            Type = AuthorityType.Update
        };

        public static AuthorityDto GetMockedAuthorityDto4() => new AuthorityDto
        {
            Id = Guid.Parse("59525baa-6eaa-42d8-b213-9094af0d604b"),
            Type = AuthorityType.Delete
        };
    }
}