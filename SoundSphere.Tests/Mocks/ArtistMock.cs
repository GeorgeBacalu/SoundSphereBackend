using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;

namespace SoundSphere.Tests.Mocks
{
    public class ArtistMock
    {
        private ArtistMock() { }

        public static IList<Artist> GetMockedArtists() => new List<Artist>
        { 
            GetMockedArtist1(), GetMockedArtist2(), GetMockedArtist3(), GetMockedArtist4(), GetMockedArtist5(), GetMockedArtist6(), GetMockedArtist7(), GetMockedArtist8(), GetMockedArtist9(), GetMockedArtist10(),
            GetMockedArtist11(), GetMockedArtist12(), GetMockedArtist13(), GetMockedArtist14(), GetMockedArtist15(), GetMockedArtist16(), GetMockedArtist17(), GetMockedArtist18(), GetMockedArtist19(), GetMockedArtist20(),
            GetMockedArtist21(), GetMockedArtist22(), GetMockedArtist23(), GetMockedArtist24(), GetMockedArtist25(), GetMockedArtist26(), GetMockedArtist27(), GetMockedArtist28(), GetMockedArtist29(), GetMockedArtist30(),
            GetMockedArtist31(), GetMockedArtist32(), GetMockedArtist33(), GetMockedArtist34(), GetMockedArtist35(), GetMockedArtist36(), GetMockedArtist37(), GetMockedArtist38(), GetMockedArtist39(), GetMockedArtist40(),
            GetMockedArtist41(), GetMockedArtist42(), GetMockedArtist43(), GetMockedArtist44(), GetMockedArtist45(), GetMockedArtist46(), GetMockedArtist47(), GetMockedArtist48(), GetMockedArtist49(), GetMockedArtist50()
        };

        public static IList<ArtistDto> GetMockedArtistDtos() => GetMockedArtists().Select(ToDto).ToList();

        public static IList<Artist> GetMockedPaginatedArtists() => GetMockedArtists().Where(artist => artist.DeletedAt == null).Take(10).ToList();

        public static IList<ArtistDto> GetMockedPaginatedArtistDtos() => GetMockedPaginatedArtists().Select(ToDto).ToList();

        public static ArtistPaginationRequest GetMockedArtistsPaginationRequest() => new ArtistPaginationRequest(
            SortCriteria: new Dictionary<ArtistSortCriterion, SortOrder> { { ArtistSortCriterion.ByName, SortOrder.Ascending } },
            SearchCriteria: new List<ArtistSearchCriterion>() { ArtistSearchCriterion.ByName },
            Name: "B");

