using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;

namespace SoundSphere.Tests.Mocks
{
    public class SongMock
    {
        private SongMock() { }

        public static IList<Song> GetMockedSongs() => GetMockedSongs1().Concat(GetMockedSongs2()).ToList();

        public static IList<SongDto> GetMockedSongDtos() => GetMockedSongDtos1().Concat(GetMockedSongDtos2()).ToList();

        public static IList<Song> GetMockedActiveSongs() => GetMockedSongs().Where(song => song.IsActive).ToList();

        public static IList<SongDto> GetMockedActiveSongDtos() => GetMockedSongDtos().Where(song => song.IsActive).ToList();

        public static IList<Song> GetMockedSongs1() => new List<Song> { GetMockedSong1(), GetMockedSong2() };

        public static IList<Song> GetMockedSongs2() => new List<Song> { GetMockedSong3(), GetMockedSong4() };

        public static IList<SongDto> GetMockedSongDtos1() => new List<SongDto> { GetMockedSongDto1(), GetMockedSongDto2() };

        public static IList<SongDto> GetMockedSongDtos2() => new List<SongDto> { GetMockedSongDto3(), GetMockedSongDto4() };

        public static Song GetMockedSong1() => new Song
        {
            Id = Guid.Parse("64f534f8-f2d4-4402-95a3-54de48b678a8"),
            Title = "song_title1",
            ImageUrl = "https://song_imageurl1.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2020, 1, 1),
            DurationSeconds = 180,
            Album = AlbumMock.GetMockedAlbum1(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist1() },
            SimilarSongs = new List<SongLink>(),
            IsActive = true
        };

        public static Song GetMockedSong2() => new Song
        {
            Id = Guid.Parse("5185636d-ab67-446e-8fc7-dbca2c50297c"),
            Title = "song_title2",
            ImageUrl = "https://song_imageurl2.jpg",
            Genre = GenreType.Rock,
            ReleaseDate = new DateOnly(2020, 1, 2),
            DurationSeconds = 185,
            Album = AlbumMock.GetMockedAlbum2(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist2() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink
                {
                    SongId = Guid.Parse("5185636d-ab67-446e-8fc7-dbca2c50297c"),
                    SimilarSongId = Guid.Parse("64f534f8-f2d4-4402-95a3-54de48b678a8")
                }
            },
            IsActive = true
        };

        public static Song GetMockedSong3() => new Song
        {
            Id = Guid.Parse("e33675e4-4ac3-44cc-96f9-76e3a68689a4"),
            Title = "song_title3",
            ImageUrl = "https://song_imageurl3.jpg",
            Genre = GenreType.Rnb,
            ReleaseDate = new DateOnly(2020, 1, 3),
            DurationSeconds = 190,
            Album = AlbumMock.GetMockedAlbum1(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist1() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink
                {
                    SongId = Guid.Parse("e33675e4-4ac3-44cc-96f9-76e3a68689a4"),
                    SimilarSongId = Guid.Parse("5185636d-ab67-446e-8fc7-dbca2c50297c")
                }
            },
            IsActive = true
        };

        public static Song GetMockedSong4() => new Song
        {
            Id = Guid.Parse("fc558718-6e35-486b-8990-a955894fe765"),
            Title = "song_title4",
            ImageUrl = "https://song_imageurl4.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2020, 1, 4),
            DurationSeconds = 195,
            Album = AlbumMock.GetMockedAlbum2(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist2() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink
                {
                    SongId = Guid.Parse("fc558718-6e35-486b-8990-a955894fe765"),
                    SimilarSongId = Guid.Parse("e33675e4-4ac3-44cc-96f9-76e3a68689a4")
                }
            },
            IsActive = false
        };

        public static Song GetMockedSong5() => new Song
        {
            Id = Guid.Parse("c5617289-4a05-4839-b1a0-710b011c6344"),
            Title = "song_title5",
            ImageUrl = "https://song_imageurl5.jpg",
            Genre = GenreType.Dance,
            ReleaseDate = new DateOnly(2020, 1, 5),
            DurationSeconds = 200,
            Album = AlbumMock.GetMockedAlbum1(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist1() },
            SimilarSongs = new List<SongLink>()
            {
                new SongLink
                {
                    SongId = Guid.Parse("c5617289-4a05-4839-b1a0-710b011c6344"),
                    SimilarSongId = Guid.Parse("fc558718-6e35-486b-8990-a955894fe765")
                }
            },
            IsActive = true
        };

