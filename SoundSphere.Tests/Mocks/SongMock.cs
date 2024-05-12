using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Dtos.Request.Models;
using SoundSphere.Database.Entities;

namespace SoundSphere.Tests.Mocks
{
    public class SongMock
    {
        private SongMock() { }

        public static IList<Song> GetMockedSongs() => new List<Song>
        {
            GetMockedSong1(), GetMockedSong2(), GetMockedSong3(), GetMockedSong4(), GetMockedSong5(), GetMockedSong6(), GetMockedSong7(), GetMockedSong8(), GetMockedSong9(), GetMockedSong10(),
            GetMockedSong11(), GetMockedSong12(), GetMockedSong13(), GetMockedSong14(), GetMockedSong15(), GetMockedSong16(), GetMockedSong17(), GetMockedSong18(), GetMockedSong19(), GetMockedSong20(),
            GetMockedSong21(), GetMockedSong22(), GetMockedSong23(), GetMockedSong24(), GetMockedSong25(), GetMockedSong26(), GetMockedSong27(), GetMockedSong28(), GetMockedSong29(), GetMockedSong30(),
            GetMockedSong31(), GetMockedSong32(), GetMockedSong33(), GetMockedSong34(), GetMockedSong35(), GetMockedSong36(), GetMockedSong37(), GetMockedSong38(), GetMockedSong39(), GetMockedSong40(),
            GetMockedSong41(), GetMockedSong42(), GetMockedSong43(), GetMockedSong44(), GetMockedSong45(), GetMockedSong46(), GetMockedSong47(), GetMockedSong48(), GetMockedSong49(), GetMockedSong50(),
            GetMockedSong51(), GetMockedSong52(), GetMockedSong53(), GetMockedSong54(), GetMockedSong55(), GetMockedSong56(), GetMockedSong57(), GetMockedSong58(), GetMockedSong59(), GetMockedSong60(),
            GetMockedSong61(), GetMockedSong62(), GetMockedSong63(), GetMockedSong64(), GetMockedSong65(), GetMockedSong66(), GetMockedSong67(), GetMockedSong68(), GetMockedSong69(), GetMockedSong70(),
            GetMockedSong71(), GetMockedSong72(), GetMockedSong73(), GetMockedSong74(), GetMockedSong75(), GetMockedSong76(), GetMockedSong77(), GetMockedSong78(), GetMockedSong79(), GetMockedSong80(),
            GetMockedSong81(), GetMockedSong82(), GetMockedSong83(), GetMockedSong84(), GetMockedSong85(), GetMockedSong86(), GetMockedSong87(), GetMockedSong88(), GetMockedSong89()
        };

        public static IList<SongDto> GetMockedSongDtos() => GetMockedSongs().Select(ToDto).ToList();

        public static IList<Song> GetMockedActiveSongs() => GetMockedSongs().Where(song => song.IsActive).ToList();

        public static IList<SongDto> GetMockedActiveSongDtos() => GetMockedSongDtos().Where(song => song.IsActive).ToList();

        public static IList<Song> GetMockedPaginatedSongs() => new List<Song> { GetMockedSong84(), GetMockedSong85(), GetMockedSong86(), GetMockedSong87(), GetMockedSong88(), GetMockedSong89() };

        public static IList<SongDto> GetMockedPaginatedSongDtos() => new List<SongDto> { GetMockedSongDto84(), GetMockedSongDto85(), GetMockedSongDto86(), GetMockedSongDto87(), GetMockedSongDto88(), GetMockedSongDto89() };

        public static IList<Song> GetMockedActivePaginatedSongs() => GetMockedPaginatedSongs().Where(song => song.IsActive).ToList();

        public static IList<SongDto> GetMockedActivePaginatedSongDtos() => GetMockedPaginatedSongDtos().Where(song => song.IsActive).ToList();

        public static IList<Song> GetMockedSongs1() => new List<Song> { GetMockedSong1(), GetMockedSong2(), GetMockedSong3(), GetMockedSong4() };

        public static IList<Song> GetMockedSongs2() => new List<Song> { GetMockedSong5(), GetMockedSong6(), GetMockedSong7(), GetMockedSong8() };

        public static IList<Song> GetMockedSongs3() => new List<Song> { GetMockedSong9(), GetMockedSong10(), GetMockedSong11(), GetMockedSong12() };

        public static IList<Song> GetMockedSongs4() => new List<Song> { GetMockedSong13(), GetMockedSong14(), GetMockedSong15(), GetMockedSong16() };

        public static IList<Song> GetMockedSongs5() => new List<Song> { GetMockedSong17(), GetMockedSong18(), GetMockedSong19(), GetMockedSong20() };

        public static IList<Song> GetMockedSongs6() => new List<Song> { GetMockedSong21(), GetMockedSong22(), GetMockedSong23(), GetMockedSong24() };

        public static IList<Song> GetMockedSongs7() => new List<Song> { GetMockedSong25(), GetMockedSong26(), GetMockedSong27(), GetMockedSong28() };

        public static IList<Song> GetMockedSongs8() => new List<Song> { GetMockedSong29(), GetMockedSong30(), GetMockedSong31(), GetMockedSong32() };

        public static IList<Song> GetMockedSongs9() => new List<Song> { GetMockedSong33(), GetMockedSong34(), GetMockedSong35(), GetMockedSong36() };

        public static IList<Song> GetMockedSongs10() => new List<Song> { GetMockedSong37(), GetMockedSong38(), GetMockedSong39(), GetMockedSong40() };

        public static IList<Song> GetMockedSongs11() => new List<Song> { GetMockedSong41(), GetMockedSong42(), GetMockedSong43(), GetMockedSong44() };

        public static IList<Song> GetMockedSongs12() => new List<Song> { GetMockedSong45(), GetMockedSong46(), GetMockedSong47(), GetMockedSong48() };

        public static IList<Song> GetMockedSongs13() => new List<Song> { GetMockedSong49(), GetMockedSong50(), GetMockedSong51(), GetMockedSong52() };

        public static IList<Song> GetMockedSongs14() => new List<Song> { GetMockedSong53(), GetMockedSong54(), GetMockedSong55(), GetMockedSong56() };

        public static IList<Song> GetMockedSongs15() => new List<Song> { GetMockedSong57(), GetMockedSong58(), GetMockedSong59(), GetMockedSong60() };

        public static IList<Song> GetMockedSongs16() => new List<Song> { GetMockedSong61(), GetMockedSong62(), GetMockedSong63(), GetMockedSong64() };

        public static IList<Song> GetMockedSongs17() => new List<Song> { GetMockedSong65(), GetMockedSong66(), GetMockedSong67(), GetMockedSong68() };

        public static IList<Song> GetMockedSongs18() => new List<Song> { GetMockedSong69(), GetMockedSong70(), GetMockedSong71(), GetMockedSong72() };

        public static IList<Song> GetMockedSongs19() => new List<Song> { GetMockedSong73(), GetMockedSong74(), GetMockedSong75(), GetMockedSong76() };

        public static IList<Song> GetMockedSongs20() => new List<Song> { GetMockedSong77(), GetMockedSong78(), GetMockedSong79(), GetMockedSong80() };

        public static IList<Song> GetMockedSongs21() => new List<Song> { GetMockedSong81(), GetMockedSong82(), GetMockedSong83() };

        public static IList<Song> GetMockedSongs22() => new List<Song> { GetMockedSong84(), GetMockedSong85(), GetMockedSong86() };

        public static IList<Song> GetMockedSongs23() => new List<Song> { GetMockedSong87(), GetMockedSong88(), GetMockedSong89() };

        public static IList<SongDto> GetMockedSongDtos1() => new List<SongDto> { GetMockedSongDto1(), GetMockedSongDto2(), GetMockedSongDto3(), GetMockedSongDto4() };

        public static IList<SongDto> GetMockedSongDtos2() => new List<SongDto> { GetMockedSongDto5(), GetMockedSongDto6(), GetMockedSongDto7(), GetMockedSongDto8() };

        public static IList<SongDto> GetMockedSongDtos3() => new List<SongDto> { GetMockedSongDto9(), GetMockedSongDto10(), GetMockedSongDto11(), GetMockedSongDto12() };

        public static IList<SongDto> GetMockedSongDtos4() => new List<SongDto> { GetMockedSongDto13(), GetMockedSongDto14(), GetMockedSongDto15(), GetMockedSongDto16() };

        public static IList<SongDto> GetMockedSongDtos5() => new List<SongDto> { GetMockedSongDto17(), GetMockedSongDto18(), GetMockedSongDto19(), GetMockedSongDto20() };

        public static IList<SongDto> GetMockedSongDtos6() => new List<SongDto> { GetMockedSongDto21(), GetMockedSongDto22(), GetMockedSongDto23(), GetMockedSongDto24() };

        public static IList<SongDto> GetMockedSongDtos7() => new List<SongDto> { GetMockedSongDto25(), GetMockedSongDto26(), GetMockedSongDto27(), GetMockedSongDto28() };

        public static IList<SongDto> GetMockedSongDtos8() => new List<SongDto> { GetMockedSongDto29(), GetMockedSongDto30(), GetMockedSongDto31(), GetMockedSongDto32() };

        public static IList<SongDto> GetMockedSongDtos9() => new List<SongDto> { GetMockedSongDto33(), GetMockedSongDto34(), GetMockedSongDto35(), GetMockedSongDto36() };

        public static IList<SongDto> GetMockedSongDtos10() => new List<SongDto> { GetMockedSongDto37(), GetMockedSongDto38(), GetMockedSongDto39(), GetMockedSongDto40() };

        public static IList<SongDto> GetMockedSongDtos11() => new List<SongDto> { GetMockedSongDto41(), GetMockedSongDto42(), GetMockedSongDto43(), GetMockedSongDto44() };

        public static IList<SongDto> GetMockedSongDtos12() => new List<SongDto> { GetMockedSongDto45(), GetMockedSongDto46(), GetMockedSongDto47(), GetMockedSongDto48() };

        public static IList<SongDto> GetMockedSongDtos13() => new List<SongDto> { GetMockedSongDto49(), GetMockedSongDto50(), GetMockedSongDto51(), GetMockedSongDto52() };

        public static IList<SongDto> GetMockedSongDtos14() => new List<SongDto> { GetMockedSongDto53(), GetMockedSongDto54(), GetMockedSongDto55(), GetMockedSongDto56() };

        public static IList<SongDto> GetMockedSongDtos15() => new List<SongDto> { GetMockedSongDto57(), GetMockedSongDto58(), GetMockedSongDto59(), GetMockedSongDto60() };

        public static IList<SongDto> GetMockedSongDtos16() => new List<SongDto> { GetMockedSongDto61(), GetMockedSongDto62(), GetMockedSongDto63(), GetMockedSongDto64() };

        public static IList<SongDto> GetMockedSongDtos17() => new List<SongDto> { GetMockedSongDto65(), GetMockedSongDto66(), GetMockedSongDto67(), GetMockedSongDto68() };

        public static IList<SongDto> GetMockedSongDtos18() => new List<SongDto> { GetMockedSongDto69(), GetMockedSongDto70(), GetMockedSongDto71(), GetMockedSongDto72() };

        public static IList<SongDto> GetMockedSongDtos19() => new List<SongDto> { GetMockedSongDto73(), GetMockedSongDto74(), GetMockedSongDto75(), GetMockedSongDto76() };

        public static IList<SongDto> GetMockedSongDtos20() => new List<SongDto> { GetMockedSongDto77(), GetMockedSongDto78(), GetMockedSongDto79(), GetMockedSongDto80() };

        public static IList<SongDto> GetMockedSongDtos21() => new List<SongDto> { GetMockedSongDto81(), GetMockedSongDto82(), GetMockedSongDto83() };

        public static IList<SongDto> GetMockedSongDtos22() => new List<SongDto> { GetMockedSongDto84(), GetMockedSongDto85(), GetMockedSongDto86() };

        public static IList<SongDto> GetMockedSongDtos23() => new List<SongDto> { GetMockedSongDto87(), GetMockedSongDto88(), GetMockedSongDto89() };

