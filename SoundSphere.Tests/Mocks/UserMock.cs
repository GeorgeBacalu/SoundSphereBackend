using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Dtos.Request.Models;
using SoundSphere.Database.Entities;
using static SoundSphere.Tests.Mocks.RoleMock;
using static SoundSphere.Tests.Mocks.AuthorityMock;

namespace SoundSphere.Tests.Mocks
{
    public class UserMock
    {
        private UserMock() { }

        public static IList<User> GetMockedUsers() => new List<User> { GetMockedUser1(), GetMockedUser2(), GetMockedUser3(), GetMockedUser4(), GetMockedUser5(), GetMockedUser6(), GetMockedUser7(), GetMockedUser8(), GetMockedUser9(), GetMockedUser10() };

        public static IList<UserDto> GetMockedUserDtos() => GetMockedUsers().Select(ToDto).ToList();

        public static IList<User> GetMockedPaginatedUsers() => GetMockedUsers().Where(user => user.DeletedAt == null).Take(10).ToList();

        public static IList<UserDto> GetMockedPaginatedUserDtos() => GetMockedPaginatedUsers().Select(ToDto).ToList();

        public static UserPaginationRequest GetMockedUsersPaginationRequest() => new UserPaginationRequest(
            SortCriteria: new Dictionary<UserSortCriterion, SortOrder> { { UserSortCriterion.ByName, SortOrder.Ascending }, { UserSortCriterion.ByEmail, SortOrder.Ascending } },
            SearchCriteria: new List<UserSearchCriterion> { UserSearchCriterion.ByName, UserSearchCriterion.ByEmail, UserSearchCriterion.ByBirthdayRange, UserSearchCriterion.ByRole },
            Name: "A",
            Email: "A",
            DateRange: new DateRange(new DateOnly(1950, 1, 1), new DateOnly(2024, 5, 31)),
            RoleType: RoleType.Listener);

