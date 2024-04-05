using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;

namespace SoundSphere.Tests.Mocks
{
    public class UserMock
    {
        private UserMock() { }

        public static IList<User> GetMockedUsers() => new List<User> { GetMockedUser1(), GetMockedUser2() };

        public static IList<UserDto> GetMockedUserDtos() => new List<UserDto> { GetMockedUserDto1(), GetMockedUserDto2() };

        public static User GetMockedUser1() => new User
        {
            Id = Guid.Parse("0a9e546f-38b4-4dbf-a482-24a82169890e"),
            Name = "user_name1",
            Email = "user_email1@email.com",
            Password = "user_password1",
            Mobile = "+407000000",
            Address = "user_address1",
            Birthday = new DateOnly(2000, 1, 1),
            Avatar = "https://user_avatar1.jpg",
            Role = RoleMock.GetMockedRole1(),
            Authorities = AuthorityMock.GetMockedAuthorities1(),
            IsActive = true
        };

        public static User GetMockedUser2() => new User
        {
            Id = Guid.Parse("31a088bd-6fe8-4226-bd03-f4af698abe83"),
            Name = "user_name2",
            Email = "user_email2@email.com",
            Password = "user_password2",
            Mobile = "+407000001",
            Address = "user_address2",
            Birthday = new DateOnly(2000, 1, 2),
            Avatar = "https://user_avatar2.jpg",
            Role = RoleMock.GetMockedRole2(),
            Authorities = AuthorityMock.GetMockedAuthorities2(),
            IsActive = false
        };

        public static User GetMockedUser3() => new User
        {
            Id = Guid.Parse("b3692c1c-384a-47ef-a258-106bceb73f0c"),
            Name = "user_name3",
            Email = "user_email3@email.com",
            Password = "user_password3",
            Mobile = "+407000002",
            Address = "user_address3",
            Birthday = new DateOnly(2000, 1, 3),
            Avatar = "https://user_avatar3.jpg",
            Role = RoleMock.GetMockedRole3(),
            Authorities = AuthorityMock.GetMockedAuthorities1(),
            IsActive = true
        };

        public static UserDto GetMockedUserDto1() => new UserDto
        {
            Id = Guid.Parse("0a9e546f-38b4-4dbf-a482-24a82169890e"),
            Name = "user_name1",
            Email = "user_email1@email.com",
            Mobile = "+407000000",
            Address = "user_address1",
            Birthday = new DateOnly(2000, 1, 1),
            Avatar = "https://user_avatar1.jpg",
            RoleId = Guid.Parse("deaf35ba-fe71-4c21-8a3c-d8e5b32a06fe"),
            AuthoritiesIds = new List<Guid>
            {
                Guid.Parse("75e924c3-34e7-46ef-b521-7331e36caadd"),
                Guid.Parse("362b20cf-3636-49ed-9489-d2700339efce")
            },
            IsActive = true
        };

        public static UserDto GetMockedUserDto2() => new UserDto
        {
            Id = Guid.Parse("31a088bd-6fe8-4226-bd03-f4af698abe83"),
            Name = "user_name2",
            Email = "user_email2@email.com",
            Mobile = "+407000001",
            Address = "user_address2",
            Birthday = new DateOnly(2000, 1, 2),
            Avatar = "https://user_avatar2.jpg",
            RoleId = Guid.Parse("2fc8f207-5af0-402f-84d0-e1c7fa7336a6"),
            AuthoritiesIds = new List<Guid>
            {
                Guid.Parse("3cc47e2d-b14e-472f-9868-fbb90b15f18e"),
                Guid.Parse("59525baa-6eaa-42d8-b213-9094af0d604b")
            },
            IsActive = false
        };

        public static UserDto GetMockedUserDto3() => new UserDto
        {
            Id = Guid.Parse("b3692c1c-384a-47ef-a258-106bceb73f0c"),
            Name = "user_name3",
            Email = "user_email3@email.com",
            Mobile = "+407000002",
            Address = "user_address3",
            Birthday = new DateOnly(2000, 1, 3),
            Avatar = "https://user_avatar3.jpg",
            RoleId = Guid.Parse("61ee6dda-e18a-4eb9-a736-3f95ba5537f7"),
            AuthoritiesIds = new List<Guid>
            {
                Guid.Parse("75e924c3-34e7-46ef-b521-7331e36caadd"),
                Guid.Parse("362b20cf-3636-49ed-9489-d2700339efce")
            },
            IsActive = true
        };
    }
}