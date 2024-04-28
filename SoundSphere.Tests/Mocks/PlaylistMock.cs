using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Tests.Mocks
{
    public class PlaylistMock
    {
        private PlaylistMock() { }

        public static IList<Playlist> GetMockedPlaylists() => new List<Playlist> { GetMockedPlaylist1(), GetMockedPlaylist2() };

        public static IList<PlaylistDto> GetMockedPlaylistDtos() => new List<PlaylistDto> { GetMockedPlaylistDto1(), GetMockedPlaylistDto2() };

        public static IList<Playlist> GetMockedActivePlaylists() => GetMockedPlaylists().Where(playlist => playlist.IsActive).ToList();

        public static IList<PlaylistDto> GetMockedActivePlaylistDtos() => GetMockedPlaylistDtos().Where(playlist => playlist.IsActive).ToList();

        public static Playlist GetMockedPlaylist1() => new Playlist
        {
            Id = Guid.Parse("239d050b-b59c-47e0-9e1a-ab5faf6f903e"),
            Title = "playlist_title1",
            User = UserMock.GetMockedUser1(),
            Songs = SongMock.GetMockedSongs1(),
            CreatedAt = new DateTime(2024, 1, 1),
            IsActive = true
        };

        public static Playlist GetMockedPlaylist2() => new Playlist
        {
            Id = Guid.Parse("e9218fdd-8e2b-4573-abc8-84bf1833e974"),
            Title = "playlist_title2",
            User = UserMock.GetMockedUser2(),
            Songs = SongMock.GetMockedSongs2(),
            CreatedAt = new DateTime(2024, 1, 2),
            IsActive = false
        };

        public static Playlist GetMockedPlaylist3() => new Playlist
        {
            Id = Guid.Parse("adb690d7-5aa3-4603-ae97-cf485aec007c"),
            Title = "playlist_title3",
            User = UserMock.GetMockedUser3(),
            Songs = SongMock.GetMockedSongs1(),
            CreatedAt = new DateTime(2024, 1, 3),
            IsActive = true
        };

        public static PlaylistDto GetMockedPlaylistDto1() => new PlaylistDto
        {
            Id = Guid.Parse("239d050b-b59c-47e0-9e1a-ab5faf6f903e"),
            Title = "playlist_title1",
            UserId = Guid.Parse("0a9e546f-38b4-4dbf-a482-24a82169890e"),
            SongsIds = new List<Guid>()
            {
                Guid.Parse("64f534f8-f2d4-4402-95a3-54de48b678a8"),
                Guid.Parse("5185636d-ab67-446e-8fc7-dbca2c50297c")
            },
            CreatedAt = new DateTime(2024, 1, 1),
            IsActive = true
        };

        public static PlaylistDto GetMockedPlaylistDto2() => new PlaylistDto
        {
            Id = Guid.Parse("e9218fdd-8e2b-4573-abc8-84bf1833e974"),
            Title = "playlist_title2",
            UserId = Guid.Parse("31a088bd-6fe8-4226-bd03-f4af698abe83"),
            SongsIds = new List<Guid>()
            {
                Guid.Parse("e33675e4-4ac3-44cc-96f9-76e3a68689a4"),
                Guid.Parse("fc558718-6e35-486b-8990-a955894fe765")
            },
            CreatedAt = new DateTime(2024, 1, 2),
            IsActive = false
        };

        public static PlaylistDto GetMockedPlaylistDto3() => new PlaylistDto
        {
            Id = Guid.Parse("adb690d7-5aa3-4603-ae97-cf485aec007c"),
            Title = "playlist_title3",
            UserId = Guid.Parse("b3692c1c-384a-47ef-a258-106bceb73f0c"),
            SongsIds = new List<Guid>()
            {
                Guid.Parse("64f534f8-f2d4-4402-95a3-54de48b678a8"),
                Guid.Parse("5185636d-ab67-446e-8fc7-dbca2c50297c")
            },
            CreatedAt = new DateTime(2024, 1, 3),
            IsActive = true
        };
    }
}