        public static SongPaginationRequest GetMockedPaginationRequest() => new SongPaginationRequest
        {
            SortCriteria = new Dictionary<SongSortCriterion, SortOrder> { { SongSortCriterion.ByTitle, SortOrder.Ascending }, { SongSortCriterion.ByReleaseDate, SortOrder.Ascending } },
            SearchCriteria = new List<SongSearchCriterion> { SongSearchCriterion.ByTitle, SongSearchCriterion.ByGenre, SongSearchCriterion.ByReleaseDateRange, SongSearchCriterion.ByDurationSecondsRange, SongSearchCriterion.ByAlbumTitle, SongSearchCriterion.ByArtistName },
            Title = "A",
            Genre = GenreType.Pop,
            DateRange = new DateRange { StartDate = new DateOnly(1950, 1, 1), EndDate = new DateOnly(2024, 1, 1) },
            DurationRange = new DurationRange { MinSeconds = 150, MaxSeconds = 250 },
            AlbumTitle = "A",
            ArtistId = Guid.Parse("4e75ecdd-aafe-4c35-836b-1b83fc7b8f88")
        };

        public static Song GetMockedSong1() => new Song
        {
            Id = Guid.Parse("64f534f8-f2d4-4402-95a3-54de48b678a8"),
            Title = "Echo of Silence",
            ImageUrl = "https://example.com/images/echo-of-silence.jpg",
            Genre = GenreType.Rnb,
            ReleaseDate = new DateOnly(2016, 11, 26),
            DurationSeconds = 221,
            Album = AlbumMock.GetMockedAlbum19(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist1() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("64f534f8-f2d4-4402-95a3-54de48b678a8"), SimilarSongId = Guid.Parse("278cfa5a-6f44-420e-9930-07da6c43a6ad") },
                new SongLink { SongId = Guid.Parse("64f534f8-f2d4-4402-95a3-54de48b678a8"), SimilarSongId = Guid.Parse("7ef7351b-912e-4a64-ba6d-cfdfcb7d56af") }
            },
            IsActive = true
        };