        public static SongDto GetMockedSongDto1() => new SongDto
        {
            Id = Guid.Parse("64f534f8-f2d4-4402-95a3-54de48b678a8"),
            Title = "song_title1",
            ImageUrl = "https://song_imageurl1.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2020, 1, 1),
            DurationSeconds = 180,
            AlbumId = Guid.Parse("6ee76a77-2be4-42e3-8417-e60d282cffcb"),
            ArtistsIds = new List<Guid> { Guid.Parse("d4f7f9d2-472e-488e-b7ef-73d169ba2bf5") },
            SimilarSongsIds = new List<Guid>(),
            IsActive = true
        };

        public static SongDto GetMockedSongDto2() => new SongDto
        {
            Id = Guid.Parse("5185636d-ab67-446e-8fc7-dbca2c50297c"),
            Title = "song_title2",
            ImageUrl = "https://song_imageurl2.jpg",
            Genre = GenreType.Rock,
            ReleaseDate = new DateOnly(2020, 1, 2),
            DurationSeconds = 185,
            AlbumId = Guid.Parse("543c9236-443d-4526-b53b-b02f33f284ab"),
            ArtistsIds = new List<Guid> { Guid.Parse("0f9d6cf2-497e-48ff-b1fa-440678951c28") },
            SimilarSongsIds = new List<Guid> { Guid.Parse("64f534f8-f2d4-4402-95a3-54de48b678a8") },
            IsActive = true
        };

        public static SongDto GetMockedSongDto3() => new SongDto
        {
            Id = Guid.Parse("e33675e4-4ac3-44cc-96f9-76e3a68689a4"),
            Title = "song_title3",
            ImageUrl = "https://song_imageurl3.jpg",
            Genre = GenreType.Rnb,
            ReleaseDate = new DateOnly(2020, 1, 3),
            DurationSeconds = 190,
            AlbumId = Guid.Parse("6ee76a77-2be4-42e3-8417-e60d282cffcb"),
            ArtistsIds = new List<Guid> { Guid.Parse("d4f7f9d2-472e-488e-b7ef-73d169ba2bf5") },
            SimilarSongsIds = new List<Guid> { Guid.Parse("5185636d-ab67-446e-8fc7-dbca2c50297c") },
            IsActive = true
        };

        public static SongDto GetMockedSongDto4() => new SongDto
        {
            Id = Guid.Parse("fc558718-6e35-486b-8990-a955894fe765"),
            Title = "song_title4",
            ImageUrl = "https://song_imageurl4.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2020, 1, 4),
            DurationSeconds = 195,
            AlbumId = Guid.Parse("543c9236-443d-4526-b53b-b02f33f284ab"),
            ArtistsIds = new List<Guid> { Guid.Parse("0f9d6cf2-497e-48ff-b1fa-440678951c28") },
            SimilarSongsIds = new List<Guid> { Guid.Parse("e33675e4-4ac3-44cc-96f9-76e3a68689a4") },
            IsActive = false
        };

        public static SongDto GetMockedSongDto5() => new SongDto
        {
            Id = Guid.Parse("c5617289-4a05-4839-b1a0-710b011c6344"),
            Title = "song_title5",
            ImageUrl = "https://song_imageurl5.jpg",
            Genre = GenreType.Dance,
            ReleaseDate = new DateOnly(2020, 1, 5),
            DurationSeconds = 200,
            AlbumId = Guid.Parse("6ee76a77-2be4-42e3-8417-e60d282cffcb"),
            ArtistsIds = new List<Guid> { Guid.Parse("d4f7f9d2-472e-488e-b7ef-73d169ba2bf5") },
            SimilarSongsIds = new List<Guid> { Guid.Parse("fc558718-6e35-486b-8990-a955894fe765") },
            IsActive = true
        };
    }
}