using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Dtos.Request.Models;
using SoundSphere.Database.Entities;
using static SoundSphere.Tests.Mocks.UserMock;
using static SoundSphere.Tests.Mocks.SongMock;

namespace SoundSphere.Tests.Mocks
{
    public class PlaylistMock
    {
        private PlaylistMock() { }

        public static IList<Playlist> GetMockedPlaylists() => new List<Playlist> 
        { 
            GetMockedPlaylist1(), GetMockedPlaylist2(), GetMockedPlaylist3(), GetMockedPlaylist4(), GetMockedPlaylist5(), GetMockedPlaylist6(), GetMockedPlaylist7(), GetMockedPlaylist8(), GetMockedPlaylist9(), GetMockedPlaylist10(),
            GetMockedPlaylist11(), GetMockedPlaylist12(), GetMockedPlaylist13(), GetMockedPlaylist14(), GetMockedPlaylist15(), GetMockedPlaylist16(), GetMockedPlaylist17(), GetMockedPlaylist18(), GetMockedPlaylist19(), GetMockedPlaylist20(),
            GetMockedPlaylist21(), GetMockedPlaylist22(), GetMockedPlaylist23()
        };

        public static IList<PlaylistDto> GetMockedPlaylistDtos() => GetMockedPlaylists().Select(ToDto).ToList();

        public static IList<Playlist> GetMockedActivePlaylists() => GetMockedPlaylists().Where(playlist => playlist.IsActive).ToList();

        public static IList<PlaylistDto> GetMockedActivePlaylistDtos() => GetMockedPlaylistDtos().Where(playlist => playlist.IsActive).ToList();

        public static IList<Playlist> GetMockedPaginatedPlaylists() => new List<Playlist> { GetMockedPlaylist18(), GetMockedPlaylist19(), GetMockedPlaylist20(), GetMockedPlaylist21(), GetMockedPlaylist22() };

        public static IList<PlaylistDto> GetMockedPaginatedPlaylistDtos() => new List<PlaylistDto> { GetMockedPlaylistDto18(), GetMockedPlaylistDto19(), GetMockedPlaylistDto20(), GetMockedPlaylistDto21(), GetMockedPlaylistDto22() };

        public static IList<Playlist> GetMockedActivePaginatedPlaylists() => GetMockedPaginatedPlaylists().Where(playlist => playlist.IsActive).ToList();

        public static IList<PlaylistDto> GetMockedActivePaginatedPlaylistDtos() => GetMockedPaginatedPlaylistDtos().Where(playlist => playlist.IsActive).ToList();

        public static PlaylistPaginationRequest GetMockedPlaylistsPaginationRequest() => new PlaylistPaginationRequest(
            SortCriteria: new Dictionary<PlaylistSortCriterion, SortOrder> { { PlaylistSortCriterion.ByCreatedDate, SortOrder.Ascending }, { PlaylistSortCriterion.ByTitle, SortOrder.Ascending } },
            SearchCriteria: new List<PlaylistSearchCriterion> { PlaylistSearchCriterion.ByUserName, PlaylistSearchCriterion.ByTitle, PlaylistSearchCriterion.ByCreatedDateRange },
            Title: "A",
            DateRange: new DateTimeRange(new DateTime(1950, 1, 1), new DateTime(2024, 5, 31)),
            UserName: "A",
            SongId: Guid.Parse("64f534f8-f2d4-4402-95a3-54de48b678a8"));

