using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;

namespace SoundSphere.Tests.Mocks
{
    public class ArtistMock
    {
        private ArtistMock() { }

        public static IList<Artist> GetMockedArtists() => new List<Artist> { GetMockedArtist1(), GetMockedArtist2() };

        public static IList<ArtistDto> GetMockedArtistDtos() => new List<ArtistDto> { GetMockedArtistDto1(), GetMockedArtistDto2() };

        public static Artist GetMockedArtist1() => new Artist
        {
            Id = Guid.Parse("d4f7f9d2-472e-488e-b7ef-73d169ba2bf5"),
            Name = "artist_name1",
            ImageUrl = "https://artist_imageurl1.jpg",
            Bio = "artist_bio1",
            SimilarArtists = new List<ArtistLink>(),
            IsActive = true,
        };

        public static Artist GetMockedArtist2() => new Artist
        {
            Id = Guid.Parse("0f9d6cf2-497e-48ff-b1fa-440678951c28"),
            Name = "artist_name2",
            ImageUrl = "https://artist_imageurl2.jpg",
            Bio = "artist_bio2",
            SimilarArtists = new List<ArtistLink>
            {
                new ArtistLink
                {
                    ArtistId = Guid.Parse("0f9d6cf2-497e-48ff-b1fa-440678951c28"),
                    SimilarArtistId = Guid.Parse("d4f7f9d2-472e-488e-b7ef-73d169ba2bf5")
                }
            },
            IsActive = false,
        };

        public static ArtistDto GetMockedArtistDto1() => new ArtistDto
        {
            Id = Guid.Parse("d4f7f9d2-472e-488e-b7ef-73d169ba2bf5"),
            Name = "artist_name1",
            ImageUrl = "https://artist_imageurl1.jpg",
            Bio = "artist_bio1",
            SimilarArtistsIds = new List<Guid>(),
            IsActive = true,
        };

        public static ArtistDto GetMockedArtistDto2() => new ArtistDto
        {
            Id = Guid.Parse("0f9d6cf2-497e-48ff-b1fa-440678951c28"),
            Name = "artist_name2",
            ImageUrl = "https://artist_imageurl2.jpg",
            Bio = "artist_bio2",
            SimilarArtistsIds = new List<Guid> { Guid.Parse("d4f7f9d2-472e-488e-b7ef-73d169ba2bf5") },
            IsActive = false,
        };
    }
}