        public static Artist GetMockedArtist1() => new Artist
        {
            Id = Guid.Parse("4e75ecdd-aafe-4c35-836b-1b83fc7b8f88"),
            Name = "The Weeknd",
            ImageUrl = "https://example.com/images/the-weeknd.jpg",
            Bio = "Canadian singer, songwriter, and record producer known for his flamboyant style and broad vocal range.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("4e75ecdd-aafe-4c35-836b-1b83fc7b8f88"), SimilarArtistId = Guid.Parse("8c301aa9-6d56-4c06-b1f2-9b9956979345") },
                new ArtistLink { ArtistId = Guid.Parse("4e75ecdd-aafe-4c35-836b-1b83fc7b8f88"), SimilarArtistId = Guid.Parse("fc3e6343-6f10-42e8-91c9-2d36c0154c42") }
            },
            CreatedAt = new DateTime(2024, 3, 1, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist2() => new Artist
        {
            Id = Guid.Parse("8c301aa9-6d56-4c06-b1f2-9b9956979345"),
            Name = "Lana Del Rey",
            ImageUrl = "https://example.com/images/lana-del-rey.jpg",
            Bio = "American singer and songwriter noted for her cinematic sound and aesthetic in music videos.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("8c301aa9-6d56-4c06-b1f2-9b9956979345"), SimilarArtistId = Guid.Parse("fc3e6343-6f10-42e8-91c9-2d36c0154c42") },
                new ArtistLink { ArtistId = Guid.Parse("8c301aa9-6d56-4c06-b1f2-9b9956979345"), SimilarArtistId = Guid.Parse("96de52c4-7dbf-4db1-baed-255c8f9215db") }
            },
            CreatedAt = new DateTime(2024, 3, 2, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist3() => new Artist
        {
            Id = Guid.Parse("fc3e6343-6f10-42e8-91c9-2d36c0154c42"),
            Name = "Daft Punk",
            ImageUrl = "https://example.com/images/daft-punk.jpg",
            Bio = "French electronic music duo known for their visual stylization and disguises associated with their music.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("fc3e6343-6f10-42e8-91c9-2d36c0154c42"), SimilarArtistId = Guid.Parse("96de52c4-7dbf-4db1-baed-255c8f9215db") },
                new ArtistLink { ArtistId = Guid.Parse("fc3e6343-6f10-42e8-91c9-2d36c0154c42"), SimilarArtistId = Guid.Parse("66c4b36e-0fac-4911-9e9a-98d42d8ece10") }
            },
            CreatedAt = new DateTime(2024, 3, 3, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist4() => new Artist
        {
            Id = Guid.Parse("96de52c4-7dbf-4db1-baed-255c8f9215db"),
            Name = "Kendrick Lamar",
            ImageUrl = "https://example.com/images/kendrick-lamar.jpg",
            Bio = "American rapper and songwriter acclaimed for his progressive musical styles and thoughtful lyrics.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("96de52c4-7dbf-4db1-baed-255c8f9215db"), SimilarArtistId = Guid.Parse("66c4b36e-0fac-4911-9e9a-98d42d8ece10") },
                new ArtistLink { ArtistId = Guid.Parse("96de52c4-7dbf-4db1-baed-255c8f9215db"), SimilarArtistId = Guid.Parse("1d9d1b54-6430-45c5-85d6-ae4d2288223f") }
            },
            CreatedAt = new DateTime(2024, 3, 4, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist5() => new Artist
        {
            Id = Guid.Parse("66c4b36e-0fac-4911-9e9a-98d42d8ece10"),
            Name = "Frank Ocean",
            ImageUrl = "https://example.com/images/frank-ocean.jpg",
            Bio = "American singer, songwriter, and photographer known for his idiosyncratic musical approach.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("66c4b36e-0fac-4911-9e9a-98d42d8ece10"), SimilarArtistId = Guid.Parse("1d9d1b54-6430-45c5-85d6-ae4d2288223f") },
                new ArtistLink { ArtistId = Guid.Parse("66c4b36e-0fac-4911-9e9a-98d42d8ece10"), SimilarArtistId = Guid.Parse("6fbd52c1-d395-4349-975b-70916be1b8d0") }
            },
            CreatedAt = new DateTime(2024, 3, 5, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist6() => new Artist
        {
            Id = Guid.Parse("1d9d1b54-6430-45c5-85d6-ae4d2288223f"),
            Name = "Sam Smith",
            ImageUrl = "https://example.com/images/sam-smith.jpg",
            Bio = "English singer and songwriter known for their soulful voice and emotive lyrics.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("1d9d1b54-6430-45c5-85d6-ae4d2288223f"), SimilarArtistId = Guid.Parse("6fbd52c1-d395-4349-975b-70916be1b8d0") },
                new ArtistLink { ArtistId = Guid.Parse("1d9d1b54-6430-45c5-85d6-ae4d2288223f"), SimilarArtistId = Guid.Parse("b37cefef-c6d2-49a7-8b12-cdeacf5fe4c1") }
            },
            CreatedAt = new DateTime(2024, 3, 6, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist7() => new Artist
        {
            Id = Guid.Parse("6fbd52c1-d395-4349-975b-70916be1b8d0"),
            Name = "Pink Floyd",
            ImageUrl = "https://example.com/images/pink-floyd.jpg",
            Bio = "British rock band famed for their philosophical lyrics, sonic experimentation, and elaborate live shows.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("6fbd52c1-d395-4349-975b-70916be1b8d0"), SimilarArtistId = Guid.Parse("b37cefef-c6d2-49a7-8b12-cdeacf5fe4c1") },
                new ArtistLink { ArtistId = Guid.Parse("6fbd52c1-d395-4349-975b-70916be1b8d0"), SimilarArtistId = Guid.Parse("08af1af0-b71e-44bc-b5d3-82dab55d6b81") }
            },
            CreatedAt = new DateTime(2024, 3, 7, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist8() => new Artist
        {
            Id = Guid.Parse("b37cefef-c6d2-49a7-8b12-cdeacf5fe4c1"),
            Name = "Michael Jackson",
            ImageUrl = "https://example.com/images/michael-jackson.jpg",
            Bio = "American singer, songwriter, and dancer dubbed the \"King of Pop,\" he is regarded as one of the most significant cultural figures of the 20th century.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("b37cefef-c6d2-49a7-8b12-cdeacf5fe4c1"), SimilarArtistId = Guid.Parse("08af1af0-b71e-44bc-b5d3-82dab55d6b81") },
                new ArtistLink { ArtistId = Guid.Parse("b37cefef-c6d2-49a7-8b12-cdeacf5fe4c1"), SimilarArtistId = Guid.Parse("2f715aa4-7597-4f8e-9594-1f0245d16a97") }
            },
            CreatedAt = new DateTime(2024, 3, 8, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist9() => new Artist
        {
            Id = Guid.Parse("08af1af0-b71e-44bc-b5d3-82dab55d6b81"),
            Name = "David Bowie",
            ImageUrl = "https://example.com/images/david-bowie.jpg",
            Bio = "English singer-songwriter and actor known for his reinvention and visual presentation.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("08af1af0-b71e-44bc-b5d3-82dab55d6b81"), SimilarArtistId = Guid.Parse("2f715aa4-7597-4f8e-9594-1f0245d16a97") },
                new ArtistLink { ArtistId = Guid.Parse("08af1af0-b71e-44bc-b5d3-82dab55d6b81"), SimilarArtistId = Guid.Parse("6a1e646b-e42d-4336-88c6-cb44d5f50682") }
            },
            CreatedAt = new DateTime(2024, 3, 9, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist10() => new Artist
        {
            Id = Guid.Parse("2f715aa4-7597-4f8e-9594-1f0245d16a97"),
            Name = "Taylor Swift",
            ImageUrl = "https://example.com/images/taylor-swift.jpg",
            Bio = "American singer-songwriter whose discography spans diverse genres, and is known for her narrative songwriting.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("2f715aa4-7597-4f8e-9594-1f0245d16a97"), SimilarArtistId = Guid.Parse("6a1e646b-e42d-4336-88c6-cb44d5f50682") },
                new ArtistLink { ArtistId = Guid.Parse("2f715aa4-7597-4f8e-9594-1f0245d16a97"), SimilarArtistId = Guid.Parse("3f7b8c31-2f1f-4a5b-8b01-3395bf4c0931") }
            },
            CreatedAt = new DateTime(2024, 3, 10, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist11() => new Artist
        {
            Id = Guid.Parse("6a1e646b-e42d-4336-88c6-cb44d5f50682"),
            Name = "Harry Styles",
            ImageUrl = "https://example.com/images/harry-styles.jpg",
            Bio = "British singer, songwriter, and actor who began his music career as a member of the boy band One Direction.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("6a1e646b-e42d-4336-88c6-cb44d5f50682"), SimilarArtistId = Guid.Parse("3f7b8c31-2f1f-4a5b-8b01-3395bf4c0931") },
                new ArtistLink { ArtistId = Guid.Parse("6a1e646b-e42d-4336-88c6-cb44d5f50682"), SimilarArtistId = Guid.Parse("3a448c5a-e1c8-456e-955f-ed2f98d87aa6") }
            },
            CreatedAt = new DateTime(2024, 3, 11, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist12() => new Artist
        {
            Id = Guid.Parse("3f7b8c31-2f1f-4a5b-8b01-3395bf4c0931"),
            Name = "Radiohead",
            ImageUrl = "https://example.com/images/radiohead.jpg",
            Bio = "English rock band known for their eclectic style and exploration of existential angst and alienation.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("3f7b8c31-2f1f-4a5b-8b01-3395bf4c0931"), SimilarArtistId = Guid.Parse("3a448c5a-e1c8-456e-955f-ed2f98d87aa6") },
                new ArtistLink { ArtistId = Guid.Parse("3f7b8c31-2f1f-4a5b-8b01-3395bf4c0931"), SimilarArtistId = Guid.Parse("9e198401-4c15-4d2d-b9fb-63001bee256e") }
            },
            CreatedAt = new DateTime(2024, 3, 12, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist13() => new Artist
        {
            Id = Guid.Parse("3a448c5a-e1c8-456e-955f-ed2f98d87aa6"),
            Name = "Beyonce",
            ImageUrl = "https://example.com/images/beyonce.jpg",
            Bio = "American singer, songwriter, and actress. Born and raised in Houston, Texas, Beyoncé performed in various singing and dancing competitions as a child.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("3a448c5a-e1c8-456e-955f-ed2f98d87aa6"), SimilarArtistId = Guid.Parse("9e198401-4c15-4d2d-b9fb-63001bee256e") },
                new ArtistLink { ArtistId = Guid.Parse("3a448c5a-e1c8-456e-955f-ed2f98d87aa6"), SimilarArtistId = Guid.Parse("60d224af-d862-43e5-b6f8-9c977feb2cfe") }
            },
            CreatedAt = new DateTime(2024, 3, 13, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist14() => new Artist
        {
            Id = Guid.Parse("9e198401-4c15-4d2d-b9fb-63001bee256e"),
            Name = "Adele",
            ImageUrl = "https://example.com/images/adele.jpg",
            Bio = "English singer-songwriter who has sold millions of albums worldwide and won a total of 15 Grammys as well as an Oscar.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("9e198401-4c15-4d2d-b9fb-63001bee256e"), SimilarArtistId = Guid.Parse("60d224af-d862-43e5-b6f8-9c977feb2cfe") },
                new ArtistLink { ArtistId = Guid.Parse("9e198401-4c15-4d2d-b9fb-63001bee256e"), SimilarArtistId = Guid.Parse("e5b25542-e19b-4661-a5e8-72fa34bfb7c9") }
            },
            CreatedAt = new DateTime(2024, 3, 14, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist15() => new Artist
        {
            Id = Guid.Parse("60d224af-d862-43e5-b6f8-9c977feb2cfe"),
            Name = "ABBA",
            ImageUrl = "https://example.com/images/abba.jpg",
            Bio = "Swedish pop group formed in Stockholm in 1972, known for hits like \"Dancing Queen\" and \"Mamma Mia.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("60d224af-d862-43e5-b6f8-9c977feb2cfe"), SimilarArtistId = Guid.Parse("e5b25542-e19b-4661-a5e8-72fa34bfb7c9") },
                new ArtistLink { ArtistId = Guid.Parse("60d224af-d862-43e5-b6f8-9c977feb2cfe"), SimilarArtistId = Guid.Parse("bdfd8e1e-09f2-4a2b-a2ec-6381b3f63616") }
            },
            CreatedAt = new DateTime(2024, 3, 15, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist16() => new Artist
        {
            Id = Guid.Parse("e5b25542-e19b-4661-a5e8-72fa34bfb7c9"),
            Name = "Ed Sheeran",
            ImageUrl = "https://example.com/images/ed-sheeran.jpg",
            Bio = "English singer-songwriter known for his catchy pop songs and heartfelt lyrics.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("e5b25542-e19b-4661-a5e8-72fa34bfb7c9"), SimilarArtistId = Guid.Parse("bdfd8e1e-09f2-4a2b-a2ec-6381b3f63616") },
                new ArtistLink { ArtistId = Guid.Parse("e5b25542-e19b-4661-a5e8-72fa34bfb7c9"), SimilarArtistId = Guid.Parse("7aeef64b-accf-4e2c-8009-1e62cbc85689") }
            },
            CreatedAt = new DateTime(2024, 3, 16, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist17() => new Artist
        {
            Id = Guid.Parse("bdfd8e1e-09f2-4a2b-a2ec-6381b3f63616"),
            Name = "J Balvin",
            ImageUrl = "https://example.com/images/j-balvin.jpg",
            Bio = "Colombian reggaeton singer who has been referred to as the \"Prince of Reggaeton\" and is one of the best-selling Latin music artists.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("bdfd8e1e-09f2-4a2b-a2ec-6381b3f63616"), SimilarArtistId = Guid.Parse("7aeef64b-accf-4e2c-8009-1e62cbc85689") },
                new ArtistLink { ArtistId = Guid.Parse("bdfd8e1e-09f2-4a2b-a2ec-6381b3f63616"), SimilarArtistId = Guid.Parse("f33a1cf0-5c51-4b65-a0fe-88d28cb33155") }
            },
            CreatedAt = new DateTime(2024, 3, 17, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist18() => new Artist
        {
            Id = Guid.Parse("7aeef64b-accf-4e2c-8009-1e62cbc85689"),
            Name = "Travis Scott",
            ImageUrl = "https://example.com/images/travis-scott.jpg",
            Bio = "American rapper, singer, songwriter, and record producer known for his highly auto-tuned half-sung/half-rapped vocal style.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("7aeef64b-accf-4e2c-8009-1e62cbc85689"), SimilarArtistId = Guid.Parse("f33a1cf0-5c51-4b65-a0fe-88d28cb33155") },
                new ArtistLink { ArtistId = Guid.Parse("7aeef64b-accf-4e2c-8009-1e62cbc85689"), SimilarArtistId = Guid.Parse("124f550a-50d0-4f6a-b445-1b9a31a48571") }
            },
            CreatedAt = new DateTime(2024, 3, 18, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist19() => new Artist
        {
            Id = Guid.Parse("f33a1cf0-5c51-4b65-a0fe-88d28cb33155"),
            Name = "Post Malone",
            ImageUrl = "https://example.com/images/post-malone.jpg",
            Bio = "American singer, rapper, and songwriter known for blending genres and subgenres of pop, rap, and rock.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("f33a1cf0-5c51-4b65-a0fe-88d28cb33155"), SimilarArtistId = Guid.Parse("124f550a-50d0-4f6a-b445-1b9a31a48571") },
                new ArtistLink { ArtistId = Guid.Parse("f33a1cf0-5c51-4b65-a0fe-88d28cb33155"), SimilarArtistId = Guid.Parse("dc3f0a12-0809-4fec-b70d-8a2035fc3cf0") }
            },
            CreatedAt = new DateTime(2024, 3, 19, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist20() => new Artist
        {
            Id = Guid.Parse("124f550a-50d0-4f6a-b445-1b9a31a48571"),
            Name = "Amy Winehouse",
            ImageUrl = "https://example.com/images/amy-winehouse.jpg",
            Bio = "English singer and songwriter known for her deep, expressive contralto vocals and her eclectic mix of musical genres.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("124f550a-50d0-4f6a-b445-1b9a31a48571"), SimilarArtistId = Guid.Parse("dc3f0a12-0809-4fec-b70d-8a2035fc3cf0") },
                new ArtistLink { ArtistId = Guid.Parse("124f550a-50d0-4f6a-b445-1b9a31a48571"), SimilarArtistId = Guid.Parse("90d3f8b5-a0fd-4616-a5ed-ebea0340b29f") }
            },
            CreatedAt = new DateTime(2024, 3, 20, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist21() => new Artist
        {
            Id = Guid.Parse("dc3f0a12-0809-4fec-b70d-8a2035fc3cf0"),
            Name = "Kanye West",
            ImageUrl = "https://example.com/images/kanye-west.jpg",
            Bio = "American rapper, singer, songwriter, record producer, and fashion designer, known for his influence on the music industry.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("dc3f0a12-0809-4fec-b70d-8a2035fc3cf0"), SimilarArtistId = Guid.Parse("90d3f8b5-a0fd-4616-a5ed-ebea0340b29f") },
                new ArtistLink { ArtistId = Guid.Parse("dc3f0a12-0809-4fec-b70d-8a2035fc3cf0"), SimilarArtistId = Guid.Parse("2f41b1f0-bf26-406f-95ff-e29ee92b5a3a") }
            },
            CreatedAt = new DateTime(2024, 3, 21, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist22() => new Artist
        {
            Id = Guid.Parse("90d3f8b5-a0fd-4616-a5ed-ebea0340b29f"),
            Name = "Lorde",
            ImageUrl = "https://example.com/images/lorde.jpg",
            Bio = "New Zealand singer-songwriter known for her unconventional musical styles and introspective songwriting.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("90d3f8b5-a0fd-4616-a5ed-ebea0340b29f"), SimilarArtistId = Guid.Parse("2f41b1f0-bf26-406f-95ff-e29ee92b5a3a") },
                new ArtistLink { ArtistId = Guid.Parse("90d3f8b5-a0fd-4616-a5ed-ebea0340b29f"), SimilarArtistId = Guid.Parse("6a9fcd23-2c91-4a32-a3c2-f44e35f1a421") }
            },
            CreatedAt = new DateTime(2024, 3, 22, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist23() => new Artist
        {
            Id = Guid.Parse("2f41b1f0-bf26-406f-95ff-e29ee92b5a3a"),
            Name = "Drake",
            ImageUrl = "https://example.com/images/drake.jpg",
            Bio = "Canadian rapper, singer, and actor from Toronto. He first gained recognition as an actor on the teen drama television series Degrassi: The Next Generation.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("2f41b1f0-bf26-406f-95ff-e29ee92b5a3a"), SimilarArtistId = Guid.Parse("6a9fcd23-2c91-4a32-a3c2-f44e35f1a421") },
                new ArtistLink { ArtistId = Guid.Parse("2f41b1f0-bf26-406f-95ff-e29ee92b5a3a"), SimilarArtistId = Guid.Parse("4b6e5e08-93b2-4f37-8c80-f59c662153e4") }
            },
            CreatedAt = new DateTime(2024, 3, 23, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist24() => new Artist
        {
            Id = Guid.Parse("6a9fcd23-2c91-4a32-a3c2-f44e35f1a421"),
            Name = "Rihanna",
            ImageUrl = "https://example.com/images/rihanna.jpg",
            Bio = "Barbadian singer, actress, fashion designer, and businesswoman known for embracing various musical styles and reinventing her image throughout her career.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("6a9fcd23-2c91-4a32-a3c2-f44e35f1a421"), SimilarArtistId = Guid.Parse("4b6e5e08-93b2-4f37-8c80-f59c662153e4") },
                new ArtistLink { ArtistId = Guid.Parse("6a9fcd23-2c91-4a32-a3c2-f44e35f1a421"), SimilarArtistId = Guid.Parse("0f9d6cf2-497e-48ff-b1fa-440678951c28") }
            },
            CreatedAt = new DateTime(2024, 3, 24, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist25() => new Artist
        {
            Id = Guid.Parse("4b6e5e08-93b2-4f37-8c80-f59c662153e4"),
            Name = "Dua Lipa",
            ImageUrl = "https://example.com/images/dua-lipa.jpg",
            Bio = "English singer and songwriter known for her disco-pop style and strong vocals.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("4b6e5e08-93b2-4f37-8c80-f59c662153e4"), SimilarArtistId = Guid.Parse("0f9d6cf2-497e-48ff-b1fa-440678951c28") },
                new ArtistLink { ArtistId = Guid.Parse("4b6e5e08-93b2-4f37-8c80-f59c662153e4"), SimilarArtistId = Guid.Parse("e97b22b8-d9a1-4a5e-bfa1-6ba9cc0758cf") }
            },
            CreatedAt = new DateTime(2024, 3, 25, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist26() => new Artist
        {
            Id = Guid.Parse("0f9d6cf2-497e-48ff-b1fa-440678951c28"),
            Name = "Billie Eilish",
            ImageUrl = "https://example.com/images/billie-eilish.jpg",
            Bio = "American singer and songwriter known for her ethereal indie pop sounds and unique aesthetic.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("0f9d6cf2-497e-48ff-b1fa-440678951c28"), SimilarArtistId = Guid.Parse("e97b22b8-d9a1-4a5e-bfa1-6ba9cc0758cf") },
                new ArtistLink { ArtistId = Guid.Parse("0f9d6cf2-497e-48ff-b1fa-440678951c28"), SimilarArtistId = Guid.Parse("78322f23-3f6f-4420-ad26-3a014f5c5ea5") }
            },
            CreatedAt = new DateTime(2024, 3, 26, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist27() => new Artist
        {
            Id = Guid.Parse("e97b22b8-d9a1-4a5e-bfa1-6ba9cc0758cf"),
            Name = "Alicia Keys",
            ImageUrl = "https://example.com/images/alicia-keys.jpg",
            Bio = "American singer-songwriter and musician, known for her soulful voice and emotive piano compositions.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("e97b22b8-d9a1-4a5e-bfa1-6ba9cc0758cf"), SimilarArtistId = Guid.Parse("78322f23-3f6f-4420-ad26-3a014f5c5ea5") },
                new ArtistLink { ArtistId = Guid.Parse("e97b22b8-d9a1-4a5e-bfa1-6ba9cc0758cf"), SimilarArtistId = Guid.Parse("2a0c3b05-5f01-4b6b-84ea-b22f8bc301dc") }
            },
            CreatedAt = new DateTime(2024, 3, 27, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist28() => new Artist
        {
            Id = Guid.Parse("78322f23-3f6f-4420-ad26-3a014f5c5ea5"),
            Name = "Arctic Monkeys",
            ImageUrl = "https://example.com/images/arctic-monkeys.jpg",
            Bio = "British rock band known for their energetic rock sound and detailed lyrics.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("78322f23-3f6f-4420-ad26-3a014f5c5ea5"), SimilarArtistId = Guid.Parse("2a0c3b05-5f01-4b6b-84ea-b22f8bc301dc") },
                new ArtistLink { ArtistId = Guid.Parse("78322f23-3f6f-4420-ad26-3a014f5c5ea5"), SimilarArtistId = Guid.Parse("4d905d70-95e3-4e76-bf59-ace2d3ec05ad") }
            },
            CreatedAt = new DateTime(2024, 3, 28, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist29() => new Artist
        {
            Id = Guid.Parse("2a0c3b05-5f01-4b6b-84ea-b22f8bc301dc"),
            Name = "Tame Impala",
            ImageUrl = "https://example.com/images/tame-impala.jpg",
            Bio = "Australian musical project led by multi-instrumentalist Kevin Parker, known for their distinctive psychedelic sound.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("2a0c3b05-5f01-4b6b-84ea-b22f8bc301dc"), SimilarArtistId = Guid.Parse("4d905d70-95e3-4e76-bf59-ace2d3ec05ad") },
                new ArtistLink { ArtistId = Guid.Parse("2a0c3b05-5f01-4b6b-84ea-b22f8bc301dc"), SimilarArtistId = Guid.Parse("9cb82f47-a287-403a-a5c6-0f821d7d4c8a") }
            },
            CreatedAt = new DateTime(2024, 3, 29, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist30() => new Artist
        {
            Id = Guid.Parse("4d905d70-95e3-4e76-bf59-ace2d3ec05ad"),
            Name = "Grimes",
            ImageUrl = "https://example.com/images/grimes.jpg",
            Bio = "Canadian musician and visual artist known for her eclectic music style and high-pitched voice.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("4d905d70-95e3-4e76-bf59-ace2d3ec05ad"), SimilarArtistId = Guid.Parse("9cb82f47-a287-403a-a5c6-0f821d7d4c8a") },
                new ArtistLink { ArtistId = Guid.Parse("4d905d70-95e3-4e76-bf59-ace2d3ec05ad"), SimilarArtistId = Guid.Parse("0ac3e817-eda5-4a60-9e8c-6a82e2a3e5f3") }
            },
            CreatedAt = new DateTime(2024, 3, 30, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist31() => new Artist
        {
            Id = Guid.Parse("9cb82f47-a287-403a-a5c6-0f821d7d4c8a"),
            Name = "SZA",
            ImageUrl = "https://example.com/images/sza.jpg",
            Bio = "American singer-songwriter known for her blend of R&B, soul, and hip-hop and thoughtful, provocative lyrics.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("9cb82f47-a287-403a-a5c6-0f821d7d4c8a"), SimilarArtistId = Guid.Parse("0ac3e817-eda5-4a60-9e8c-6a82e2a3e5f3") },
                new ArtistLink { ArtistId = Guid.Parse("9cb82f47-a287-403a-a5c6-0f821d7d4c8a"), SimilarArtistId = Guid.Parse("cd1f9668-1eed-4a1c-9894-f7b3e3b9b486") }
            },
            CreatedAt = new DateTime(2024, 3, 31, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist32() => new Artist
        {
            Id = Guid.Parse("0ac3e817-eda5-4a60-9e8c-6a82e2a3e5f3"),
            Name = "The Lumineers",
            ImageUrl = "https://example.com/images/the-lumineers.jpg",
            Bio = "American folk-rock band known for their rustic sound and emotive lyrics.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("0ac3e817-eda5-4a60-9e8c-6a82e2a3e5f3"), SimilarArtistId = Guid.Parse("cd1f9668-1eed-4a1c-9894-f7b3e3b9b486") },
                new ArtistLink { ArtistId = Guid.Parse("0ac3e817-eda5-4a60-9e8c-6a82e2a3e5f3"), SimilarArtistId = Guid.Parse("f98851a4-dd7b-45df-83d2-77df1f53f678") }
            },
            CreatedAt = new DateTime(2024, 4, 1, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist33() => new Artist
        {
            Id = Guid.Parse("cd1f9668-1eed-4a1c-9894-f7b3e3b9b486"),
            Name = "Tyler, The Creator",
            ImageUrl = "https://example.com/images/tyler-the-creator.jpg",
            Bio = "American rapper and producer known for his innovative production and provocative lyrics.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("cd1f9668-1eed-4a1c-9894-f7b3e3b9b486"), SimilarArtistId = Guid.Parse("f98851a4-dd7b-45df-83d2-77df1f53f678") },
                new ArtistLink { ArtistId = Guid.Parse("cd1f9668-1eed-4a1c-9894-f7b3e3b9b486"), SimilarArtistId = Guid.Parse("2ba4fb8c-22d7-4ba6-9e95-1a1bfaa0fb4b") }
            },
            CreatedAt = new DateTime(2024, 4, 2, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist34() => new Artist
        {
            Id = Guid.Parse("f98851a4-dd7b-45df-83d2-77df1f53f678"),
            Name = "Norah Jones",
            ImageUrl = "https://example.com/images/norah-jones.jpg",
            Bio = "American singer, songwriter, and pianist known for her jazzy voice and ability to blend different genres including jazz and pop.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("f98851a4-dd7b-45df-83d2-77df1f53f678"), SimilarArtistId = Guid.Parse("2ba4fb8c-22d7-4ba6-9e95-1a1bfaa0fb4b") },
                new ArtistLink { ArtistId = Guid.Parse("f98851a4-dd7b-45df-83d2-77df1f53f678"), SimilarArtistId = Guid.Parse("bb90ec41-ce2d-4165-993b-c5ee2aabe8ce") }
            },
            CreatedAt = new DateTime(2024, 4, 3, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist35() => new Artist
        {
            Id = Guid.Parse("2ba4fb8c-22d7-4ba6-9e95-1a1bfaa0fb4b"),
            Name = "John Legend",
            ImageUrl = "https://example.com/images/john-legend.jpg",
            Bio = "American singer, songwriter, producer, and actor known for his smooth vocal quality and thoughtful musicianship.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("2ba4fb8c-22d7-4ba6-9e95-1a1bfaa0fb4b"), SimilarArtistId = Guid.Parse("bb90ec41-ce2d-4165-993b-c5ee2aabe8ce") },
                new ArtistLink { ArtistId = Guid.Parse("2ba4fb8c-22d7-4ba6-9e95-1a1bfaa0fb4b"), SimilarArtistId = Guid.Parse("e1d9cee7-ecde-4d07-bc64-3cec9c3f7b04") }
            },
            CreatedAt = new DateTime(2024, 4, 4, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist36() => new Artist
        {
            Id = Guid.Parse("bb90ec41-ce2d-4165-993b-c5ee2aabe8ce"),
            Name = "Kid Cudi",
            ImageUrl = "https://example.com/images/kid-cudi.jpg",
            Bio = "American rapper and actor known for his innovative sound that has influenced a generation of artists.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("bb90ec41-ce2d-4165-993b-c5ee2aabe8ce"), SimilarArtistId = Guid.Parse("e1d9cee7-ecde-4d07-bc64-3cec9c3f7b04") },
                new ArtistLink { ArtistId = Guid.Parse("bb90ec41-ce2d-4165-993b-c5ee2aabe8ce"), SimilarArtistId = Guid.Parse("a89d646a-230a-43ee-bd42-7988a4f80711") }
            },
            CreatedAt = new DateTime(2024, 4, 5, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist37() => new Artist
        {
            Id = Guid.Parse("e1d9cee7-ecde-4d07-bc64-3cec9c3f7b04"),
            Name = "Khalid",
            ImageUrl = "https://example.com/images/khalid.jpg",
            Bio = "American singer known for his rich, soulful voice and introspective themes.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("e1d9cee7-ecde-4d07-bc64-3cec9c3f7b04"), SimilarArtistId = Guid.Parse("a89d646a-230a-43ee-bd42-7988a4f80711") },
                new ArtistLink { ArtistId = Guid.Parse("e1d9cee7-ecde-4d07-bc64-3cec9c3f7b04"), SimilarArtistId = Guid.Parse("d2a3e8cf-b32d-4abe-9ae8-6ce7a89d2f64") }
            },
            CreatedAt = new DateTime(2024, 4, 6, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist38() => new Artist
        {
            Id = Guid.Parse("a89d646a-230a-43ee-bd42-7988a4f80711"),
            Name = "Sufjan Stevens",
            ImageUrl = "https://example.com/images/sufjan-stevens.jpg",
            Bio = "American singer-songwriter known for his lyrical and instrumental complexity and exploration of religious and spiritual themes.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("a89d646a-230a-43ee-bd42-7988a4f80711"), SimilarArtistId = Guid.Parse("d2a3e8cf-b32d-4abe-9ae8-6ce7a89d2f64") },
                new ArtistLink { ArtistId = Guid.Parse("a89d646a-230a-43ee-bd42-7988a4f80711"), SimilarArtistId = Guid.Parse("fe3b72a5-0aa0-49f3-8407-accf2b081227") }
            },
            CreatedAt = new DateTime(2024, 4, 7, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist39() => new Artist
        {
            Id = Guid.Parse("d2a3e8cf-b32d-4abe-9ae8-6ce7a89d2f64"),
            Name = "Hozier",
            ImageUrl = "https://example.com/images/hozier.jpg",
            Bio = "Irish singer and songwriter known for his poetic lyrics and soulful rock influences.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("d2a3e8cf-b32d-4abe-9ae8-6ce7a89d2f64"), SimilarArtistId = Guid.Parse("fe3b72a5-0aa0-49f3-8407-accf2b081227") },
                new ArtistLink { ArtistId = Guid.Parse("d2a3e8cf-b32d-4abe-9ae8-6ce7a89d2f64"), SimilarArtistId = Guid.Parse("1e00d247-6e57-413e-9ede-d4835f5cfbd1") }
            },
            CreatedAt = new DateTime(2024, 4, 8, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist40() => new Artist
        {
            Id = Guid.Parse("fe3b72a5-0aa0-49f3-8407-accf2b081227"),
            Name = "Maroon 5",
            ImageUrl = "https://example.com/images/maroon-5.jpg",
            Bio = "American pop rock band known for their catchy pop hits and dynamic stage presence.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("fe3b72a5-0aa0-49f3-8407-accf2b081227"), SimilarArtistId = Guid.Parse("1e00d247-6e57-413e-9ede-d4835f5cfbd1") },
                new ArtistLink { ArtistId = Guid.Parse("fe3b72a5-0aa0-49f3-8407-accf2b081227"), SimilarArtistId = Guid.Parse("f32c8b31-d086-4d22-95e3-efb8e6cebbd8") }
            },
            CreatedAt = new DateTime(2024, 4, 9, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist41() => new Artist
        {
            Id = Guid.Parse("1e00d247-6e57-413e-9ede-d4835f5cfbd1"),
            Name = "Lady Gaga",
            ImageUrl = "https://example.com/images/lady-gaga.jpg",
            Bio = "American singer, songwriter, and actress known for her unconventionality and provocative work as well as visual experimentation.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("1e00d247-6e57-413e-9ede-d4835f5cfbd1"), SimilarArtistId = Guid.Parse("f32c8b31-d086-4d22-95e3-efb8e6cebbd8") },
                new ArtistLink { ArtistId = Guid.Parse("1e00d247-6e57-413e-9ede-d4835f5cfbd1"), SimilarArtistId = Guid.Parse("6ab42fcb-33e8-4f13-ba58-c1ad5d98d543") }
            },
            CreatedAt = new DateTime(2024, 4, 10, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist42() => new Artist
        {
            Id = Guid.Parse("f32c8b31-d086-4d22-95e3-efb8e6cebbd8"),
            Name = "Shakira",
            ImageUrl = "https://example.com/images/shakira.jpg",
            Bio = "Colombian singer, songwriter, and dancer known for her blend of Latin music with mainstream pop and her energetic belly dancing.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("f32c8b31-d086-4d22-95e3-efb8e6cebbd8"), SimilarArtistId = Guid.Parse("6ab42fcb-33e8-4f13-ba58-c1ad5d98d543") },
                new ArtistLink { ArtistId = Guid.Parse("f32c8b31-d086-4d22-95e3-efb8e6cebbd8"), SimilarArtistId = Guid.Parse("99aa6f02-c3c4-44fa-aa2a-23f31c2f355b") }
            },
            CreatedAt = new DateTime(2024, 4, 11, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist43() => new Artist
        {
            Id = Guid.Parse("6ab42fcb-33e8-4f13-ba58-c1ad5d98d543"),
            Name = "Coldplay",
            ImageUrl = "https://example.com/images/coldplay.jpg",
            Bio = "British rock band known for their soulful melodies and strong lyrical themes of love and personal introspection.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("6ab42fcb-33e8-4f13-ba58-c1ad5d98d543"), SimilarArtistId = Guid.Parse("99aa6f02-c3c4-44fa-aa2a-23f31c2f355b") },
                new ArtistLink { ArtistId = Guid.Parse("6ab42fcb-33e8-4f13-ba58-c1ad5d98d543"), SimilarArtistId = Guid.Parse("f4f1479f-ccb7-4519-a636-fd3f1f8f921c") }
            },
            CreatedAt = new DateTime(2024, 4, 12, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist44() => new Artist
        {
            Id = Guid.Parse("99aa6f02-c3c4-44fa-aa2a-23f31c2f355b"),
            Name = "BTS",
            ImageUrl = "https://example.com/images/bts.jpg",
            Bio = "South Korean boy band sensation known for their global impact on pop music and dynamic choreography.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("99aa6f02-c3c4-44fa-aa2a-23f31c2f355b"), SimilarArtistId = Guid.Parse("f4f1479f-ccb7-4519-a636-fd3f1f8f921c") },
                new ArtistLink { ArtistId = Guid.Parse("99aa6f02-c3c4-44fa-aa2a-23f31c2f355b"), SimilarArtistId = Guid.Parse("d41d8cd9-245e-45ff-a5a7-d5d80a42f118") }
            },
            CreatedAt = new DateTime(2024, 4, 13, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist45() => new Artist
        {
            Id = Guid.Parse("f4f1479f-ccb7-4519-a636-fd3f1f8f921c"),
            Name = "Juanes",
            ImageUrl = "https://example.com/images/juanes.jpg",
            Bio = "Colombian rock musician known for his focus on universal themes and his advocacy for peace in Latin America.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("f4f1479f-ccb7-4519-a636-fd3f1f8f921c"), SimilarArtistId = Guid.Parse("d41d8cd9-245e-45ff-a5a7-d5d80a42f118") },
                new ArtistLink { ArtistId = Guid.Parse("f4f1479f-ccb7-4519-a636-fd3f1f8f921c"), SimilarArtistId = Guid.Parse("98087cf2-9ae8-46b8-9bba-4cd0f63c8c08") }
            },
            CreatedAt = new DateTime(2024, 4, 14, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetMockedArtist46() => new Artist
        {
            Id = Guid.Parse("d41d8cd9-245e-45ff-a5a7-d5d80a42f118"),
            Name = "Celine Dion",
            ImageUrl = "https://example.com/images/celine-dion.jpg",
            Bio = "Canadian singer known for her powerful vocals and emotional ballads, having achieved global fame.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("d41d8cd9-245e-45ff-a5a7-d5d80a42f118"), SimilarArtistId = Guid.Parse("98087cf2-9ae8-46b8-9bba-4cd0f63c8c08") },
                new ArtistLink { ArtistId = Guid.Parse("d41d8cd9-245e-45ff-a5a7-d5d80a42f118"), SimilarArtistId = Guid.Parse("da17e22b-8b95-4f85-83d3-57a1ec9613e7") }
            },
            CreatedAt = new DateTime(2024, 4, 15, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = new DateTime(2024, 4, 15, 12, 0, 0)
        };

        public static Artist GetMockedArtist47() => new Artist
        {
            Id = Guid.Parse("98087cf2-9ae8-46b8-9bba-4cd0f63c8c08"),
            Name = "Metallica",
            ImageUrl = "https://example.com/images/metallica.jpg",
            Bio = "American heavy metal band renowned for their fast tempos, aggressive musicianship, and deep lyrical content.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("98087cf2-9ae8-46b8-9bba-4cd0f63c8c08"), SimilarArtistId = Guid.Parse("da17e22b-8b95-4f85-83d3-57a1ec9613e7") },
                new ArtistLink { ArtistId = Guid.Parse("98087cf2-9ae8-46b8-9bba-4cd0f63c8c08"), SimilarArtistId = Guid.Parse("3c16dffc-25a0-45ac-804d-cfcfbe730f1b") }
            },
            CreatedAt = new DateTime(2024, 4, 16, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = new DateTime(2024, 4, 16, 12, 0, 0)
        };

        public static Artist GetMockedArtist48() => new Artist
        {
            Id = Guid.Parse("da17e22b-8b95-4f85-83d3-57a1ec9613e7"),
            Name = "Enrique Iglesias",
            ImageUrl = "https://example.com/images/enrique-iglesias.jpg",
            Bio = "Spanish singer, songwriter, and actor known as the King of Latin Pop and one of the best-selling Latin artists.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("da17e22b-8b95-4f85-83d3-57a1ec9613e7"), SimilarArtistId = Guid.Parse("3c16dffc-25a0-45ac-804d-cfcfbe730f1b") },
                new ArtistLink { ArtistId = Guid.Parse("da17e22b-8b95-4f85-83d3-57a1ec9613e7"), SimilarArtistId = Guid.Parse("4fa7fb3d-1aa8-4e91-ba7a-6f4781c01d41") }
            },
            CreatedAt = new DateTime(2024, 4, 17, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = new DateTime(2024, 4, 17, 12, 0, 0)
        };

        public static Artist GetMockedArtist49() => new Artist
        {
            Id = Guid.Parse("3c16dffc-25a0-45ac-804d-cfcfbe730f1b"),
            Name = "The Rolling Stones",
            ImageUrl = "https://example.com/images/the-rolling-stones.jpg",
            Bio = "English rock band known for their many hits and enduring influence on rock and roll since the 1960s.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("3c16dffc-25a0-45ac-804d-cfcfbe730f1b"), SimilarArtistId = Guid.Parse("4fa7fb3d-1aa8-4e91-ba7a-6f4781c01d41") },
                new ArtistLink { ArtistId = Guid.Parse("3c16dffc-25a0-45ac-804d-cfcfbe730f1b"), SimilarArtistId = Guid.Parse("b4e62a70-8b1d-4dd3-af8a-b44f6737fb22") }
            },
            CreatedAt = new DateTime(2024, 4, 18, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = new DateTime(2024, 4, 18, 12, 0, 0)
        };

        public static Artist GetMockedArtist50() => new Artist
        {
            Id = Guid.Parse("4fa7fb3d-1aa8-4e91-ba7a-6f4781c01d41"),
            Name = "Queen",
            ImageUrl = "https://example.com/images/queen.jpg",
            Bio = "British rock band famous for their style diversity, elaborate live performances, and classic hits like \"Bohemian Rhapsody\".",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("4fa7fb3d-1aa8-4e91-ba7a-6f4781c01d41"), SimilarArtistId = Guid.Parse("b4e62a70-8b1d-4dd3-af8a-b44f6737fb22") },
                new ArtistLink { ArtistId = Guid.Parse("4fa7fb3d-1aa8-4e91-ba7a-6f4781c01d41"), SimilarArtistId = Guid.Parse("4e75ecdd-aafe-4c35-836b-1b83fc7b8f88") }
            },
            CreatedAt = new DateTime(2024, 4, 19, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = new DateTime(2024, 4, 19, 12, 0, 0)
        };

        public static Artist GetMockedArtist51() => new Artist
        {
            Id = Guid.Parse("b4e62a70-8b1d-4dd3-af8a-b44f6737fb22"),
            Name = "U2",
            ImageUrl = "https://example.com/images/u2.jpg",
            Bio = "Irish rock band internationally acclaimed for their rock anthems and humanitarian efforts.",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink { ArtistId = Guid.Parse("b4e62a70-8b1d-4dd3-af8a-b44f6737fb22"), SimilarArtistId = Guid.Parse("4e75ecdd-aafe-4c35-836b-1b83fc7b8f88") },
                new ArtistLink { ArtistId = Guid.Parse("b4e62a70-8b1d-4dd3-af8a-b44f6737fb22"), SimilarArtistId = Guid.Parse("8c301aa9-6d56-4c06-b1f2-9b9956979345") }
            },
            CreatedAt = new DateTime(2024, 4, 20, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static ArtistDto GetMockedArtistDto1() => ToDto(GetMockedArtist1());

        public static ArtistDto GetMockedArtistDto2() => ToDto(GetMockedArtist2());

        public static ArtistDto GetMockedArtistDto3() => ToDto(GetMockedArtist3());

        public static ArtistDto GetMockedArtistDto4() => ToDto(GetMockedArtist4());

        public static ArtistDto GetMockedArtistDto5() => ToDto(GetMockedArtist5());

        public static ArtistDto GetMockedArtistDto6() => ToDto(GetMockedArtist6());

        public static ArtistDto GetMockedArtistDto7() => ToDto(GetMockedArtist7());

        public static ArtistDto GetMockedArtistDto8() => ToDto(GetMockedArtist8());

        public static ArtistDto GetMockedArtistDto9() => ToDto(GetMockedArtist9());

        public static ArtistDto GetMockedArtistDto10() => ToDto(GetMockedArtist10());

        public static ArtistDto GetMockedArtistDto11() => ToDto(GetMockedArtist11());

        public static ArtistDto GetMockedArtistDto12() => ToDto(GetMockedArtist12());

        public static ArtistDto GetMockedArtistDto13() => ToDto(GetMockedArtist13());

        public static ArtistDto GetMockedArtistDto14() => ToDto(GetMockedArtist14());

        public static ArtistDto GetMockedArtistDto15() => ToDto(GetMockedArtist15());

        public static ArtistDto GetMockedArtistDto16() => ToDto(GetMockedArtist16());

        public static ArtistDto GetMockedArtistDto17() => ToDto(GetMockedArtist17());

        public static ArtistDto GetMockedArtistDto18() => ToDto(GetMockedArtist18());

        public static ArtistDto GetMockedArtistDto19() => ToDto(GetMockedArtist19());

        public static ArtistDto GetMockedArtistDto20() => ToDto(GetMockedArtist20());

        public static ArtistDto GetMockedArtistDto21() => ToDto(GetMockedArtist21());

        public static ArtistDto GetMockedArtistDto22() => ToDto(GetMockedArtist22());

        public static ArtistDto GetMockedArtistDto23() => ToDto(GetMockedArtist23());

        public static ArtistDto GetMockedArtistDto24() => ToDto(GetMockedArtist24());

        public static ArtistDto GetMockedArtistDto25() => ToDto(GetMockedArtist25());

        public static ArtistDto GetMockedArtistDto26() => ToDto(GetMockedArtist26());

        public static ArtistDto GetMockedArtistDto27() => ToDto(GetMockedArtist27());

        public static ArtistDto GetMockedArtistDto28() => ToDto(GetMockedArtist28());

        public static ArtistDto GetMockedArtistDto29() => ToDto(GetMockedArtist29());

        public static ArtistDto GetMockedArtistDto30() => ToDto(GetMockedArtist30());

        public static ArtistDto GetMockedArtistDto31() => ToDto(GetMockedArtist31());

        public static ArtistDto GetMockedArtistDto32() => ToDto(GetMockedArtist32());

        public static ArtistDto GetMockedArtistDto33() => ToDto(GetMockedArtist33());

        public static ArtistDto GetMockedArtistDto34() => ToDto(GetMockedArtist34());

        public static ArtistDto GetMockedArtistDto35() => ToDto(GetMockedArtist35());

        public static ArtistDto GetMockedArtistDto36() => ToDto(GetMockedArtist36());

        public static ArtistDto GetMockedArtistDto37() => ToDto(GetMockedArtist37());

        public static ArtistDto GetMockedArtistDto38() => ToDto(GetMockedArtist38());

        public static ArtistDto GetMockedArtistDto39() => ToDto(GetMockedArtist39());

        public static ArtistDto GetMockedArtistDto40() => ToDto(GetMockedArtist40());

        public static ArtistDto GetMockedArtistDto41() => ToDto(GetMockedArtist41());

        public static ArtistDto GetMockedArtistDto42() => ToDto(GetMockedArtist42());

        public static ArtistDto GetMockedArtistDto43() => ToDto(GetMockedArtist43());

        public static ArtistDto GetMockedArtistDto44() => ToDto(GetMockedArtist44());

        public static ArtistDto GetMockedArtistDto45() => ToDto(GetMockedArtist45());

        public static ArtistDto GetMockedArtistDto46() => ToDto(GetMockedArtist46());

        public static ArtistDto GetMockedArtistDto47() => ToDto(GetMockedArtist47());

        public static ArtistDto GetMockedArtistDto48() => ToDto(GetMockedArtist48());

        public static ArtistDto GetMockedArtistDto49() => ToDto(GetMockedArtist49());

        public static ArtistDto GetMockedArtistDto50() => ToDto(GetMockedArtist50());

        public static ArtistDto GetMockedArtistDto51() => ToDto(GetMockedArtist51());

        private static ArtistDto ToDto(Artist artist) => new ArtistDto
        {
            Id = artist.Id,
            Name = artist.Name,
            ImageUrl = artist.ImageUrl,
            Bio = artist.Bio,
            SimilarArtistsIds = artist.SimilarArtists.Select(artistLink => artistLink.SimilarArtistId).ToList(),
            CreatedAt = artist.CreatedAt,
            UpdatedAt = artist.UpdatedAt,
            DeletedAt = artist.DeletedAt
        };
    }
}