        public static Playlist GetMockedPlaylist1() => new Playlist
        {
            Id = Guid.Parse("239d050b-b59c-47e0-9e1a-ab5faf6f903e"),
            Title = "Playlist 1",
            User = GetMockedUser1(),
            Songs = GetMockedSongs1(),
            CreatedAt = new DateTime(2024, 5, 7),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist2() => new Playlist
        {
            Id = Guid.Parse("67b394ad-aeba-4804-be29-71fc4ebd37c8"),
            Title = "Playlist 2",
            User = GetMockedUser2(),
            Songs = GetMockedSongs2(),
            CreatedAt = new DateTime(2024, 5, 6),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist3() => new Playlist
        {
            Id = Guid.Parse("25fc284d-9ad3-462e-8947-9a46f1bb7bde"),
            Title = "Playlist 3",
            User = GetMockedUser3(),
            Songs = GetMockedSongs3(),
            CreatedAt = new DateTime(2024, 5, 5),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist4() => new Playlist
        {
            Id = Guid.Parse("db73836b-b090-4b9e-979c-f02e712a4941"),
            Title = "Playlist 4",
            User = GetMockedUser4(),
            Songs = GetMockedSongs4(),
            CreatedAt = new DateTime(2024, 5, 4),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist5() => new Playlist
        {
            Id = Guid.Parse("883b32f5-6f45-4e36-aae4-271b40322445"),
            Title = "Playlist 5",
            User = GetMockedUser5(),
            Songs = GetMockedSongs5(),
            CreatedAt = new DateTime(2024, 5, 3),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist6() => new Playlist
        {
            Id = Guid.Parse("a9f4b39f-c273-41e1-a82f-544e4c287192"),
            Title = "Playlist 6",
            User = GetMockedUser6(),
            Songs = GetMockedSongs6(),
            CreatedAt = new DateTime(2024, 5, 2),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist7() => new Playlist
        {
            Id = Guid.Parse("49ea6e91-4033-49bd-9275-24292ee36bb8"),
            Title = "Playlist 7",
            User = GetMockedUser7(),
            Songs = GetMockedSongs7(),
            CreatedAt = new DateTime(2024, 5, 1),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist8() => new Playlist
        {
            Id = Guid.Parse("f1d2f01e-dcb1-46e5-bb3c-a6a0cbd280f3"),
            Title = "Playlist 8",
            User = GetMockedUser8(),
            Songs = GetMockedSongs8(),
            CreatedAt = new DateTime(2024, 4, 30),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist9() => new Playlist
        {
            Id = Guid.Parse("20a85f02-c737-4f5e-ab72-e1512e1177d1"),
            Title = "Playlist 9",
            User = GetMockedUser9(),
            Songs = GetMockedSongs9(),
            CreatedAt = new DateTime(2024, 4, 29),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist10() => new Playlist
        {
            Id = Guid.Parse("c012c843-ca72-4121-b818-4b38d03be24b"),
            Title = "Playlist 10",
            User = GetMockedUser10(),
            Songs = GetMockedSongs10(),
            CreatedAt = new DateTime(2024, 4, 28),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist11() => new Playlist
        {
            Id = Guid.Parse("c6c1b55d-b30f-49e1-af8d-beeff7b76b24"),
            Title = "Playlist 11",
            User = GetMockedUser1(),
            Songs = GetMockedSongs11(),
            CreatedAt = new DateTime(2024, 4, 27),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist12() => new Playlist
        {
            Id = Guid.Parse("55fca191-e4fc-43da-a576-1a867256e65d"),
            Title = "Playlist 12",
            User = GetMockedUser2(),
            Songs = GetMockedSongs12(),
            CreatedAt = new DateTime(2024, 4, 26),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist13() => new Playlist
        {
            Id = Guid.Parse("514b9ab3-0a63-4219-8989-9b3dd6422005"),
            Title = "Playlist 13",
            User = GetMockedUser3(),
            Songs = GetMockedSongs13(),
            CreatedAt = new DateTime(2024, 4, 25),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist14() => new Playlist
        {
            Id = Guid.Parse("e8560429-a622-473a-9646-bfcaac9cc934"),
            Title = "Playlist 14",
            User = GetMockedUser4(),
            Songs = GetMockedSongs14(),
            CreatedAt = new DateTime(2024, 4, 24),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist15() => new Playlist
        {
            Id = Guid.Parse("677abd5c-33e0-4553-9be7-f8be825e7afe"),
            Title = "Playlist 15",
            User = GetMockedUser5(),
            Songs = GetMockedSongs15(),
            CreatedAt = new DateTime(2024, 4, 23),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist16() => new Playlist
        {
            Id = Guid.Parse("1b6de32d-f60e-45ee-9d96-dcec12fe6525"),
            Title = "Playlist 16",
            User = GetMockedUser6(),
            Songs = GetMockedSongs16(),
            CreatedAt = new DateTime(2024, 4, 22),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist17() => new Playlist
        {
            Id = Guid.Parse("4c8747b0-e5a6-48e4-af5c-bdebbe041daa"),
            Title = "Playlist 17",
            User = GetMockedUser7(),
            Songs = GetMockedSongs17(),
            CreatedAt = new DateTime(2024, 4, 21),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist18() => new Playlist
        {
            Id = Guid.Parse("ab9ee809-25a5-4c16-8f04-1c9ee63c7784"),
            Title = "Playlist 18",
            User = GetMockedUser8(),
            Songs = GetMockedSongs18(),
            CreatedAt = new DateTime(2024, 4, 20),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist19() => new Playlist
        {
            Id = Guid.Parse("01de5998-2c9f-40c5-8766-0d17103da4a0"),
            Title = "Playlist 19",
            User = GetMockedUser9(),
            Songs = GetMockedSongs19(),
            CreatedAt = new DateTime(2024, 4, 19),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist20() => new Playlist
        {
            Id = Guid.Parse("309ba8a0-3a2a-44fd-b359-1058defb3c75"),
            Title = "Playlist 20",
            User = GetMockedUser10(),
            Songs = GetMockedSongs20(),
            CreatedAt = new DateTime(2024, 4, 18),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist21() => new Playlist
        {
            Id = Guid.Parse("ca6f3c65-8051-48a5-8c36-adbfc1a6ac1c"),
            Title = "Playlist 21",
            User = GetMockedUser1(),
            Songs = GetMockedSongs21(),
            CreatedAt = new DateTime(2024, 4, 17),
            IsActive = false
        };

        public static Playlist GetMockedPlaylist22() => new Playlist
        {
            Id = Guid.Parse("80759f9e-c756-4ae1-abcc-c5b300c4890f"),
            Title = "Playlist 22",
            User = GetMockedUser2(),
            Songs = GetMockedSongs22(),
            CreatedAt = new DateTime(2024, 4, 16),
            IsActive = false
        };

        public static Playlist GetMockedPlaylist23() => new Playlist
        {
            Id = Guid.Parse("07919c9c-2642-4a31-8200-e978052c4d7e"),
            Title = "Playlist 23",
            User = GetMockedUser3(),
            Songs = GetMockedSongs23(),
            CreatedAt = new DateTime(2024, 4, 15),
            IsActive = false
        };

        public static Playlist GetMockedPlaylist24() => new Playlist
        {
            Id = Guid.Parse("78b11337-713d-4994-8e49-42b06bde9010"),
            Title = "Playlist 24",
            User = GetMockedUser4(),
            Songs = GetMockedSongs1(),
            CreatedAt = new DateTime(2024, 4, 14),
            IsActive = true
        };

        public static PlaylistDto GetMockedPlaylistDto1() => ToDto(GetMockedPlaylist1());

        public static PlaylistDto GetMockedPlaylistDto2() => ToDto(GetMockedPlaylist2());

        public static PlaylistDto GetMockedPlaylistDto3() => ToDto(GetMockedPlaylist3());

        public static PlaylistDto GetMockedPlaylistDto4() => ToDto(GetMockedPlaylist4());

        public static PlaylistDto GetMockedPlaylistDto5() => ToDto(GetMockedPlaylist5());

        public static PlaylistDto GetMockedPlaylistDto6() => ToDto(GetMockedPlaylist6());

        public static PlaylistDto GetMockedPlaylistDto7() => ToDto(GetMockedPlaylist7());

        public static PlaylistDto GetMockedPlaylistDto8() => ToDto(GetMockedPlaylist8());

        public static PlaylistDto GetMockedPlaylistDto9() => ToDto(GetMockedPlaylist9());

        public static PlaylistDto GetMockedPlaylistDto10() => ToDto(GetMockedPlaylist10());

        public static PlaylistDto GetMockedPlaylistDto11() => ToDto(GetMockedPlaylist11());

        public static PlaylistDto GetMockedPlaylistDto12() => ToDto(GetMockedPlaylist12());

        public static PlaylistDto GetMockedPlaylistDto13() => ToDto(GetMockedPlaylist13());

        public static PlaylistDto GetMockedPlaylistDto14() => ToDto(GetMockedPlaylist14());

        public static PlaylistDto GetMockedPlaylistDto15() => ToDto(GetMockedPlaylist15());

        public static PlaylistDto GetMockedPlaylistDto16() => ToDto(GetMockedPlaylist16());

        public static PlaylistDto GetMockedPlaylistDto17() => ToDto(GetMockedPlaylist17());

        public static PlaylistDto GetMockedPlaylistDto18() => ToDto(GetMockedPlaylist18());

        public static PlaylistDto GetMockedPlaylistDto19() => ToDto(GetMockedPlaylist19());

        public static PlaylistDto GetMockedPlaylistDto20() => ToDto(GetMockedPlaylist20());

        public static PlaylistDto GetMockedPlaylistDto21() => ToDto(GetMockedPlaylist21());

        public static PlaylistDto GetMockedPlaylistDto22() => ToDto(GetMockedPlaylist22());

        public static PlaylistDto GetMockedPlaylistDto23() => ToDto(GetMockedPlaylist23());

        public static PlaylistDto GetMockedPlaylistDto24() => ToDto(GetMockedPlaylist24());

        private static PlaylistDto ToDto(Playlist playlist) => new PlaylistDto
        {
            Id = playlist.Id,
            Title = playlist.Title,
            UserId = playlist.User.Id,
            SongsIds = playlist.Songs.Select(song => song.Id).ToList(),
            CreatedAt = playlist.CreatedAt,
            IsActive = playlist.IsActive
        };
    }
}