        public static User GetMockedUser1() => new User
        {
            Id = Guid.Parse("0a9e546f-38b4-4dbf-a482-24a82169890e"),
            Name = "John Doe",
            Email = "john.doe@email.com",
            PasswordHash = "2BQB4efoBi4Z5VldE8ErF0qQ47NvO9+0hF0al7PmJ24=",
            PasswordSalt = "8N2aeuESLAW/U7ugfotVkA==",
            Mobile = "+40721543701",
            Address = "123 Main St, Boston, USA",
            Birthday = new DateOnly(1980, 2, 15),
            Avatar = "https://john-doe-avatar.jpg",
            EmailNotifications = true,
            Theme = Theme.Dark,
            Role = GetMockedRole1(),
            Authorities = GetMockedAuthoritiesAdmin(),
            CreatedAt = new DateTime(2024, 5, 1, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static User GetMockedUser2() => new User
        {
            Id = Guid.Parse("7eb88892-549b-4cae-90be-c52088354643"),
            Name = "Jane Smith",
            Email = "jane.smith@email.com",
            PasswordHash = "3JaeXTimekdGh/REM6606mLuKrYRoiR41ZU/G6lB+QY=",
            PasswordSalt = "YQg1izpe9i4sytzAGgHI7Q==",
            Mobile = "+40756321802",
            Address = "456 Oak St, London, UK",
            Birthday = new DateOnly(1982, 7, 10),
            Avatar = "https://jane-smith-avatar.jpg",
            EmailNotifications = true,
            Theme = Theme.Dark,
            Role = GetMockedRole1(),
            Authorities = GetMockedAuthoritiesAdmin(),
            CreatedAt = new DateTime(2024, 5, 2, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static User GetMockedUser3() => new User
        {
            Id = Guid.Parse("36712aa4-77f0-4510-8425-cf53dad54840"),
            Name = "Michael Johnson",
            Email = "michael.johnson@email.com",
            PasswordHash = "hwITjH+aBIuqnoBtW2QYYaL07ZYfdhzFtO3f5FMLps4=",
            PasswordSalt = "bFfX/bLhotMNHzTFJm6mOg==",
            Mobile = "+40789712303",
            Address = "789 Pine St, Madrid, Spain",
            Birthday = new DateOnly(1990, 11, 20),
            Avatar = "https://michael-johnson-avatar.jpg",
            EmailNotifications = true,
            Theme = Theme.Dark,
            Role = GetMockedRole1(),
            Authorities = GetMockedAuthoritiesAdmin(),
            CreatedAt = new DateTime(2024, 5, 3, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static User GetMockedUser4() => new User
        {
            Id = Guid.Parse("cb83d524-2016-4ce3-965b-223af9e7ba99"),
            Name = "Laura Brown",
            Email = "laura.brown@email.com",
            PasswordHash = "3s4NRoTE5JNNU3oBn1Nu7/TvBCb7oEJ4rG6NDRoeYyo=",
            PasswordSalt = "sqNvyZ7e7GkjoEqU9YfMAw==",
            Mobile = "+40734289604",
            Address = "333 Elm St, Paris, France",
            Birthday = new DateOnly(1985, 8, 25),
            Avatar = "https://laura-brown-avatar.jpg",
            EmailNotifications = true,
            Theme = Theme.Dark,
            Role = GetMockedRole2(),
            Authorities = GetMockedAuthoritiesModerator(),
            CreatedAt = new DateTime(2024, 5, 4, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static User GetMockedUser5() => new User
        {
            Id = Guid.Parse("9c6a5f06-b1f0-4507-b2f8-955156a8bed6"),
            Name = "Robert Davis",
            Email = "robert.davis@email.com",
            PasswordHash = "5iTYDGwqxY+oxmeR/tv0LYx+9nu8nxWUoPEyjTXKsPA=",
            PasswordSalt = "Nsh49T0WsVz+bOVqLNnzdA==",
            Mobile = "+40754321805",
            Address = "555 Oak St, Berlin, Germany",
            Birthday = new DateOnly(1988, 5, 12),
            Avatar = "https://robert-davis-avatar.jpg",
            EmailNotifications = true,
            Theme = Theme.Dark,
            Role = GetMockedRole2(),
            Authorities = GetMockedAuthoritiesModerator(),
            CreatedAt = new DateTime(2024, 5, 5, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static User GetMockedUser6() => new User
        {
            Id = Guid.Parse("ff3dd044-ebfb-4e1d-9050-b6dcfe684a1a"),
            Name = "Emily Wilson",
            Email = "emily.wilson@email.com",
            PasswordHash = "gG3FRFHv+UsSpIZt721JS8sJ5xyzleeSBL7+r5GiUsA=",
            PasswordSalt = "jyjr+pUA1grf5+b2JYWLQw==",
            Mobile = "+40789012606",
            Address = "777 Pine St, Sydney, Australia",
            Birthday = new DateOnly(1995, 9, 8),
            Avatar = "https://emily-wilson-avatar.jpg",
            EmailNotifications = false,
            Theme = Theme.Light,
            Role = GetMockedRole2(),
            Authorities = GetMockedAuthoritiesModerator(),
            CreatedAt = new DateTime(2024, 5, 6, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = new DateTime(2024, 5, 6, 12, 0, 0)
        };

        public static User GetMockedUser7() => new User
        {
            Id = Guid.Parse("f2a1edb1-332f-4c2f-af59-6b5508eafbec"),
            Name = "Michaela Taylor",
            Email = "michaela.taylor@email.com",
            PasswordHash = "bA1qhE2oW/YKJ+/wIP72g6RddIqtXz20bjsnwLu0/AY=",
            PasswordSalt = "Mq6REd3vYBoqGTaoyJ0mHA==",
            Mobile = "+40723145607",
            Address = "999 Elm St, Rome, Italy",
            Birthday = new DateOnly(1983, 12, 7),
            Avatar = "https://michaela-taylor-avatar.jpg",
            EmailNotifications = false,
            Theme = Theme.Light,
            Role = GetMockedRole2(),
            Authorities = GetMockedAuthoritiesListener(),
            CreatedAt = new DateTime(2024, 5, 7, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = new DateTime(2024, 5, 7, 12, 0, 0)
        };

        public static User GetMockedUser8() => new User
        {
            Id = Guid.Parse("14a3d4c4-fb21-4153-9096-d96bda62ee59"),
            Name = "David Anderson",
            Email = "david.anderson@email.com",
            PasswordHash = "+reqhXnGiShHUC5sxP02v23kKaYjedIRfZAYg9/3Mkw=",
            PasswordSalt = "I+vzT0Q0h9v6BokkhHH40g==",
            Mobile = "+40787654308",
            Address = "111 Oak St, Moscow, Russia",
            Birthday = new DateOnly(1992, 4, 23),
            Avatar = "https://david-anderson-avatar.jpg",
            EmailNotifications = false,
            Theme = Theme.Light,
            Role = GetMockedRole3(),
            Authorities = GetMockedAuthoritiesListener(),
            CreatedAt = new DateTime(2024, 5, 8, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = new DateTime(2024, 5, 8, 12, 0, 0)
        };

        public static User GetMockedUser9() => new User
        {
            Id = Guid.Parse("978d41ee-1f38-4c84-8826-8e62bc5ba109"),
            Name = "Sophia Garcia",
            Email = "sophia.garcia@email.com",
            PasswordHash = "vBz8hkle8HsLLhVuCFuQaaYJw4ds0CmxCJBvacBp/pU=",
            PasswordSalt = "8auwB8NbccCHKdS1S+9sUg==",
            Mobile = "+40754321809",
            Address = "333 Pine St, Athens, Greece",
            Birthday = new DateOnly(1998, 7, 30),
            Avatar = "https://sophia-garcia-avatar.jpg",
            EmailNotifications = false,
            Theme = Theme.Light,
            Role = GetMockedRole3(),
            Authorities = GetMockedAuthoritiesListener(),
            CreatedAt = new DateTime(2024, 5, 9, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = new DateTime(2024, 5, 9, 12, 0, 0)
        };

        public static User GetMockedUser10() => new User
        {
            Id = Guid.Parse("da189940-d09e-4587-9665-8efdf10684dd"),
            Name = "Joseph Wilson",
            Email = "joseph.wilson@email.com",
            PasswordHash = "+BiUsbLF9nCTfJJyoZCMQz8yVuD1FNfmWQZ7w8K8ucU=",
            PasswordSalt = "MHN0/ddi3b/HJXWTGm2RNw==",
            Mobile = "+40789012610",
            Address = "555 Elm St, Madrid, Spain",
            Birthday = new DateOnly(1991, 3, 14),
            Avatar = "https://joseph-wilson-avatar.jpg",
            EmailNotifications = false,
            Theme = Theme.Light,
            Role = GetMockedRole3(),
            Authorities = GetMockedAuthoritiesListener(),
            CreatedAt = new DateTime(2024, 5, 10, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = new DateTime(2024, 5, 10, 12, 0, 0)
        };

        public static User GetMockedUser11() => new User
        {
            Id = Guid.Parse("a3daa48b-ece1-430e-a609-32e6c79c232f"),
            Name = "Olivia Martinez",
            Email = "olivia.martinez@email.com",
            PasswordHash = "HevTSA0PtWByNjcE4cuWAHj8Xo15o0DqPrKDtQ/iOro=",
            PasswordSalt = "zhbTGpnXdSQedOoZX3E8Vw==",
            Mobile = "+40723145611",
            Address = "777 Oak St, Tokyo, Japan",
            Birthday = new DateOnly(1999, 10, 17),
            Avatar = "https://olivia-martinez-avatar.jpg",
            EmailNotifications = false,
            Theme = Theme.SystemDefault,
            Role = GetMockedRole3(),
            Authorities = GetMockedAuthoritiesListener(),
            CreatedAt = new DateTime(2024, 5, 11, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static UserDto GetMockedUserDto1() => ToDto(GetMockedUser1());

        public static UserDto GetMockedUserDto2() => ToDto(GetMockedUser2());

        public static UserDto GetMockedUserDto3() => ToDto(GetMockedUser3());

        public static UserDto GetMockedUserDto4() => ToDto(GetMockedUser4());

        public static UserDto GetMockedUserDto5() => ToDto(GetMockedUser5());

        public static UserDto GetMockedUserDto6() => ToDto(GetMockedUser6());

        public static UserDto GetMockedUserDto7() => ToDto(GetMockedUser7());

        public static UserDto GetMockedUserDto8() => ToDto(GetMockedUser8());

        public static UserDto GetMockedUserDto9() => ToDto(GetMockedUser9());

        public static UserDto GetMockedUserDto10() => ToDto(GetMockedUser10());

        public static UserDto GetMockedUserDto11() => ToDto(GetMockedUser11());

        private static UserDto ToDto(User user) => new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Mobile = user.Mobile,
            Address = user.Address,
            Birthday = user.Birthday,
            Avatar = user.Avatar,
            RoleId = user.Role.Id,
            AuthoritiesIds = user.Authorities.Select(authority => authority.Id).ToList(),
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            DeletedAt = user.DeletedAt
        };
    }
}