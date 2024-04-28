using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Tests.Mocks
{
    public class AlbumMock
    {
        private AlbumMock() { }

        public static IList<Album> GetMockedAlbums() => new List<Album> { GetMockedAlbum1(), GetMockedAlbum2() };

        public static IList<AlbumDto> GetMockedAlbumDtos() => new List<AlbumDto> { GetMockedAlbumDto1(), GetMockedAlbumDto2() };

        public static IList<Album> GetMockedActiveAlbums() => GetMockedAlbums().Where(album => album.IsActive).ToList();

        public static IList<AlbumDto> GetMockedActiveAlbumDtos() => GetMockedAlbumDtos().Where(album => album.IsActive).ToList();

        public static Album GetMockedAlbum1() => new Album
        {
            Id = Guid.Parse("6ee76a77-2be4-42e3-8417-e60d282cffcb"),
            Title = "album_title1",
            ImageUrl = "https://album_imageurl1.jpg",
            ReleaseDate = new DateOnly(2020, 1, 1),
            SimilarAlbums = new List<AlbumLink>(),
            IsActive = true
        };

        public static Album GetMockedAlbum2() => new Album
        {
            Id = Guid.Parse("543c9236-443d-4526-b53b-b02f33f284ab"),
            Title = "album_title2",
            ImageUrl = "https://album_imageurl2.jpg",
            ReleaseDate = new DateOnly(2020, 1, 2),
            SimilarAlbums = new List<AlbumLink> { new AlbumLink { AlbumId = Guid.Parse("543c9236-443d-4526-b53b-b02f33f284ab"), SimilarAlbumId = Guid.Parse("6ee76a77-2be4-42e3-8417-e60d282cffcb") } },
            IsActive = false
        };

        public static Album GetMockedAlbum3() => new Album
        {
            Id = Guid.Parse("8a145bd2-7b7f-4188-bcdd-1c3a4a7c5e45"),
            Title = "album_title3",
            ImageUrl = "https://album_imageurl3.jpg",
            ReleaseDate = new DateOnly(2020, 1, 3),
            SimilarAlbums = new List<AlbumLink> { new AlbumLink { AlbumId = Guid.Parse("8a145bd2-7b7f-4188-bcdd-1c3a4a7c5e45"), SimilarAlbumId = Guid.Parse("543c9236-443d-4526-b53b-b02f33f284ab") } },
            IsActive = true
        };

        public static AlbumDto GetMockedAlbumDto1() => new AlbumDto
        {
            Id = Guid.Parse("6ee76a77-2be4-42e3-8417-e60d282cffcb"),
            Title = "album_title1",
            ImageUrl = "https://album_imageurl1.jpg",
            ReleaseDate = new DateOnly(2020, 1, 1),
            SimilarAlbumsIds = new List<Guid>(),
            IsActive = true
        };

        public static AlbumDto GetMockedAlbumDto2() => new AlbumDto
        {
            Id = Guid.Parse("543c9236-443d-4526-b53b-b02f33f284ab"),
            Title = "album_title2",
            ImageUrl = "https://album_imageurl2.jpg",
            ReleaseDate = new DateOnly(2020, 1, 2),
            SimilarAlbumsIds = new List<Guid> { Guid.Parse("6ee76a77-2be4-42e3-8417-e60d282cffcb") },
            IsActive = false
        };

        public static AlbumDto GetMockedAlbumDto3() => new AlbumDto
        {
            Id = Guid.Parse("8a145bd2-7b7f-4188-bcdd-1c3a4a7c5e45"),
            Title = "album_title3",
            ImageUrl = "https://album_imageurl3.jpg",
            ReleaseDate = new DateOnly(2020, 1, 3),
            SimilarAlbumsIds = new List<Guid> { Guid.Parse("543c9236-443d-4526-b53b-b02f33f284ab") },
            IsActive = true
        };
    }
}