        public static Song GetMockedSong2() => new Song
        {
            Id = Guid.Parse("278cfa5a-6f44-420e-9930-07da6c43a6ad"),
            Title = "Light Show",
            ImageUrl = "https://example.com/images/light-show.jpg", 
            Genre = GenreType.Rnb,
            ReleaseDate = new DateOnly(2016, 11, 28),
            DurationSeconds = 199,
            Album = AlbumMock.GetMockedAlbum19(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist1() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("278cfa5a-6f44-420e-9930-07da6c43a6ad"), SimilarSongId = Guid.Parse("7ef7351b-912e-4a64-ba6d-cfdfcb7d56af") },
                new SongLink { SongId = Guid.Parse("278cfa5a-6f44-420e-9930-07da6c43a6ad"), SimilarSongId = Guid.Parse("03b3fb9f-38af-4074-8ab5-b9644ab44397") }
            },
            IsActive = true
        };

        public static Song GetMockedSong3() => new Song
        {
            Id = Guid.Parse("7ef7351b-912e-4a64-ba6d-cfdfcb7d56af"),
            Title = "Twilight Zone",
            ImageUrl = "https://example.com/images/twilight-zone.jpg",
            Genre = GenreType.Rnb,
            ReleaseDate = new DateOnly(2016, 11, 25),
            DurationSeconds = 205,
            Album = AlbumMock.GetMockedAlbum19(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist1() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("7ef7351b-912e-4a64-ba6d-cfdfcb7d56af"), SimilarSongId = Guid.Parse("03b3fb9f-38af-4074-8ab5-b9644ab44397") },
                new SongLink { SongId = Guid.Parse("7ef7351b-912e-4a64-ba6d-cfdfcb7d56af"), SimilarSongId = Guid.Parse("3aff8c17-3c98-44ed-a849-1976d2c4a91a") }
            },
            IsActive = true
        };

        public static Song GetMockedSong4() => new Song
        {
            Id = Guid.Parse("03b3fb9f-38af-4074-8ab5-b9644ab44397"),
            Title = "Dark Paradise",
            ImageUrl = "https://example.com/images/dark-paradise.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2012, 1, 27),
            DurationSeconds = 240,
            Album = AlbumMock.GetMockedAlbum3(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist2() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("03b3fb9f-38af-4074-8ab5-b9644ab44397"), SimilarSongId = Guid.Parse("3aff8c17-3c98-44ed-a849-1976d2c4a91a") },
                new SongLink { SongId = Guid.Parse("03b3fb9f-38af-4074-8ab5-b9644ab44397"), SimilarSongId = Guid.Parse("e7b0024e-cc97-46a8-bd3a-450607eebe3c") }
            },
            IsActive = true
        };

        public static Song GetMockedSong5() => new Song
        {
            Id = Guid.Parse("3aff8c17-3c98-44ed-a849-1976d2c4a91a"),
            Title = "Lost in Beauty",
            ImageUrl = "https://example.com/images/lost-in-beauty.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2012, 1, 27),
            DurationSeconds = 218,
            Album = AlbumMock.GetMockedAlbum3(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist2() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("3aff8c17-3c98-44ed-a849-1976d2c4a91a"), SimilarSongId = Guid.Parse("e7b0024e-cc97-46a8-bd3a-450607eebe3c") },
                new SongLink { SongId = Guid.Parse("3aff8c17-3c98-44ed-a849-1976d2c4a91a"), SimilarSongId = Guid.Parse("8a5f664a-c72d-46b2-b12b-b38e4a5ec67f") }
            },
            IsActive = true
        };

        public static Song GetMockedSong6() => new Song
        {
            Id = Guid.Parse("e7b0024e-cc97-46a8-bd3a-450607eebe3c"),
            Title = "Summer Time Sadness",
            ImageUrl = "https://example.com/images/summer-time-sadness.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2012, 2, 1),
            DurationSeconds = 268,
            Album = AlbumMock.GetMockedAlbum3(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist2() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("e7b0024e-cc97-46a8-bd3a-450607eebe3c"), SimilarSongId = Guid.Parse("8a5f664a-c72d-46b2-b12b-b38e4a5ec67f") },
                new SongLink { SongId = Guid.Parse("e7b0024e-cc97-46a8-bd3a-450607eebe3c"), SimilarSongId = Guid.Parse("83b64a87-6bc5-4b61-9121-505f37b81682") }
            },
            IsActive = true
        };

        public static Song GetMockedSong7() => new Song
        {
            Id = Guid.Parse("8a5f664a-c72d-46b2-b12b-b38e4a5ec67f"),
            Title = "Blue Jeans",
            ImageUrl = "https://example.com/images/blue-jeans.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2012, 2, 3),
            DurationSeconds = 245,
            Album = AlbumMock.GetMockedAlbum3(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist2() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("8a5f664a-c72d-46b2-b12b-b38e4a5ec67f"), SimilarSongId = Guid.Parse("83b64a87-6bc5-4b61-9121-505f37b81682") },
                new SongLink { SongId = Guid.Parse("8a5f664a-c72d-46b2-b12b-b38e4a5ec67f"), SimilarSongId = Guid.Parse("23abfe5e-e938-4bf5-93d4-202e2fa3aa3e") }
            },
            IsActive = true
        };

        public static Song GetMockedSong8() => new Song
        {
            Id = Guid.Parse("83b64a87-6bc5-4b61-9121-505f37b81682"),
            Title = "Radio",
            ImageUrl = "https://example.com/images/radio.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2012, 2, 5),
            DurationSeconds = 235,
            Album = AlbumMock.GetMockedAlbum3(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist2() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("83b64a87-6bc5-4b61-9121-505f37b81682"), SimilarSongId = Guid.Parse("23abfe5e-e938-4bf5-93d4-202e2fa3aa3e") },
                new SongLink { SongId = Guid.Parse("83b64a87-6bc5-4b61-9121-505f37b81682"), SimilarSongId = Guid.Parse("48f88f8f-c393-4bda-9812-a748486f404e") }
            },
            IsActive = true
        };

        public static Song GetMockedSong9() => new Song
        {
            Id = Guid.Parse("23abfe5e-e938-4bf5-93d4-202e2fa3aa3e"),
            Title = "Carmen",
            ImageUrl = "https://example.com/images/carmen.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2012, 2, 7),
            DurationSeconds = 249,
            Album = AlbumMock.GetMockedAlbum3(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist2() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("23abfe5e-e938-4bf5-93d4-202e2fa3aa3e"), SimilarSongId = Guid.Parse("48f88f8f-c393-4bda-9812-a748486f404e") },
                new SongLink { SongId = Guid.Parse("23abfe5e-e938-4bf5-93d4-202e2fa3aa3e"), SimilarSongId = Guid.Parse("eb6c8e4c-502e-45b4-9a69-387e33cdadb1") }
            },
            IsActive = true
        };

        public static Song GetMockedSong10() => new Song
        {
            Id = Guid.Parse("48f88f8f-c393-4bda-9812-a748486f404e"),
            Title = "Night Visions",
            ImageUrl = "https://example.com/images/night-visions.jpg",
            Genre = GenreType.Electronic,
            ReleaseDate = new DateOnly(2013, 5, 17),
            DurationSeconds = 337,
            Album = AlbumMock.GetMockedAlbum4(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist3() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("48f88f8f-c393-4bda-9812-a748486f404e"), SimilarSongId = Guid.Parse("eb6c8e4c-502e-45b4-9a69-387e33cdadb1") },
                new SongLink { SongId = Guid.Parse("48f88f8f-c393-4bda-9812-a748486f404e"), SimilarSongId = Guid.Parse("68b0682b-9ac7-42a2-873a-8e9874e12953") }
            },
            IsActive = true
        };

        public static Song GetMockedSong11() => new Song
        {
            Id = Guid.Parse("eb6c8e4c-502e-45b4-9a69-387e33cdadb1"),
            Title = "Synth Dreams",
            ImageUrl = "https://example.com/images/synth-dreams.jpg",
            Genre = GenreType.Electronic,
            ReleaseDate = new DateOnly(2013, 5, 17),
            DurationSeconds = 289,
            Album = AlbumMock.GetMockedAlbum4(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist3() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("eb6c8e4c-502e-45b4-9a69-387e33cdadb1"), SimilarSongId = Guid.Parse("68b0682b-9ac7-42a2-873a-8e9874e12953") },
                new SongLink { SongId = Guid.Parse("eb6c8e4c-502e-45b4-9a69-387e33cdadb1"), SimilarSongId = Guid.Parse("1e43835a-4902-4d12-abaf-0bc8f2dae2aa") }
            },
            IsActive = true
        };

        public static Song GetMockedSong12() => new Song
        {
            Id = Guid.Parse("68b0682b-9ac7-42a2-873a-8e9874e12953"),
            Title = "Echoes of Tomorrow",
            ImageUrl = "https://example.com/images/echoes-of-tomorrow.jpg",
            Genre = GenreType.Electronic,
            ReleaseDate = new DateOnly(2013, 5, 17),
            DurationSeconds = 305,
            Album = AlbumMock.GetMockedAlbum4(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist3() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("68b0682b-9ac7-42a2-873a-8e9874e12953"), SimilarSongId = Guid.Parse("1e43835a-4902-4d12-abaf-0bc8f2dae2aa") },
                new SongLink { SongId = Guid.Parse("68b0682b-9ac7-42a2-873a-8e9874e12953"), SimilarSongId = Guid.Parse("19dc1564-8c00-4377-95db-16535a80610a") }
            },
            IsActive = true
        };

        public static Song GetMockedSong13() => new Song
        {
            Id = Guid.Parse("1e43835a-4902-4d12-abaf-0bc8f2dae2aa"),
            Title = "Touch of the Light",
            ImageUrl = "https://example.com/images/touch-of-the-light.jpg",
            Genre = GenreType.Electronic,
            ReleaseDate = new DateOnly(2013, 5, 21),
            DurationSeconds = 337,
            Album = AlbumMock.GetMockedAlbum4(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist3() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("1e43835a-4902-4d12-abaf-0bc8f2dae2aa"), SimilarSongId = Guid.Parse("19dc1564-8c00-4377-95db-16535a80610a") },
                new SongLink { SongId = Guid.Parse("1e43835a-4902-4d12-abaf-0bc8f2dae2aa"), SimilarSongId = Guid.Parse("b26a4472-db66-4bec-926d-bb53f31083c2") }
            },
            IsActive = true
        };

        public static Song GetMockedSong14() => new Song
        {
            Id = Guid.Parse("19dc1564-8c00-4377-95db-16535a80610a"),
            Title = "Digital Love",
            ImageUrl = "https://example.com/images/digital-love.jpg",
            Genre = GenreType.Electronic,
            ReleaseDate = new DateOnly(2013, 5, 23),
            DurationSeconds = 325,
            Album = AlbumMock.GetMockedAlbum4(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist3() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("19dc1564-8c00-4377-95db-16535a80610a"), SimilarSongId = Guid.Parse("b26a4472-db66-4bec-926d-bb53f31083c2") },
                new SongLink { SongId = Guid.Parse("19dc1564-8c00-4377-95db-16535a80610a"), SimilarSongId = Guid.Parse("e91a010d-0fa4-4801-bee5-1974e87ab3d2") }
            },
            IsActive = true
        };

        public static Song GetMockedSong15() => new Song
        {
            Id = Guid.Parse("b26a4472-db66-4bec-926d-bb53f31083c2"),
            Title = "Instant Crush",
            ImageUrl = "https://example.com/images/instant-crush.jpg",
            Genre = GenreType.Electronic,
            ReleaseDate = new DateOnly(2013, 5, 25),
            DurationSeconds = 337,
            Album = AlbumMock.GetMockedAlbum4(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist3() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("b26a4472-db66-4bec-926d-bb53f31083c2"), SimilarSongId = Guid.Parse("e91a010d-0fa4-4801-bee5-1974e87ab3d2") },
                new SongLink { SongId = Guid.Parse("b26a4472-db66-4bec-926d-bb53f31083c2"), SimilarSongId = Guid.Parse("469b6456-1157-43da-a275-88c983fcee9d") }
            },
            IsActive = true
        };

        public static Song GetMockedSong16() => new Song
        {
            Id = Guid.Parse("e91a010d-0fa4-4801-bee5-1974e87ab3d2"),
            Title = "Lose Yourself to Dance",
            ImageUrl = "https://example.com/images/lose-yourself-to-dance.jpg",
            Genre = GenreType.Electronic,
            ReleaseDate = new DateOnly(2013, 5, 27),
            DurationSeconds = 354,
            Album = AlbumMock.GetMockedAlbum4(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist3() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("e91a010d-0fa4-4801-bee5-1974e87ab3d2"), SimilarSongId = Guid.Parse("469b6456-1157-43da-a275-88c983fcee9d") },
                new SongLink { SongId = Guid.Parse("e91a010d-0fa4-4801-bee5-1974e87ab3d2"), SimilarSongId = Guid.Parse("c23a762d-0f9f-43b9-8a6a-7a34421ee042") }
            },
            IsActive = true
        };

        public static Song GetMockedSong17() => new Song
        {
            Id = Guid.Parse("469b6456-1157-43da-a275-88c983fcee9d"),
            Title = "Streets on Fire",
            ImageUrl = "https://example.com/images/streets-on-fire.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2012, 10, 22),
            DurationSeconds = 222,
            Album = AlbumMock.GetMockedAlbum5(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist4() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("469b6456-1157-43da-a275-88c983fcee9d"), SimilarSongId = Guid.Parse("c23a762d-0f9f-43b9-8a6a-7a34421ee042") },
                new SongLink { SongId = Guid.Parse("469b6456-1157-43da-a275-88c983fcee9d"), SimilarSongId = Guid.Parse("80b291d4-5306-4879-8aa1-fec2ea4b6516") }
            },
            IsActive = true
        };

        public static Song GetMockedSong18() => new Song
        {
            Id = Guid.Parse("c23a762d-0f9f-43b9-8a6a-7a34421ee042"),
            Title = "The Prelude",
            ImageUrl = "https://example.com/images/the-prelude.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2012, 10, 22),
            DurationSeconds = 210,
            Album = AlbumMock.GetMockedAlbum5(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist4() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("c23a762d-0f9f-43b9-8a6a-7a34421ee042"), SimilarSongId = Guid.Parse("80b291d4-5306-4879-8aa1-fec2ea4b6516") },
                new SongLink { SongId = Guid.Parse("c23a762d-0f9f-43b9-8a6a-7a34421ee042"), SimilarSongId = Guid.Parse("7ef7351b-912e-4a64-ba6d-cfdfcb7d56af") }
            },
            IsActive = true
        };

        public static Song GetMockedSong19() => new Song
        {
            Id = Guid.Parse("80b291d4-5306-4879-8aa1-fec2ea4b6516"),
            Title = "Dawn Breaks",
            ImageUrl = "https://example.com/images/dawn-breaks.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2012, 10, 22),
            DurationSeconds = 237,
            Album = AlbumMock.GetMockedAlbum5(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist4() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("80b291d4-5306-4879-8aa1-fec2ea4b6516"), SimilarSongId = Guid.Parse("81acd1bf-e6f3-44c4-ad24-c47dc22adc60") },
                new SongLink { SongId = Guid.Parse("80b291d4-5306-4879-8aa1-fec2ea4b6516"), SimilarSongId = Guid.Parse("0e13b758-e1dc-4f5a-a481-de9ab43934f1") }
            },
            IsActive = true
        };

        public static Song GetMockedSong20() => new Song
        {
            Id = Guid.Parse("81acd1bf-e6f3-44c4-ad24-c47dc22adc60"),
            Title = "City Streets",
            ImageUrl = "https://example.com/images/city-streets.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2012, 10, 25),
            DurationSeconds = 215,
            Album = AlbumMock.GetMockedAlbum5(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist4() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("81acd1bf-e6f3-44c4-ad24-c47dc22adc60"), SimilarSongId = Guid.Parse("0e13b758-e1dc-4f5a-a481-de9ab43934f1") },
                new SongLink { SongId = Guid.Parse("81acd1bf-e6f3-44c4-ad24-c47dc22adc60"), SimilarSongId = Guid.Parse("9acfdf82-5ffd-474d-8303-b22a2a9ce0f8") }
            },
            IsActive = true
        };

        public static Song GetMockedSong21() => new Song
        {
            Id = Guid.Parse("0e13b758-e1dc-4f5a-a481-de9ab43934f1"),
            Title = "Back Home",
            ImageUrl = "https://example.com/images/back-home.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2012, 10, 27),
            DurationSeconds = 231,
            Album = AlbumMock.GetMockedAlbum5(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist4() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("0e13b758-e1dc-4f5a-a481-de9ab43934f1"), SimilarSongId = Guid.Parse("9acfdf82-5ffd-474d-8303-b22a2a9ce0f8") },
                new SongLink { SongId = Guid.Parse("0e13b758-e1dc-4f5a-a481-de9ab43934f1"), SimilarSongId = Guid.Parse("586efb75-57ab-43ca-9b85-3bdeeae3ae19") }
            },
            IsActive = true
        };

        public static Song GetMockedSong22() => new Song
        {
            Id = Guid.Parse("9acfdf82-5ffd-474d-8303-b22a2a9ce0f8"),
            Title = "Compton",
            ImageUrl = "https://example.com/images/compton.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2012, 10, 29),
            DurationSeconds = 210,
            Album = AlbumMock.GetMockedAlbum5(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist4() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("9acfdf82-5ffd-474d-8303-b22a2a9ce0f8"), SimilarSongId = Guid.Parse("586efb75-57ab-43ca-9b85-3bdeeae3ae19") },
                new SongLink { SongId = Guid.Parse("9acfdf82-5ffd-474d-8303-b22a2a9ce0f8"), SimilarSongId = Guid.Parse("e4be062f-b594-4580-b514-d9d6cdaf2933") }
            },
            IsActive = true
        };

        public static Song GetMockedSong23() => new Song
        {
            Id = Guid.Parse("586efb75-57ab-43ca-9b85-3bdeeae3ae19"),
            Title = "Swimming Pools (Drank)",
            ImageUrl = "https://example.com/images/swimming-pools.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2012, 10, 31),
            DurationSeconds = 241,
            Album = AlbumMock.GetMockedAlbum5(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist4() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("586efb75-57ab-43ca-9b85-3bdeeae3ae19"), SimilarSongId = Guid.Parse("e4be062f-b594-4580-b514-d9d6cdaf2933") },
                new SongLink { SongId = Guid.Parse("586efb75-57ab-43ca-9b85-3bdeeae3ae19"), SimilarSongId = Guid.Parse("0500418e-bfad-4cd1-860d-994cbdc9e2df") }
            },
            IsActive = true
        };

        public static Song GetMockedSong24() => new Song
        {
            Id = Guid.Parse("e4be062f-b594-4580-b514-d9d6cdaf2933"),
            Title = "Endless Sun",
            ImageUrl = "https://example.com/images/endless-sun.jpg",
            Genre = GenreType.Rnb,
            ReleaseDate = new DateOnly(2012, 7, 10),
            DurationSeconds = 248,
            Album = AlbumMock.GetMockedAlbum6(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist5() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("e4be062f-b594-4580-b514-d9d6cdaf2933"), SimilarSongId = Guid.Parse("0500418e-bfad-4cd1-860d-994cbdc9e2df") },
                new SongLink { SongId = Guid.Parse("e4be062f-b594-4580-b514-d9d6cdaf2933"), SimilarSongId = Guid.Parse("bc81df0c-573b-4269-ac2c-2b2667967dbb") }
            },
            IsActive = true
        };

        public static Song GetMockedSong25() => new Song
        {
            Id = Guid.Parse("0500418e-bfad-4cd1-860d-994cbdc9e2df"),
            Title = "Silent Waves",
            ImageUrl = "https://example.com/images/silent-waves.jpg",
            Genre = GenreType.Rnb,
            ReleaseDate = new DateOnly(2012, 7, 10),
            DurationSeconds = 264,
            Album = AlbumMock.GetMockedAlbum6(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist5() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("0500418e-bfad-4cd1-860d-994cbdc9e2df"), SimilarSongId = Guid.Parse("bc81df0c-573b-4269-ac2c-2b2667967dbb") },
                new SongLink { SongId = Guid.Parse("0500418e-bfad-4cd1-860d-994cbdc9e2df"), SimilarSongId = Guid.Parse("866d5272-594d-423d-b702-cfccbe7a8e44") }
            },
            IsActive = true
        };

        public static Song GetMockedSong26() => new Song
        {
            Id = Guid.Parse("bc81df0c-573b-4269-ac2c-2b2667967dbb"),
            Title = "Ocean Avenue",
            ImageUrl = "https://example.com/images/ocean-avenue.jpg",
            Genre = GenreType.Rnb,
            ReleaseDate = new DateOnly(2012, 7, 10),
            DurationSeconds = 312,
            Album = AlbumMock.GetMockedAlbum6(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist5() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("bc81df0c-573b-4269-ac2c-2b2667967dbb"), SimilarSongId = Guid.Parse("866d5272-594d-423d-b702-cfccbe7a8e44") },
                new SongLink { SongId = Guid.Parse("bc81df0c-573b-4269-ac2c-2b2667967dbb"), SimilarSongId = Guid.Parse("5231566f-373d-46a2-af0f-62e1a9ea643b") }
            },
            IsActive = true
        };

        public static Song GetMockedSong27() => new Song
        {
            Id = Guid.Parse("866d5272-594d-423d-b702-cfccbe7a8e44"),
            Title = "Pyramid Nights",
            ImageUrl = "https://example.com/images/pyramid-nights.jpg",
            Genre = GenreType.Rnb,
            ReleaseDate = new DateOnly(2012, 7, 12),
            DurationSeconds = 312,
            Album = AlbumMock.GetMockedAlbum6(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist5() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("866d5272-594d-423d-b702-cfccbe7a8e44"), SimilarSongId = Guid.Parse("5231566f-373d-46a2-af0f-62e1a9ea643b") },
                new SongLink { SongId = Guid.Parse("866d5272-594d-423d-b702-cfccbe7a8e44"), SimilarSongId = Guid.Parse("70eb9a4b-be40-4b01-8534-796bb5f02d90") }
            },
            IsActive = true
        };

        public static Song GetMockedSong28() => new Song
        {
            Id = Guid.Parse("5231566f-373d-46a2-af0f-62e1a9ea643b"),
            Title = "Sweet Life",
            ImageUrl = "https://example.com/images/sweet-life.jpg",
            Genre = GenreType.Rnb,
            ReleaseDate = new DateOnly(2012, 7, 14),
            DurationSeconds = 288,
            Album = AlbumMock.GetMockedAlbum6(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist5() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("5231566f-373d-46a2-af0f-62e1a9ea643b"), SimilarSongId = Guid.Parse("70eb9a4b-be40-4b01-8534-796bb5f02d90") },
                new SongLink { SongId = Guid.Parse("5231566f-373d-46a2-af0f-62e1a9ea643b"), SimilarSongId = Guid.Parse("bb5f149f-8e45-455c-91d1-97639e96f671") }
            },
            IsActive = true
        };

        public static Song GetMockedSong29() => new Song
        {
            Id = Guid.Parse("70eb9a4b-be40-4b01-8534-796bb5f02d90"),
            Title = "Lost",
            ImageUrl = "https://example.com/images/lost.jpg",
            Genre = GenreType.Rnb,
            ReleaseDate = new DateOnly(2012, 7, 16),
            DurationSeconds = 234,
            Album = AlbumMock.GetMockedAlbum6(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist5() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("70eb9a4b-be40-4b01-8534-796bb5f02d90"), SimilarSongId = Guid.Parse("bb5f149f-8e45-455c-91d1-97639e96f671") },
                new SongLink { SongId = Guid.Parse("70eb9a4b-be40-4b01-8534-796bb5f02d90"), SimilarSongId = Guid.Parse("4a88b128-2471-4f25-b51a-136363ddbe7d") }
            },
            IsActive = true
        };

        public static Song GetMockedSong30() => new Song
        {
            Id = Guid.Parse("bb5f149f-8e45-455c-91d1-97639e96f671"),
            Title = "Super Rich Kids",
            ImageUrl = "https://example.com/images/super-rich-kids.jpg",
            Genre = GenreType.Rnb,
            ReleaseDate = new DateOnly(2012, 7, 18),
            DurationSeconds = 303,
            Album = AlbumMock.GetMockedAlbum6(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist5() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("bb5f149f-8e45-455c-91d1-97639e96f671"), SimilarSongId = Guid.Parse("4a88b128-2471-4f25-b51a-136363ddbe7d") },
                new SongLink { SongId = Guid.Parse("bb5f149f-8e45-455c-91d1-97639e96f671"), SimilarSongId = Guid.Parse("84b69406-d92e-4feb-a313-f1f373f1958c") }
            },
            IsActive = true
        };

        public static Song GetMockedSong31() => new Song
        {
            Id = Guid.Parse("4a88b128-2471-4f25-b51a-136363ddbe7d"),
            Title = "Light House",
            ImageUrl = "https://example.com/images/light-house.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2014, 6, 5),
            DurationSeconds = 242,
            Album = AlbumMock.GetMockedAlbum7(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist6() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("4a88b128-2471-4f25-b51a-136363ddbe7d"), SimilarSongId = Guid.Parse("84b69406-d92e-4feb-a313-f1f373f1958c") },
                new SongLink { SongId = Guid.Parse("4a88b128-2471-4f25-b51a-136363ddbe7d"), SimilarSongId = Guid.Parse("ded148fc-19d8-478a-9060-0b4543727d37") }
            },
            IsActive = true
        };

        public static Song GetMockedSong32() => new Song
        {
            Id = Guid.Parse("84b69406-d92e-4feb-a313-f1f373f1958c"),
            Title = "Moonlight Shadow",
            ImageUrl = "https://example.com/images/moonlight-shadow.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2014, 6, 7),
            DurationSeconds = 258,
            Album = AlbumMock.GetMockedAlbum7(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist6() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("84b69406-d92e-4feb-a313-f1f373f1958c"), SimilarSongId = Guid.Parse("ded148fc-19d8-478a-9060-0b4543727d37") },
                new SongLink { SongId = Guid.Parse("84b69406-d92e-4feb-a313-f1f373f1958c"), SimilarSongId = Guid.Parse("92388b4d-6ff6-4500-9b02-24d4227f3a28") }
            },
            IsActive = true
        };

        public static Song GetMockedSong33() => new Song
        {
            Id = Guid.Parse("ded148fc-19d8-478a-9060-0b4543727d37"),
            Title = "Stay With Me",
            ImageUrl = "https://example.com/images/stay-with-me.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2014, 6, 9),
            DurationSeconds = 182,
            Album = AlbumMock.GetMockedAlbum7(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist6() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("ded148fc-19d8-478a-9060-0b4543727d37"), SimilarSongId = Guid.Parse("92388b4d-6ff6-4500-9b02-24d4227f3a28") },
                new SongLink { SongId = Guid.Parse("ded148fc-19d8-478a-9060-0b4543727d37"), SimilarSongId = Guid.Parse("e1817fb5-5fb6-44aa-abbf-eda52cc578d7") }
            },
            IsActive = true
        };

        public static Song GetMockedSong34() => new Song
        {
            Id = Guid.Parse("92388b4d-6ff6-4500-9b02-24d4227f3a28"),
            Title = "Leave Your Lover",
            ImageUrl = "https://example.com/images/leave-your-lover.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2014, 6, 11),
            DurationSeconds = 206,
            Album = AlbumMock.GetMockedAlbum7(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist6() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("92388b4d-6ff6-4500-9b02-24d4227f3a28"), SimilarSongId = Guid.Parse("e1817fb5-5fb6-44aa-abbf-eda52cc578d7") },
                new SongLink { SongId = Guid.Parse("92388b4d-6ff6-4500-9b02-24d4227f3a28"), SimilarSongId = Guid.Parse("9a5f706f-ae38-418d-b911-77559d20e076") }
            },
            IsActive = true
        };

        public static Song GetMockedSong35() => new Song
        {
            Id = Guid.Parse("e1817fb5-5fb6-44aa-abbf-eda52cc578d7"),
            Title = "Eclipse Phase",
            ImageUrl = "https://example.com/images/eclipse-phase.jpg",
            Genre = GenreType.Rock,
            ReleaseDate = new DateOnly(1973, 4, 1),
            DurationSeconds = 312,
            Album = AlbumMock.GetMockedAlbum8(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist7() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("e1817fb5-5fb6-44aa-abbf-eda52cc578d7"), SimilarSongId = Guid.Parse("9a5f706f-ae38-418d-b911-77559d20e076") },
                new SongLink { SongId = Guid.Parse("e1817fb5-5fb6-44aa-abbf-eda52cc578d7"), SimilarSongId = Guid.Parse("01db55cc-7d06-4778-b9c1-7ccdc3e3cd13") }
            },
            IsActive = true
        };

        public static Song GetMockedSong36() => new Song
        {
            Id = Guid.Parse("9a5f706f-ae38-418d-b911-77559d20e076"),
            Title = "Moons Dark Side",
            ImageUrl = "https://example.com/images/moons-dark-side.jpg",
            Genre = GenreType.Rock,
            ReleaseDate = new DateOnly(1973, 4, 3),
            DurationSeconds = 295,
            Album = AlbumMock.GetMockedAlbum8(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist7() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("9a5f706f-ae38-418d-b911-77559d20e076"), SimilarSongId = Guid.Parse("01db55cc-7d06-4778-b9c1-7ccdc3e3cd13") },
                new SongLink { SongId = Guid.Parse("9a5f706f-ae38-418d-b911-77559d20e076"), SimilarSongId = Guid.Parse("f80a900f-e0a5-4cc3-8adf-27c6309b81ca") }
            },
            IsActive = true
        };

        public static Song GetMockedSong37() => new Song
        {
            Id = Guid.Parse("01db55cc-7d06-4778-b9c1-7ccdc3e3cd13"),
            Title = "Time",
            ImageUrl = "https://example.com/images/time.jpg",
            Genre = GenreType.Rock,
            ReleaseDate = new DateOnly(1973, 4, 5),
            DurationSeconds = 408,
            Album = AlbumMock.GetMockedAlbum8(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist7() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("01db55cc-7d06-4778-b9c1-7ccdc3e3cd13"), SimilarSongId = Guid.Parse("f80a900f-e0a5-4cc3-8adf-27c6309b81ca") },
                new SongLink { SongId = Guid.Parse("01db55cc-7d06-4778-b9c1-7ccdc3e3cd13"), SimilarSongId = Guid.Parse("d6a31188-c1b8-4976-b372-aced401f2347") }
            },
            IsActive = true
        };

        public static Song GetMockedSong38() => new Song
        {
            Id = Guid.Parse("f80a900f-e0a5-4cc3-8adf-27c6309b81ca"),
            Title = "The Great Gig in the Sky",
            ImageUrl = "https://example.com/images/great-gig-in-the-sky.jpg",
            Genre = GenreType.Rock,
            ReleaseDate = new DateOnly(1973, 4, 7),
            DurationSeconds = 276,
            Album = AlbumMock.GetMockedAlbum8(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist7() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("f80a900f-e0a5-4cc3-8adf-27c6309b81ca"), SimilarSongId = Guid.Parse("d6a31188-c1b8-4976-b372-aced401f2347") },
                new SongLink { SongId = Guid.Parse("f80a900f-e0a5-4cc3-8adf-27c6309b81ca"), SimilarSongId = Guid.Parse("8f0191bc-a242-4cb0-a9ba-38f48e823e54") }
            },
            IsActive = true
        };

        public static Song GetMockedSong39() => new Song
        {
            Id = Guid.Parse("d6a31188-c1b8-4976-b372-aced401f2347"),
            Title = "Stardust Memories",
            ImageUrl = "https://example.com/images/stardust-memories.jpg",
            Genre = GenreType.Rock,
            ReleaseDate = new DateOnly(2016, 1, 10),
            DurationSeconds = 289,
            Album = AlbumMock.GetMockedAlbum10(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist9() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("d6a31188-c1b8-4976-b372-aced401f2347"), SimilarSongId = Guid.Parse("8f0191bc-a242-4cb0-a9ba-38f48e823e54") },
                new SongLink { SongId = Guid.Parse("d6a31188-c1b8-4976-b372-aced401f2347"), SimilarSongId = Guid.Parse("c80e5abb-3759-49eb-8cbe-c2b7ff742072") }
            },
            IsActive = true
        };

        public static Song GetMockedSong40() => new Song
        {
            Id = Guid.Parse("8f0191bc-a242-4cb0-a9ba-38f48e823e54"),
            Title = "Black Skies",
            ImageUrl = "https://example.com/images/black-skies.jpg",
            Genre = GenreType.Rock,
            ReleaseDate = new DateOnly(2016, 1, 12),
            DurationSeconds = 305,
            Album = AlbumMock.GetMockedAlbum10(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist9() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("8f0191bc-a242-4cb0-a9ba-38f48e823e54"), SimilarSongId = Guid.Parse("c80e5abb-3759-49eb-8cbe-c2b7ff742072") },
                new SongLink { SongId = Guid.Parse("8f0191bc-a242-4cb0-a9ba-38f48e823e54"), SimilarSongId = Guid.Parse("f1647832-8eb1-460d-a5ae-c9fac5e2cd5d") }
            },
            IsActive = true
        };

        public static Song GetMockedSong41() => new Song
        {
            Id = Guid.Parse("c80e5abb-3759-49eb-8cbe-c2b7ff742072"),
            Title = "Lazarus",
            ImageUrl = "https://example.com/images/lazarus.jpg",
            Genre = GenreType.Rock,
            ReleaseDate = new DateOnly(2016, 1, 14),
            DurationSeconds = 264,
            Album = AlbumMock.GetMockedAlbum10(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist9() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("c80e5abb-3759-49eb-8cbe-c2b7ff742072"), SimilarSongId = Guid.Parse("f1647832-8eb1-460d-a5ae-c9fac5e2cd5d") },
                new SongLink { SongId = Guid.Parse("c80e5abb-3759-49eb-8cbe-c2b7ff742072"), SimilarSongId = Guid.Parse("e3d3e750-7179-4189-9c19-fd546c4493c5") }
            },
            IsActive = true
        };

        public static Song GetMockedSong42() => new Song
        {
            Id = Guid.Parse("f1647832-8eb1-460d-a5ae-c9fac5e2cd5d"),
            Title = "I Can't Give Everything Away",
            ImageUrl = "https://example.com/images/i-cant-give-everything-away.jpg",
            Genre = GenreType.Rock,
            ReleaseDate = new DateOnly(2016, 1, 16),
            DurationSeconds = 249,
            Album = AlbumMock.GetMockedAlbum10(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist9() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("f1647832-8eb1-460d-a5ae-c9fac5e2cd5d"), SimilarSongId = Guid.Parse("e3d3e750-7179-4189-9c19-fd546c4493c5") },
                new SongLink { SongId = Guid.Parse("f1647832-8eb1-460d-a5ae-c9fac5e2cd5d"), SimilarSongId = Guid.Parse("2a14abb5-4eea-46e2-bb70-2cf907acbf09") }
            },
            IsActive = true
        };

        public static Song GetMockedSong43() => new Song
        {
            Id = Guid.Parse("e3d3e750-7179-4189-9c19-fd546c4493c5"),
            Title = "New Romantics",
            ImageUrl = "https://example.com/images/new-romantics.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2017, 11, 16),
            DurationSeconds = 234,
            Album = AlbumMock.GetMockedAlbum50(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist10() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("e3d3e750-7179-4189-9c19-fd546c4493c5"), SimilarSongId = Guid.Parse("2a14abb5-4eea-46e2-bb70-2cf907acbf09") },
                new SongLink { SongId = Guid.Parse("e3d3e750-7179-4189-9c19-fd546c4493c5"), SimilarSongId = Guid.Parse("8f764924-0f7a-49e0-a39d-d29f9c3b1161") }
            },
            IsActive = true
        };

        public static Song GetMockedSong44() => new Song
        {
            Id = Guid.Parse("2a14abb5-4eea-46e2-bb70-2cf907acbf09"),
            Title = "End Game",
            ImageUrl = "https://example.com/images/end-game.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2017, 11, 18),
            DurationSeconds = 245,
            Album = AlbumMock.GetMockedAlbum50(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist10() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("2a14abb5-4eea-46e2-bb70-2cf907acbf09"), SimilarSongId = Guid.Parse("8f764924-0f7a-49e0-a39d-d29f9c3b1161") },
                new SongLink { SongId = Guid.Parse("2a14abb5-4eea-46e2-bb70-2cf907acbf09"), SimilarSongId = Guid.Parse("2dec0615-0284-4260-a0f8-44baa2954bc4") }
            },
            IsActive = true
        };

        public static Song GetMockedSong45() => new Song
        {
            Id = Guid.Parse("8f764924-0f7a-49e0-a39d-d29f9c3b1161"),
            Title = "Delicate",
            ImageUrl = "https://example.com/images/delicate.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2017, 11, 20),
            DurationSeconds = 228,
            Album = AlbumMock.GetMockedAlbum50(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist10() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("8f764924-0f7a-49e0-a39d-d29f9c3b1161"), SimilarSongId = Guid.Parse("2dec0615-0284-4260-a0f8-44baa2954bc4") },
                new SongLink { SongId = Guid.Parse("8f764924-0f7a-49e0-a39d-d29f9c3b1161"), SimilarSongId = Guid.Parse("55bce552-42e3-4ae9-96d2-6df192f2ac50") }
            },
            IsActive = true
        };

        public static Song GetMockedSong46() => new Song
        {
            Id = Guid.Parse("2dec0615-0284-4260-a0f8-44baa2954bc4"),
            Title = "Echoes of Glory",
            ImageUrl = "https://example.com/images/echoes-of-glory.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2017, 11, 12),
            DurationSeconds = 228,
            Album = AlbumMock.GetMockedAlbum50(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist10() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("2dec0615-0284-4260-a0f8-44baa2954bc4"), SimilarSongId = Guid.Parse("55bce552-42e3-4ae9-96d2-6df192f2ac50") },
                new SongLink { SongId = Guid.Parse("2dec0615-0284-4260-a0f8-44baa2954bc4"), SimilarSongId = Guid.Parse("7aaaf887-d5e5-47b6-b14b-6dfb7e423ea8") }
            },
            IsActive = true
        };

        public static Song GetMockedSong47() => new Song
        {
            Id = Guid.Parse("55bce552-42e3-4ae9-96d2-6df192f2ac50"),
            Title = "Silent Screams",
            ImageUrl = "https://example.com/images/silent-screams.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2017, 11, 14),
            DurationSeconds = 214,
            Album = AlbumMock.GetMockedAlbum50(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist10() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("55bce552-42e3-4ae9-96d2-6df192f2ac50"), SimilarSongId = Guid.Parse("7aaaf887-d5e5-47b6-b14b-6dfb7e423ea8") },
                new SongLink { SongId = Guid.Parse("55bce552-42e3-4ae9-96d2-6df192f2ac50"), SimilarSongId = Guid.Parse("f7a5b648-8efe-41a8-8407-a9a76d8eb6fc") }
            },
            IsActive = true
        };

        public static Song GetMockedSong48() => new Song
        {
            Id = Guid.Parse("7aaaf887-d5e5-47b6-b14b-6dfb7e423ea8"),
            Title = "Watermelon Sugar",
            ImageUrl = "https://example.com/images/watermelon-sugar.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2019, 12, 15),
            DurationSeconds = 174,
            Album = AlbumMock.GetMockedAlbum11(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist11() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("7aaaf887-d5e5-47b6-b14b-6dfb7e423ea8"), SimilarSongId = Guid.Parse("f7a5b648-8efe-41a8-8407-a9a76d8eb6fc") },
                new SongLink { SongId = Guid.Parse("7aaaf887-d5e5-47b6-b14b-6dfb7e423ea8"), SimilarSongId = Guid.Parse("25d27dd1-8add-421e-93e0-ba7d964ff990") }
            },
            IsActive = true
        };

        public static Song GetMockedSong49() => new Song
        {
            Id = Guid.Parse("f7a5b648-8efe-41a8-8407-a9a76d8eb6fc"),
            Title = "Golden Days",
            ImageUrl = "https://example.com/images/golden-days.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2019, 12, 17),
            DurationSeconds = 210,
            Album = AlbumMock.GetMockedAlbum11(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist11() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("f7a5b648-8efe-41a8-8407-a9a76d8eb6fc"), SimilarSongId = Guid.Parse("25d27dd1-8add-421e-93e0-ba7d964ff990") },
                new SongLink { SongId = Guid.Parse("f7a5b648-8efe-41a8-8407-a9a76d8eb6fc"), SimilarSongId = Guid.Parse("93218e81-668d-4697-ab5b-e9e04cc9732d") }
            },
            IsActive = true
        };

        public static Song GetMockedSong50() => new Song
        {
            Id = Guid.Parse("25d27dd1-8add-421e-93e0-ba7d964ff990"),
            Title = "Hold Up",
            ImageUrl = "https://example.com/images/holdup.jpg",
            Genre = GenreType.Rnb,
            ReleaseDate = new DateOnly(2019, 12, 18),
            DurationSeconds = 251,
            Album = AlbumMock.GetMockedAlbum11(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist11() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("25d27dd1-8add-421e-93e0-ba7d964ff990"), SimilarSongId = Guid.Parse("93218e81-668d-4697-ab5b-e9e04cc9732d") },
                new SongLink { SongId = Guid.Parse("25d27dd1-8add-421e-93e0-ba7d964ff990"), SimilarSongId = Guid.Parse("3e5f72f3-c2f9-4771-99c8-5a5ced274ed7") }
            },
            IsActive = true
        };

        public static Song GetMockedSong51() => new Song
        {
            Id = Guid.Parse("93218e81-668d-4697-ab5b-e9e04cc9732d"),
            Title = "Freedom",
            ImageUrl = "https://example.com/images/freedom.jpg",
            Genre = GenreType.Rnb,
            ReleaseDate = new DateOnly(2019, 12, 18),
            DurationSeconds = 274,
            Album = AlbumMock.GetMockedAlbum11(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist11() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("93218e81-668d-4697-ab5b-e9e04cc9732d"), SimilarSongId = Guid.Parse("3e5f72f3-c2f9-4771-99c8-5a5ced274ed7") },
                new SongLink { SongId = Guid.Parse("93218e81-668d-4697-ab5b-e9e04cc9732d"), SimilarSongId = Guid.Parse("c00dd550-41aa-41e3-890e-abf3c937e62f") }
            },
            IsActive = true
        };

        public static Song GetMockedSong52() => new Song
        {
            Id = Guid.Parse("3e5f72f3-c2f9-4771-99c8-5a5ced274ed7"),
            Title = "Falling",
            ImageUrl = "https://example.com/images/falling.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2019, 12, 19),
            DurationSeconds = 240,
            Album = AlbumMock.GetMockedAlbum11(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist11() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("3e5f72f3-c2f9-4771-99c8-5a5ced274ed7"), SimilarSongId = Guid.Parse("c00dd550-41aa-41e3-890e-abf3c937e62f") },
                new SongLink { SongId = Guid.Parse("3e5f72f3-c2f9-4771-99c8-5a5ced274ed7"), SimilarSongId = Guid.Parse("bd5e6040-ba62-4a5d-aeda-ba81f6f46eea") }
            },
            IsActive = true
        };

        public static Song GetMockedSong53() => new Song
        {
            Id = Guid.Parse("c00dd550-41aa-41e3-890e-abf3c937e62f"),
            Title = "She",
            ImageUrl = "https://example.com/images/she.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2019, 12, 21),
            DurationSeconds = 295,
            Album = AlbumMock.GetMockedAlbum11(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist11() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("c00dd550-41aa-41e3-890e-abf3c937e62f"), SimilarSongId = Guid.Parse("bd5e6040-ba62-4a5d-aeda-ba81f6f46eea") },
                new SongLink { SongId = Guid.Parse("c00dd550-41aa-41e3-890e-abf3c937e62f"), SimilarSongId = Guid.Parse("448f0c35-54b1-4bd3-9a37-24b350b50d0f") }
            },
            IsActive = true
        };

        public static Song GetMockedSong54() => new Song
        {
            Id = Guid.Parse("bd5e6040-ba62-4a5d-aeda-ba81f6f46eea"),
            Title = "Rolling in the Deep",
            ImageUrl = "https://example.com/images/rolling-in-the-deep.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2011, 1, 29),
            DurationSeconds = 228,
            Album = AlbumMock.GetMockedAlbum17(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist14() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("bd5e6040-ba62-4a5d-aeda-ba81f6f46eea"), SimilarSongId = Guid.Parse("448f0c35-54b1-4bd3-9a37-24b350b50d0f") },
                new SongLink { SongId = Guid.Parse("bd5e6040-ba62-4a5d-aeda-ba81f6f46eea"), SimilarSongId = Guid.Parse("835cb2dd-fb45-4605-9445-a2d14d2fba7b") }
            },
            IsActive = true
        };

        public static Song GetMockedSong55() => new Song
        {
            Id = Guid.Parse("448f0c35-54b1-4bd3-9a37-24b350b50d0f"),
            Title = "Someone Like You",
            ImageUrl = "https://example.com/images/someone-like-you.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2011, 1, 31),
            DurationSeconds = 286,
            Album = AlbumMock.GetMockedAlbum17(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist14() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("448f0c35-54b1-4bd3-9a37-24b350b50d0f"), SimilarSongId = Guid.Parse("835cb2dd-fb45-4605-9445-a2d14d2fba7b") },
                new SongLink { SongId = Guid.Parse("448f0c35-54b1-4bd3-9a37-24b350b50d0f"), SimilarSongId = Guid.Parse("d38a98c4-5cc7-45a2-93c0-4d03ff9cb496") }
            },
            IsActive = true
        };

        public static Song GetMockedSong56() => new Song
        {
            Id = Guid.Parse("835cb2dd-fb45-4605-9445-a2d14d2fba7b"),
            Title = "Set Fire to the Rain",
            ImageUrl = "https://example.com/images/set-fire-to-the-rain.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2011, 1, 28),
            DurationSeconds = 242,
            Album = AlbumMock.GetMockedAlbum17(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist14() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("835cb2dd-fb45-4605-9445-a2d14d2fba7b"), SimilarSongId = Guid.Parse("d38a98c4-5cc7-45a2-93c0-4d03ff9cb496") },
                new SongLink { SongId = Guid.Parse("835cb2dd-fb45-4605-9445-a2d14d2fba7b"), SimilarSongId = Guid.Parse("34531668-8c68-4ebd-a6a7-36645e9bac97") }
            },
            IsActive = true
        };

        public static Song GetMockedSong57() => new Song
        {
            Id = Guid.Parse("d38a98c4-5cc7-45a2-93c0-4d03ff9cb496"),
            Title = "Rumour Has It",
            ImageUrl = "https://example.com/images/rumour-has-it.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2011, 1, 30),
            DurationSeconds = 223,
            Album = AlbumMock.GetMockedAlbum17(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist14() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("d38a98c4-5cc7-45a2-93c0-4d03ff9cb496"), SimilarSongId = Guid.Parse("34531668-8c68-4ebd-a6a7-36645e9bac97") },
                new SongLink { SongId = Guid.Parse("d38a98c4-5cc7-45a2-93c0-4d03ff9cb496"), SimilarSongId = Guid.Parse("f2c126f4-0b32-46b1-a293-e4d53fd70d0f") }
            },
            IsActive = true
        };

        public static Song GetMockedSong58() => new Song
        {
            Id = Guid.Parse("34531668-8c68-4ebd-a6a7-36645e9bac97"),
            Title = "Started From the Bottom",
            ImageUrl = "https://example.com/images/started-from-the-bottom.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2013, 9, 26),
            DurationSeconds = 197,
            Album = AlbumMock.GetMockedAlbum27(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist23() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("34531668-8c68-4ebd-a6a7-36645e9bac97"), SimilarSongId = Guid.Parse("f2c126f4-0b32-46b1-a293-e4d53fd70d0f") },
                new SongLink { SongId = Guid.Parse("34531668-8c68-4ebd-a6a7-36645e9bac97"), SimilarSongId = Guid.Parse("095f7293-c7b5-4c29-9112-4aa24a8c063a") }
            },
            IsActive = true
        };

        public static Song GetMockedSong59() => new Song
        {
            Id = Guid.Parse("f2c126f4-0b32-46b1-a293-e4d53fd70d0f"),
            Title = "Hold On, We’re Going Home",
            ImageUrl = "https://example.com/images/hold-on-were-going-home.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2013, 9, 28),
            DurationSeconds = 231,
            Album = AlbumMock.GetMockedAlbum27(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist23() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("f2c126f4-0b32-46b1-a293-e4d53fd70d0f"), SimilarSongId = Guid.Parse("095f7293-c7b5-4c29-9112-4aa24a8c063a") },
                new SongLink { SongId = Guid.Parse("f2c126f4-0b32-46b1-a293-e4d53fd70d0f"), SimilarSongId = Guid.Parse("1a4c8801-de6e-4787-b940-69ec4b9e8ad1") }
            },
            IsActive = true
        };

        public static Song GetMockedSong60() => new Song
        {
            Id = Guid.Parse("095f7293-c7b5-4c29-9112-4aa24a8c063a"),
            Title = "Too Much",
            ImageUrl = "https://example.com/images/too-much.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2013, 9, 30),
            DurationSeconds = 250,
            Album = AlbumMock.GetMockedAlbum27(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist23() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("095f7293-c7b5-4c29-9112-4aa24a8c063a"), SimilarSongId = Guid.Parse("1a4c8801-de6e-4787-b940-69ec4b9e8ad1") },
                new SongLink { SongId = Guid.Parse("095f7293-c7b5-4c29-9112-4aa24a8c063a"), SimilarSongId = Guid.Parse("a1f09d89-9f92-4531-ae83-7d595da08138") }
            },
            IsActive = true
        };

        public static Song GetMockedSong61() => new Song
        {
            Id = Guid.Parse("1a4c8801-de6e-4787-b940-69ec4b9e8ad1"),
            Title = "Wu-Tang Forever",
            ImageUrl = "https://example.com/images/wutang-forever.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2013, 10, 2),
            DurationSeconds = 210,
            Album = AlbumMock.GetMockedAlbum27(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist23() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("1a4c8801-de6e-4787-b940-69ec4b9e8ad1"), SimilarSongId = Guid.Parse("a1f09d89-9f92-4531-ae83-7d595da08138") },
                new SongLink { SongId = Guid.Parse("1a4c8801-de6e-4787-b940-69ec4b9e8ad1"), SimilarSongId = Guid.Parse("923182ab-0058-40ea-a878-f6ba3dad4f74") }
            },
            IsActive = true
        };

        public static Song GetMockedSong62() => new Song
        {
            Id = Guid.Parse("a1f09d89-9f92-4531-ae83-7d595da08138"),
            Title = "Work",
            ImageUrl = "https://example.com/images/work.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2016, 1, 30),
            DurationSeconds = 213,
            Album = AlbumMock.GetMockedAlbum28(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist24() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("a1f09d89-9f92-4531-ae83-7d595da08138"), SimilarSongId = Guid.Parse("923182ab-0058-40ea-a878-f6ba3dad4f74") },
                new SongLink { SongId = Guid.Parse("a1f09d89-9f92-4531-ae83-7d595da08138"), SimilarSongId = Guid.Parse("3c15b1af-8b00-4356-ae83-873a553d99c6") }
            },
            IsActive = true
        };

        public static Song GetMockedSong63() => new Song
        {
            Id = Guid.Parse("923182ab-0058-40ea-a878-f6ba3dad4f74"),
            Title = "Desperado",
            ImageUrl = "https://example.com/images/desperado.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2016, 2, 1),
            DurationSeconds = 192,
            Album = AlbumMock.GetMockedAlbum28(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist24() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("923182ab-0058-40ea-a878-f6ba3dad4f74"), SimilarSongId = Guid.Parse("3c15b1af-8b00-4356-ae83-873a553d99c6") },
                new SongLink { SongId = Guid.Parse("923182ab-0058-40ea-a878-f6ba3dad4f74"), SimilarSongId = Guid.Parse("3dab7f91-33f3-4271-a2ff-d9b5eb232068") }
            },
            IsActive = true
        };

        public static Song GetMockedSong64() => new Song
        {
            Id = Guid.Parse("3c15b1af-8b00-4356-ae83-873a553d99c6"),
            Title = "Kiss It Better",
            ImageUrl = "https://example.com/images/kiss-it-better.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2016, 1, 28),
            DurationSeconds = 242,
            Album = AlbumMock.GetMockedAlbum28(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist24() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("3c15b1af-8b00-4356-ae83-873a553d99c6"), SimilarSongId = Guid.Parse("3dab7f91-33f3-4271-a2ff-d9b5eb232068") },
                new SongLink { SongId = Guid.Parse("3c15b1af-8b00-4356-ae83-873a553d99c6"), SimilarSongId = Guid.Parse("74316db6-7ad9-4570-b837-136944e986ad") }
            },
            IsActive = true
        };

        public static Song GetMockedSong65() => new Song
        {
            Id = Guid.Parse("3dab7f91-33f3-4271-a2ff-d9b5eb232068"),
            Title = "Love on the Brain",
            ImageUrl = "https://example.com/images/love-on-the-brain.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2016, 1, 30),
            DurationSeconds = 228,
            Album = AlbumMock.GetMockedAlbum28(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist24() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("3dab7f91-33f3-4271-a2ff-d9b5eb232068"), SimilarSongId = Guid.Parse("74316db6-7ad9-4570-b837-136944e986ad") },
                new SongLink { SongId = Guid.Parse("3dab7f91-33f3-4271-a2ff-d9b5eb232068"), SimilarSongId = Guid.Parse("4cf28277-1496-4420-ac8a-9c24e1e41181") }
            },
            IsActive = true
        };

        public static Song GetMockedSong66() => new Song
        {
            Id = Guid.Parse("74316db6-7ad9-4570-b837-136944e986ad"),
            Title = "Four Out Of Five",
            ImageUrl = "https://example.com/images/four-out-of-five.jpg",
            Genre = GenreType.Rock,
            ReleaseDate = new DateOnly(2018, 5, 13),
            DurationSeconds = 275,
            Album = AlbumMock.GetMockedAlbum33(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist28() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("74316db6-7ad9-4570-b837-136944e986ad"), SimilarSongId = Guid.Parse("4cf28277-1496-4420-ac8a-9c24e1e41181") },
                new SongLink { SongId = Guid.Parse("74316db6-7ad9-4570-b837-136944e986ad"), SimilarSongId = Guid.Parse("9a9e7afb-d32b-4975-a7ed-d0174761e6e7") }
            },
            IsActive = true
        };

        public static Song GetMockedSong67() => new Song
        {
            Id = Guid.Parse("4cf28277-1496-4420-ac8a-9c24e1e41181"),
            Title = "One Point Perspective",
            ImageUrl = "https://example.com/images/one-point-perspective.jpg",
            Genre = GenreType.Rock,
            ReleaseDate = new DateOnly(2018, 5, 15),
            DurationSeconds = 243,
            Album = AlbumMock.GetMockedAlbum33(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist28() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("4cf28277-1496-4420-ac8a-9c24e1e41181"), SimilarSongId = Guid.Parse("9a9e7afb-d32b-4975-a7ed-d0174761e6e7") },
                new SongLink { SongId = Guid.Parse("4cf28277-1496-4420-ac8a-9c24e1e41181"), SimilarSongId = Guid.Parse("ad372f33-1ace-46da-b3ef-f9156398a019") }
            },
            IsActive = true
        };

        public static Song GetMockedSong68() => new Song
        {
            Id = Guid.Parse("9a9e7afb-d32b-4975-a7ed-d0174761e6e7"),
            Title = "Star Treatment",
            ImageUrl = "https://example.com/images/star-treatment.jpg",
            Genre = GenreType.Rock,
            ReleaseDate = new DateOnly(2018, 5, 11),
            DurationSeconds = 345,
            Album = AlbumMock.GetMockedAlbum33(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist28() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("9a9e7afb-d32b-4975-a7ed-d0174761e6e7"), SimilarSongId = Guid.Parse("ad372f33-1ace-46da-b3ef-f9156398a019") },
                new SongLink { SongId = Guid.Parse("9a9e7afb-d32b-4975-a7ed-d0174761e6e7"), SimilarSongId = Guid.Parse("bc795075-07ac-4458-a8a2-56b7d8ab0437") }
            },
            IsActive = true
        };

        public static Song GetMockedSong69() => new Song
        {
            Id = Guid.Parse("ad372f33-1ace-46da-b3ef-f9156398a019"),
            Title = "Batphone",
            ImageUrl = "https://example.com/images/batphone.jpg",
            Genre = GenreType.Rock,
            ReleaseDate = new DateOnly(2018, 5, 13),
            DurationSeconds = 300,
            Album = AlbumMock.GetMockedAlbum33(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist28() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("ad372f33-1ace-46da-b3ef-f9156398a019"), SimilarSongId = Guid.Parse("bc795075-07ac-4458-a8a2-56b7d8ab0437") },
                new SongLink { SongId = Guid.Parse("ad372f33-1ace-46da-b3ef-f9156398a019"), SimilarSongId = Guid.Parse("72c4be16-5b12-432b-8cec-4fa162788527") }
            },
            IsActive = true
        };

        public static Song GetMockedSong70() => new Song
        {
            Id = Guid.Parse("bc795075-07ac-4458-a8a2-56b7d8ab0437"),
            Title = "Paradise",
            ImageUrl = "https://example.com/images/paradise.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2016, 8, 20),
            DurationSeconds = 278,
            Album = AlbumMock.GetMockedAlbum24(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist5() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("bc795075-07ac-4458-a8a2-56b7d8ab0437"), SimilarSongId = Guid.Parse("72c4be16-5b12-432b-8cec-4fa162788527") },
                new SongLink { SongId = Guid.Parse("bc795075-07ac-4458-a8a2-56b7d8ab0437"), SimilarSongId = Guid.Parse("f111ef40-05d2-4e04-9856-d63fade4a12d") }
            },
            IsActive = true
        };

        public static Song GetMockedSong71() => new Song
        {
            Id = Guid.Parse("72c4be16-5b12-432b-8cec-4fa162788527"),
            Title = "Moon River",
            ImageUrl = "https://example.com/images/moon-river.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2016, 8, 22),
            DurationSeconds = 219,
            Album = AlbumMock.GetMockedAlbum24(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist5() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("72c4be16-5b12-432b-8cec-4fa162788527"), SimilarSongId = Guid.Parse("f111ef40-05d2-4e04-9856-d63fade4a12d") },
                new SongLink { SongId = Guid.Parse("72c4be16-5b12-432b-8cec-4fa162788527"), SimilarSongId = Guid.Parse("f027a18c-3164-42a2-b49b-f299a1477798") }
            },
            IsActive = true
        };

        public static Song GetMockedSong72() => new Song
        {
            Id = Guid.Parse("f111ef40-05d2-4e04-9856-d63fade4a12d"),
            Title = "Pink + White",
            ImageUrl = "https://example.com/images/pink-white.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2016, 8, 24),
            DurationSeconds = 204,
            Album = AlbumMock.GetMockedAlbum24(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist5() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("f111ef40-05d2-4e04-9856-d63fade4a12d"), SimilarSongId = Guid.Parse("f027a18c-3164-42a2-b49b-f299a1477798") },
                new SongLink { SongId = Guid.Parse("f111ef40-05d2-4e04-9856-d63fade4a12d"), SimilarSongId = Guid.Parse("a19c9d5d-5999-4972-aca8-1990d6c854ea") }
            },
            IsActive = true
        };

        public static Song GetMockedSong73() => new Song
        {
            Id = Guid.Parse("f027a18c-3164-42a2-b49b-f299a1477798"),
            Title = "Nikes",
            ImageUrl = "https://example.com/images/nikes.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2016, 8, 26),
            DurationSeconds = 314,
            Album = AlbumMock.GetMockedAlbum24(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist5() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("f027a18c-3164-42a2-b49b-f299a1477798"), SimilarSongId = Guid.Parse("a19c9d5d-5999-4972-aca8-1990d6c854ea") },
                new SongLink { SongId = Guid.Parse("f027a18c-3164-42a2-b49b-f299a1477798"), SimilarSongId = Guid.Parse("46d680bc-036d-4e72-bb12-1d8ed212c83a") }
            },
            IsActive = true
        };

        public static Song GetMockedSong74() => new Song
        {
            Id = Guid.Parse("a19c9d5d-5999-4972-aca8-1990d6c854ea"),
            Title = "Solo (Reprise)",
            ImageUrl = "https://example.com/images/solo-reprise.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2016, 8, 21),
            DurationSeconds = 157,
            Album = AlbumMock.GetMockedAlbum24(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist5() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("a19c9d5d-5999-4972-aca8-1990d6c854ea"), SimilarSongId = Guid.Parse("46d680bc-036d-4e72-bb12-1d8ed212c83a") },
                new SongLink { SongId = Guid.Parse("a19c9d5d-5999-4972-aca8-1990d6c854ea"), SimilarSongId = Guid.Parse("8f162292-9e44-4003-97b8-86fe410c486a") }
            },
            IsActive = true
        };

        public static Song GetMockedSong75() => new Song
        {
            Id = Guid.Parse("46d680bc-036d-4e72-bb12-1d8ed212c83a"),
            Title = "Green Light",
            ImageUrl = "https://example.com/images/green-light.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2017, 6, 16),
            DurationSeconds = 234,
            Album = AlbumMock.GetMockedAlbum26(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist15() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("46d680bc-036d-4e72-bb12-1d8ed212c83a"), SimilarSongId = Guid.Parse("8f162292-9e44-4003-97b8-86fe410c486a") },
                new SongLink { SongId = Guid.Parse("46d680bc-036d-4e72-bb12-1d8ed212c83a"), SimilarSongId = Guid.Parse("b2c5d58f-9983-48c5-8a1e-96dafdb3307c") }
            },
            IsActive = true
        };

        public static Song GetMockedSong76() => new Song
        {
            Id = Guid.Parse("8f162292-9e44-4003-97b8-86fe410c486a"),
            Title = "Homemade Dynamite",
            ImageUrl = "https://example.com/images/homemade-dynamite.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2017, 6, 18),
            DurationSeconds = 214,
            Album = AlbumMock.GetMockedAlbum26(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist15() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("8f162292-9e44-4003-97b8-86fe410c486a"), SimilarSongId = Guid.Parse("b2c5d58f-9983-48c5-8a1e-96dafdb3307c") },
                new SongLink { SongId = Guid.Parse("8f162292-9e44-4003-97b8-86fe410c486a"), SimilarSongId = Guid.Parse("d7cc064f-70a0-43ad-bd86-8d200580d6b8") }
            },
            IsActive = true
        };

        public static Song GetMockedSong77() => new Song
        {
            Id = Guid.Parse("b2c5d58f-9983-48c5-8a1e-96dafdb3307c"),
            Title = "Liability",
            ImageUrl = "https://example.com/images/liability.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2017, 6, 20),
            DurationSeconds = 178,
            Album = AlbumMock.GetMockedAlbum26(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist15() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("b2c5d58f-9983-48c5-8a1e-96dafdb3307c"), SimilarSongId = Guid.Parse("d7cc064f-70a0-43ad-bd86-8d200580d6b8") },
                new SongLink { SongId = Guid.Parse("b2c5d58f-9983-48c5-8a1e-96dafdb3307c"), SimilarSongId = Guid.Parse("daf6789f-eb96-423c-9958-82ba17c4517a") }
            },
            IsActive = true
        };

        public static Song GetMockedSong78() => new Song
        {
            Id = Guid.Parse("d7cc064f-70a0-43ad-bd86-8d200580d6b8"),
            Title = "Perfect Places",
            ImageUrl = "https://example.com/images/perfect-places.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2017, 6, 22),
            DurationSeconds = 230,
            Album = AlbumMock.GetMockedAlbum26(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist15() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("d7cc064f-70a0-43ad-bd86-8d200580d6b8"), SimilarSongId = Guid.Parse("daf6789f-eb96-423c-9958-82ba17c4517a") },
                new SongLink { SongId = Guid.Parse("d7cc064f-70a0-43ad-bd86-8d200580d6b8"), SimilarSongId = Guid.Parse("5248af81-7450-48a8-bce9-d179a79101b3") }
            },
            IsActive = true
        };

        public static Song GetMockedSong79() => new Song
        {
            Id = Guid.Parse("daf6789f-eb96-423c-9958-82ba17c4517a"),
            Title = "Rollercoaster",
            ImageUrl = "https://example.com/images/rollercoaster.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2013, 6, 18),
            DurationSeconds = 212,
            Album = AlbumMock.GetMockedAlbum36(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist21() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("daf6789f-eb96-423c-9958-82ba17c4517a"), SimilarSongId = Guid.Parse("5248af81-7450-48a8-bce9-d179a79101b3") },
                new SongLink { SongId = Guid.Parse("daf6789f-eb96-423c-9958-82ba17c4517a"), SimilarSongId = Guid.Parse("a8bdf565-b0d2-46dd-aa34-fedd762ff61c") }
            },
            IsActive = true
        };

        public static Song GetMockedSong80() => new Song
        {
            Id = Guid.Parse("5248af81-7450-48a8-bce9-d179a79101b3"),
            Title = "Black Skinhead",
            ImageUrl = "https://example.com/images/black-skinhead.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2013, 6, 20),
            DurationSeconds = 188,
            Album = AlbumMock.GetMockedAlbum36(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist21() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("5248af81-7450-48a8-bce9-d179a79101b3"), SimilarSongId = Guid.Parse("a8bdf565-b0d2-46dd-aa34-fedd762ff61c") },
                new SongLink { SongId = Guid.Parse("5248af81-7450-48a8-bce9-d179a79101b3"), SimilarSongId = Guid.Parse("be8f4a76-9966-4b2f-95ef-067dd7879655") }
            },
            IsActive = true
        };

        public static Song GetMockedSong81() => new Song
        {
            Id = Guid.Parse("a8bdf565-b0d2-46dd-aa34-fedd762ff61c"),
            Title = "Bound 2",
            ImageUrl = "https://example.com/images/bound2.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2013, 6, 22),
            DurationSeconds = 223,
            Album = AlbumMock.GetMockedAlbum36(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist21() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("a8bdf565-b0d2-46dd-aa34-fedd762ff61c"), SimilarSongId = Guid.Parse("be8f4a76-9966-4b2f-95ef-067dd7879655") },
                new SongLink { SongId = Guid.Parse("a8bdf565-b0d2-46dd-aa34-fedd762ff61c"), SimilarSongId = Guid.Parse("5904e413-0619-4099-afbc-71e133a68511") }
            },
            IsActive = true
        };

        public static Song GetMockedSong82() => new Song
        {
            Id = Guid.Parse("be8f4a76-9966-4b2f-95ef-067dd7879655"),
            Title = "I'm In It",
            ImageUrl = "https://example.com/images/im-in-it.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2013, 6, 19),
            DurationSeconds = 233,
            Album = AlbumMock.GetMockedAlbum36(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist21() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("be8f4a76-9966-4b2f-95ef-067dd7879655"), SimilarSongId = Guid.Parse("5904e413-0619-4099-afbc-71e133a68511") },
                new SongLink { SongId = Guid.Parse("be8f4a76-9966-4b2f-95ef-067dd7879655"), SimilarSongId = Guid.Parse("a5780fe8-4708-4574-a3de-416064b970d1") }
            },
            IsActive = true
        };

        public static Song GetMockedSong83() => new Song
        {
            Id = Guid.Parse("5904e413-0619-4099-afbc-71e133a68511"),
            Title = "Blood On The Leaves",
            ImageUrl = "https://example.com/images/blood-on-the-leaves.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2013, 6, 19),
            DurationSeconds = 306,
            Album = AlbumMock.GetMockedAlbum36(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist21() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("5904e413-0619-4099-afbc-71e133a68511"), SimilarSongId = Guid.Parse("a5780fe8-4708-4574-a3de-416064b970d1") },
                new SongLink { SongId = Guid.Parse("5904e413-0619-4099-afbc-71e133a68511"), SimilarSongId = Guid.Parse("212c0538-0cb2-4126-bcc9-baaa8265afb2") }
            },
            IsActive = true
        };

        public static Song GetMockedSong84() => new Song
        {
            Id = Guid.Parse("a5780fe8-4708-4574-a3de-416064b970d1"),
            Title = "Reflection",
            ImageUrl = "https://example.com/images/reflection.jpg",
            Genre = GenreType.Pop,
            ReleaseDate = new DateOnly(2015, 3, 15),
            DurationSeconds = 228,
            Album = AlbumMock.GetMockedAlbum32(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist4() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("a5780fe8-4708-4574-a3de-416064b970d1"), SimilarSongId = Guid.Parse("212c0538-0cb2-4126-bcc9-baaa8265afb2") },
                new SongLink { SongId = Guid.Parse("a5780fe8-4708-4574-a3de-416064b970d1"), SimilarSongId = Guid.Parse("748d7f2c-0e5d-45cd-8f5d-77da302bbc0c") }
            },
            IsActive = true
        };

        public static Song GetMockedSong85() => new Song
        {
            Id = Guid.Parse("212c0538-0cb2-4126-bcc9-baaa8265afb2"),
            Title = "Alright",
            ImageUrl = "https://example.com/images/alright.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2015, 3, 17),
            DurationSeconds = 225,
            Album = AlbumMock.GetMockedAlbum32(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist4() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("212c0538-0cb2-4126-bcc9-baaa8265afb2"), SimilarSongId = Guid.Parse("748d7f2c-0e5d-45cd-8f5d-77da302bbc0c") },
                new SongLink { SongId = Guid.Parse("212c0538-0cb2-4126-bcc9-baaa8265afb2"), SimilarSongId = Guid.Parse("3a2b8ce7-e279-42dd-8905-a42d35bf6fa0") }
            },
            IsActive = false
        };

        public static Song GetMockedSong86() => new Song
        {
            Id = Guid.Parse("748d7f2c-0e5d-45cd-8f5d-77da302bbc0c"),
            Title = "King Kunta",
            ImageUrl = "https://example.com/images/king-kunta.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2015, 3, 19),
            DurationSeconds = 234,
            Album = AlbumMock.GetMockedAlbum32(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist4() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("748d7f2c-0e5d-45cd-8f5d-77da302bbc0c"), SimilarSongId = Guid.Parse("3a2b8ce7-e279-42dd-8905-a42d35bf6fa0") },
                new SongLink { SongId = Guid.Parse("748d7f2c-0e5d-45cd-8f5d-77da302bbc0c"), SimilarSongId = Guid.Parse("af20c27b-e20c-459a-bbde-7603fc8715fc") }
            },
            IsActive = false
        };

        public static Song GetMockedSong87() => new Song
        {
            Id = Guid.Parse("3a2b8ce7-e279-42dd-8905-a42d35bf6fa0"),
            Title = "The Blacker The Berry",
            ImageUrl = "https://example.com/images/the-blacker-the-berry.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2015, 3, 16),
            DurationSeconds = 312,
            Album = AlbumMock.GetMockedAlbum32(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist4() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("3a2b8ce7-e279-42dd-8905-a42d35bf6fa0"), SimilarSongId = Guid.Parse("af20c27b-e20c-459a-bbde-7603fc8715fc") },
                new SongLink { SongId = Guid.Parse("3a2b8ce7-e279-42dd-8905-a42d35bf6fa0"), SimilarSongId = Guid.Parse("a23d5a35-4168-40d2-a8eb-cd15f71f120c") }
            },
            IsActive = false
        };

        public static Song GetMockedSong88() => new Song
        {
            Id = Guid.Parse("af20c27b-e20c-459a-bbde-7603fc8715fc"),
            Title = "These Walls",
            ImageUrl = "https://example.com/images/these-walls.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2015, 3, 16),
            DurationSeconds = 287,
            Album = AlbumMock.GetMockedAlbum32(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist4() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("af20c27b-e20c-459a-bbde-7603fc8715fc"), SimilarSongId = Guid.Parse("a23d5a35-4168-40d2-a8eb-cd15f71f120c") },
                new SongLink { SongId = Guid.Parse("af20c27b-e20c-459a-bbde-7603fc8715fc"), SimilarSongId = Guid.Parse("8b39efa4-ce8d-4617-84e7-45bf095c290a") }
            },
            IsActive = false
        };

        public static Song GetMockedSong89() => new Song
        {
            Id = Guid.Parse("a23d5a35-4168-40d2-a8eb-cd15f71f120c"),
            Title = "Complexion (A Zulu Love)",
            ImageUrl = "https://example.com/images/complexion.jpg",
            Genre = GenreType.HipHop,
            ReleaseDate = new DateOnly(2015, 3, 16),
            DurationSeconds = 279,
            Album = AlbumMock.GetMockedAlbum32(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist4() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("a23d5a35-4168-40d2-a8eb-cd15f71f120c"), SimilarSongId = Guid.Parse("8b39efa4-ce8d-4617-84e7-45bf095c290a") },
                new SongLink { SongId = Guid.Parse("a23d5a35-4168-40d2-a8eb-cd15f71f120c"), SimilarSongId = Guid.Parse("64f534f8-f2d4-4402-95a3-54de48b678a8") }
            },
            IsActive = false
        };

        public static Song GetMockedSong90() => new Song
        {
            Id = Guid.Parse("8b39efa4-ce8d-4617-84e7-45bf095c290a"),
            Title = "Evening Shadows",
            ImageUrl = "https://example.com/images/evening-shadows.jpg",
            Genre = GenreType.Jazz,
            ReleaseDate = new DateOnly(2021, 5, 25),
            DurationSeconds = 194,
            Album = AlbumMock.GetMockedAlbum1(),
            Artists = new List<Artist> { ArtistMock.GetMockedArtist16() },
            SimilarSongs = new List<SongLink>
            {
                new SongLink { SongId = Guid.Parse("8b39efa4-ce8d-4617-84e7-45bf095c290a"), SimilarSongId = Guid.Parse("64f534f8-f2d4-4402-95a3-54de48b678a8") },
                new SongLink { SongId = Guid.Parse("8b39efa4-ce8d-4617-84e7-45bf095c290a"), SimilarSongId = Guid.Parse("278cfa5a-6f44-420e-9930-07da6c43a6ad") }
            },
            IsActive = true
        };

        public static SongDto GetMockedSongDto1() => ToDto(GetMockedSong1());

        public static SongDto GetMockedSongDto2() => ToDto(GetMockedSong2());

        public static SongDto GetMockedSongDto3() => ToDto(GetMockedSong3());

        public static SongDto GetMockedSongDto4() => ToDto(GetMockedSong4());

        public static SongDto GetMockedSongDto5() => ToDto(GetMockedSong5());

        public static SongDto GetMockedSongDto6() => ToDto(GetMockedSong6());

        public static SongDto GetMockedSongDto7() => ToDto(GetMockedSong7());

        public static SongDto GetMockedSongDto8() => ToDto(GetMockedSong8());

        public static SongDto GetMockedSongDto9() => ToDto(GetMockedSong9());

        public static SongDto GetMockedSongDto10() => ToDto(GetMockedSong10());

        public static SongDto GetMockedSongDto11() => ToDto(GetMockedSong11());

        public static SongDto GetMockedSongDto12() => ToDto(GetMockedSong12());

        public static SongDto GetMockedSongDto13() => ToDto(GetMockedSong13());

        public static SongDto GetMockedSongDto14() => ToDto(GetMockedSong14());

        public static SongDto GetMockedSongDto15() => ToDto(GetMockedSong15());

        public static SongDto GetMockedSongDto16() => ToDto(GetMockedSong16());

        public static SongDto GetMockedSongDto17() => ToDto(GetMockedSong17());

        public static SongDto GetMockedSongDto18() => ToDto(GetMockedSong18());

        public static SongDto GetMockedSongDto19() => ToDto(GetMockedSong19());

        public static SongDto GetMockedSongDto20() => ToDto(GetMockedSong20());

        public static SongDto GetMockedSongDto21() => ToDto(GetMockedSong21());

        public static SongDto GetMockedSongDto22() => ToDto(GetMockedSong22());

        public static SongDto GetMockedSongDto23() => ToDto(GetMockedSong23());

        public static SongDto GetMockedSongDto24() => ToDto(GetMockedSong24());

        public static SongDto GetMockedSongDto25() => ToDto(GetMockedSong25());

        public static SongDto GetMockedSongDto26() => ToDto(GetMockedSong26());

        public static SongDto GetMockedSongDto27() => ToDto(GetMockedSong27());

        public static SongDto GetMockedSongDto28() => ToDto(GetMockedSong28());

        public static SongDto GetMockedSongDto29() => ToDto(GetMockedSong29());

        public static SongDto GetMockedSongDto30() => ToDto(GetMockedSong30());

        public static SongDto GetMockedSongDto31() => ToDto(GetMockedSong31());

        public static SongDto GetMockedSongDto32() => ToDto(GetMockedSong32());

        public static SongDto GetMockedSongDto33() => ToDto(GetMockedSong33());

        public static SongDto GetMockedSongDto34() => ToDto(GetMockedSong34());

        public static SongDto GetMockedSongDto35() => ToDto(GetMockedSong35());

        public static SongDto GetMockedSongDto36() => ToDto(GetMockedSong36());

        public static SongDto GetMockedSongDto37() => ToDto(GetMockedSong37());

        public static SongDto GetMockedSongDto38() => ToDto(GetMockedSong38());

        public static SongDto GetMockedSongDto39() => ToDto(GetMockedSong39());

        public static SongDto GetMockedSongDto40() => ToDto(GetMockedSong40());

        public static SongDto GetMockedSongDto41() => ToDto(GetMockedSong41());

        public static SongDto GetMockedSongDto42() => ToDto(GetMockedSong42());

        public static SongDto GetMockedSongDto43() => ToDto(GetMockedSong43());

        public static SongDto GetMockedSongDto44() => ToDto(GetMockedSong44());

        public static SongDto GetMockedSongDto45() => ToDto(GetMockedSong45());

        public static SongDto GetMockedSongDto46() => ToDto(GetMockedSong46());

        public static SongDto GetMockedSongDto47() => ToDto(GetMockedSong47());

        public static SongDto GetMockedSongDto48() => ToDto(GetMockedSong48());

        public static SongDto GetMockedSongDto49() => ToDto(GetMockedSong49());

        public static SongDto GetMockedSongDto50() => ToDto(GetMockedSong50());

        public static SongDto GetMockedSongDto51() => ToDto(GetMockedSong51());

        public static SongDto GetMockedSongDto52() => ToDto(GetMockedSong52());

        public static SongDto GetMockedSongDto53() => ToDto(GetMockedSong53());

        public static SongDto GetMockedSongDto54() => ToDto(GetMockedSong54());

        public static SongDto GetMockedSongDto55() => ToDto(GetMockedSong55());

        public static SongDto GetMockedSongDto56() => ToDto(GetMockedSong56());

        public static SongDto GetMockedSongDto57() => ToDto(GetMockedSong57());

        public static SongDto GetMockedSongDto58() => ToDto(GetMockedSong58());

        public static SongDto GetMockedSongDto59() => ToDto(GetMockedSong59());

        public static SongDto GetMockedSongDto60() => ToDto(GetMockedSong60());

        public static SongDto GetMockedSongDto61() => ToDto(GetMockedSong61());

        public static SongDto GetMockedSongDto62() => ToDto(GetMockedSong62());

        public static SongDto GetMockedSongDto63() => ToDto(GetMockedSong63());

        public static SongDto GetMockedSongDto64() => ToDto(GetMockedSong64());

        public static SongDto GetMockedSongDto65() => ToDto(GetMockedSong65());

        public static SongDto GetMockedSongDto66() => ToDto(GetMockedSong66());

        public static SongDto GetMockedSongDto67() => ToDto(GetMockedSong67());

        public static SongDto GetMockedSongDto68() => ToDto(GetMockedSong68());

        public static SongDto GetMockedSongDto69() => ToDto(GetMockedSong69());

        public static SongDto GetMockedSongDto70() => ToDto(GetMockedSong70());

        public static SongDto GetMockedSongDto71() => ToDto(GetMockedSong71());

        public static SongDto GetMockedSongDto72() => ToDto(GetMockedSong72());

        public static SongDto GetMockedSongDto73() => ToDto(GetMockedSong73());

        public static SongDto GetMockedSongDto74() => ToDto(GetMockedSong74());

        public static SongDto GetMockedSongDto75() => ToDto(GetMockedSong75());

        public static SongDto GetMockedSongDto76() => ToDto(GetMockedSong76());

        public static SongDto GetMockedSongDto77() => ToDto(GetMockedSong77());

        public static SongDto GetMockedSongDto78() => ToDto(GetMockedSong78());

        public static SongDto GetMockedSongDto79() => ToDto(GetMockedSong79());

        public static SongDto GetMockedSongDto80() => ToDto(GetMockedSong80());

        public static SongDto GetMockedSongDto81() => ToDto(GetMockedSong81());

        public static SongDto GetMockedSongDto82() => ToDto(GetMockedSong82());

        public static SongDto GetMockedSongDto83() => ToDto(GetMockedSong83());

        public static SongDto GetMockedSongDto84() => ToDto(GetMockedSong84());

        public static SongDto GetMockedSongDto85() => ToDto(GetMockedSong85());

        public static SongDto GetMockedSongDto86() => ToDto(GetMockedSong86());

        public static SongDto GetMockedSongDto87() => ToDto(GetMockedSong87());

        public static SongDto GetMockedSongDto88() => ToDto(GetMockedSong88());

        public static SongDto GetMockedSongDto89() => ToDto(GetMockedSong89());

        public static SongDto GetMockedSongDto90() => ToDto(GetMockedSong90());

        private static SongDto ToDto(Song song) => new SongDto
        {
            Id = song.Id,
            Title = song.Title,
            ImageUrl = song.ImageUrl,
            Genre = song.Genre,
            ReleaseDate = song.ReleaseDate,
            DurationSeconds = song.DurationSeconds,
            AlbumId = song.Album.Id,
            ArtistsIds = song.Artists.Select(artist => artist.Id).ToList(),
            SimilarSongsIds = song.SimilarSongs.Select(songLink => songLink.SimilarSongId).ToList(),
            IsActive = song.IsActive
        };
    }
}