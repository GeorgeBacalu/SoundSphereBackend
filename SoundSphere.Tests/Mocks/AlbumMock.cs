using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Dtos.Request.Models;
using SoundSphere.Database.Entities;

namespace SoundSphere.Tests.Mocks
{
    public class AlbumMock
    {
        private AlbumMock() { }

        public static IList<Album> GetMockedAlbums() => new List<Album>
        {
            GetMockedAlbum1(), GetMockedAlbum2(), GetMockedAlbum3(), GetMockedAlbum4(), GetMockedAlbum5(), GetMockedAlbum6(), GetMockedAlbum7(), GetMockedAlbum8(), GetMockedAlbum9(), GetMockedAlbum10(),
            GetMockedAlbum11(), GetMockedAlbum12(), GetMockedAlbum13(), GetMockedAlbum14(), GetMockedAlbum15(), GetMockedAlbum16(), GetMockedAlbum17(), GetMockedAlbum18(), GetMockedAlbum19(), GetMockedAlbum20(),
            GetMockedAlbum21(), GetMockedAlbum22(), GetMockedAlbum23(), GetMockedAlbum24(), GetMockedAlbum25(), GetMockedAlbum26(), GetMockedAlbum27(), GetMockedAlbum28(), GetMockedAlbum29(), GetMockedAlbum30(),
            GetMockedAlbum31(), GetMockedAlbum32(), GetMockedAlbum33(), GetMockedAlbum34(), GetMockedAlbum35(), GetMockedAlbum36(), GetMockedAlbum37(), GetMockedAlbum38(), GetMockedAlbum39(), GetMockedAlbum40(),
            GetMockedAlbum41(), GetMockedAlbum42(), GetMockedAlbum43(), GetMockedAlbum44(), GetMockedAlbum45(), GetMockedAlbum46(), GetMockedAlbum47(), GetMockedAlbum48(), GetMockedAlbum49(), GetMockedAlbum50()
        };

        public static IList<AlbumDto> GetMockedAlbumDtos() => GetMockedAlbums().Select(ToDto).ToList();

        public static IList<Album> GetMockedActiveAlbums() => GetMockedAlbums().Where(album => album.IsActive).ToList();

        public static IList<AlbumDto> GetMockedActiveAlbumDtos() => GetMockedAlbumDtos().Where(album => album.IsActive).ToList();

        public static IList<Album> GetMockedPaginatedAlbums() => new List<Album> { GetMockedAlbum45(), GetMockedAlbum46(), GetMockedAlbum47(), GetMockedAlbum48(), GetMockedAlbum49(), GetMockedAlbum50() };

        public static IList<AlbumDto> GetMockedPaginatedAlbumDtos() => new List<AlbumDto> { GetMockedAlbumDto45(), GetMockedAlbumDto46(), GetMockedAlbumDto47(), GetMockedAlbumDto48(), GetMockedAlbumDto49(), GetMockedAlbumDto50() };

        public static IList<Album> GetMockedActivePaginatedAlbums() => GetMockedPaginatedAlbums().Where(album => album.IsActive).ToList();

        public static IList<AlbumDto> GetMockedActivePaginatedAlbumDtos() => GetMockedPaginatedAlbumDtos().Where(album => album.IsActive).ToList();

        public static AlbumPaginationRequest GetMockedAlbumsPaginationRequest() => new AlbumPaginationRequest(
            SortCriteria: new Dictionary<AlbumSortCriterion, SortOrder> { { AlbumSortCriterion.ByTitle, SortOrder.Ascending }, { AlbumSortCriterion.ByReleaseDate, SortOrder.Ascending } },
            SearchCriteria: new List<AlbumSearchCriterion> { AlbumSearchCriterion.ByTitle, AlbumSearchCriterion.ByReleaseDateRange },
            Title: "A",
            DateRange: new DateRange(new DateOnly(1950, 1, 1), new DateOnly(2024, 5, 31)));

        public static Album GetMockedAlbum1() => new Album
        {
            Id = Guid.Parse("6ee76a77-2be4-42e3-8417-e60d282cffcb"),
            Title = "Utopia",
            ImageUrl = "https://example.com/utopia.jpg",
            ReleaseDate = new DateOnly(2022, 4, 25),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("6ee76a77-2be4-42e3-8417-e60d282cffcb"), SimilarAlbumId = Guid.Parse("b58f5f3f-d5e8-49f3-8b12-cfe33f762b4f") },
                new AlbumLink { AlbumId = Guid.Parse("6ee76a77-2be4-42e3-8417-e60d282cffcb"), SimilarAlbumId = Guid.Parse("05c4fe5d-8c0f-4eff-8ff2-e2e0e91218db") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum2() => new Album
        {
            Id = Guid.Parse("b58f5f3f-d5e8-49f3-8b12-cfe33f762b4f"),
            Title = "Echoes of Silence",
            ImageUrl = "https://example.com/echoes-of-silence.jpg",
            ReleaseDate = new DateOnly(2011, 12, 21),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("b58f5f3f-d5e8-49f3-8b12-cfe33f762b4f"), SimilarAlbumId = Guid.Parse("05c4fe5d-8c0f-4eff-8ff2-e2e0e91218db") },
                new AlbumLink { AlbumId = Guid.Parse("b58f5f3f-d5e8-49f3-8b12-cfe33f762b4f"), SimilarAlbumId = Guid.Parse("77953b24-e1c0-4d1f-a730-a12d5460b0d1") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum3() => new Album
        {
            Id = Guid.Parse("05c4fe5d-8c0f-4eff-8ff2-e2e0e91218db"),
            Title = "Born to Die",
            ImageUrl = "https://example.com/born-to-die.jpg",
            ReleaseDate = new DateOnly(2012, 1, 27),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("05c4fe5d-8c0f-4eff-8ff2-e2e0e91218db"), SimilarAlbumId = Guid.Parse("77953b24-e1c0-4d1f-a730-a12d5460b0d1") },
                new AlbumLink { AlbumId = Guid.Parse("05c4fe5d-8c0f-4eff-8ff2-e2e0e91218db"), SimilarAlbumId = Guid.Parse("e6d397e8-c355-4d68-b9e4-4a1c67daf6fa") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum4() => new Album
        {
            Id = Guid.Parse("77953b24-e1c0-4d1f-a730-a12d5460b0d1"),
            Title = "Random Access Memories",
            ImageUrl = "https://example.com/random-access-memories.jpg",
            ReleaseDate = new DateOnly(2013, 5, 17),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("77953b24-e1c0-4d1f-a730-a12d5460b0d1"), SimilarAlbumId = Guid.Parse("e6d397e8-c355-4d68-b9e4-4a1c67daf6fa") },
                new AlbumLink { AlbumId = Guid.Parse("77953b24-e1c0-4d1f-a730-a12d5460b0d1"), SimilarAlbumId = Guid.Parse("c0ec7e3d-8d8b-47b5-9b29-8a4b4c5357c1") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum5() => new Album
        {
            Id = Guid.Parse("e6d397e8-c355-4d68-b9e4-4a1c67daf6fa"),
            Title = "Good Kid, M.A.A.D City",
            ImageUrl = "https://example.com/good-kid-maad-city.jpg",
            ReleaseDate = new DateOnly(2012, 10, 22),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("e6d397e8-c355-4d68-b9e4-4a1c67daf6fa"), SimilarAlbumId = Guid.Parse("c0ec7e3d-8d8b-47b5-9b29-8a4b4c5357c1") },
                new AlbumLink { AlbumId = Guid.Parse("e6d397e8-c355-4d68-b9e4-4a1c67daf6fa"), SimilarAlbumId = Guid.Parse("8f70a802-6741-48de-9f2f-7f1d184b1687") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum6() => new Album
        {
            Id = Guid.Parse("c0ec7e3d-8d8b-47b5-9b29-8a4b4c5357c1"),
            Title = "Channel Orange",
            ImageUrl = "https://example.com/channel-orange.jpg",
            ReleaseDate = new DateOnly(2012, 7, 10),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("c0ec7e3d-8d8b-47b5-9b29-8a4b4c5357c1"), SimilarAlbumId = Guid.Parse("8f70a802-6741-48de-9f2f-7f1d184b1687") },
                new AlbumLink { AlbumId = Guid.Parse("c0ec7e3d-8d8b-47b5-9b29-8a4b4c5357c1"), SimilarAlbumId = Guid.Parse("bd2096d2-5b54-432a-9363-a99ed0078c7a") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum7() => new Album
        {
            Id = Guid.Parse("8f70a802-6741-48de-9f2f-7f1d184b1687"),
            Title = "In the Lonely Hour",
            ImageUrl = "https://example.com/in-the-lonely-hour.jpg",
            ReleaseDate = new DateOnly(2014, 5, 26),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("8f70a802-6741-48de-9f2f-7f1d184b1687"), SimilarAlbumId = Guid.Parse("bd2096d2-5b54-432a-9363-a99ed0078c7a") },
                new AlbumLink { AlbumId = Guid.Parse("8f70a802-6741-48de-9f2f-7f1d184b1687"), SimilarAlbumId = Guid.Parse("6542c145-0f0e-432f-8b8c-9a6a53c6b8e8") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum8() => new Album
        {
            Id = Guid.Parse("bd2096d2-5b54-432a-9363-a99ed0078c7a"),
            Title = "The Dark Side of the Moon",
            ImageUrl = "https://example.com/the-dark-side-of-the-moon.jpg",
            ReleaseDate = new DateOnly(1973, 3, 1),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("bd2096d2-5b54-432a-9363-a99ed0078c7a"), SimilarAlbumId = Guid.Parse("6542c145-0f0e-432f-8b8c-9a6a53c6b8e8") },
                new AlbumLink { AlbumId = Guid.Parse("bd2096d2-5b54-432a-9363-a99ed0078c7a"), SimilarAlbumId = Guid.Parse("c3e92b3d-f3b7-4935-baac-68b47054e897") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum9() => new Album
        {
            Id = Guid.Parse("6542c145-0f0e-432f-8b8c-9a6a53c6b8e8"),
            Title = "Thriller",
            ImageUrl = "https://example.com/thriller.jpg",
            ReleaseDate = new DateOnly(1982, 11, 30),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("6542c145-0f0e-432f-8b8c-9a6a53c6b8e8"), SimilarAlbumId = Guid.Parse("c3e92b3d-f3b7-4935-baac-68b47054e897") },
                new AlbumLink { AlbumId = Guid.Parse("6542c145-0f0e-432f-8b8c-9a6a53c6b8e8"), SimilarAlbumId = Guid.Parse("d4b7c7f3-8c6f-4f1a-91ff-ee2a850b2f3e") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum10() => new Album
        {
            Id = Guid.Parse("c3e92b3d-f3b7-4935-baac-68b47054e897"),
            Title = "Black Star",
            ImageUrl = "https://example.com/black-star.jpg",
            ReleaseDate = new DateOnly(2016, 1, 8),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("c3e92b3d-f3b7-4935-baac-68b47054e897"), SimilarAlbumId = Guid.Parse("d4b7c7f3-8c6f-4f1a-91ff-ee2a850b2f3e") },
                new AlbumLink { AlbumId = Guid.Parse("c3e92b3d-f3b7-4935-baac-68b47054e897"), SimilarAlbumId = Guid.Parse("f30c8c10-d54f-498c-8423-6e1d2dcaae4d") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum11() => new Album
        {
            Id = Guid.Parse("d4b7c7f3-8c6f-4f1a-91ff-ee2a850b2f3e"),
            Title = "Fine Line",
            ImageUrl = "https://example.com/fine-line.jpg",
            ReleaseDate = new DateOnly(2019, 12, 13),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("d4b7c7f3-8c6f-4f1a-91ff-ee2a850b2f3e"), SimilarAlbumId = Guid.Parse("f30c8c10-d54f-498c-8423-6e1d2dcaae4d") },
                new AlbumLink { AlbumId = Guid.Parse("d4b7c7f3-8c6f-4f1a-91ff-ee2a850b2f3e"), SimilarAlbumId = Guid.Parse("5fefa585-5a2f-4f4b-b6f5-850768f7c7b8") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum12() => new Album
        {
            Id = Guid.Parse("f30c8c10-d54f-498c-8423-6e1d2dcaae4d"),
            Title = "After Hours",
            ImageUrl = "https://example.com/after-hours.jpg",
            ReleaseDate = new DateOnly(2020, 3, 20),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("f30c8c10-d54f-498c-8423-6e1d2dcaae4d"), SimilarAlbumId = Guid.Parse("5fefa585-5a2f-4f4b-b6f5-850768f7c7b8") },
                new AlbumLink { AlbumId = Guid.Parse("f30c8c10-d54f-498c-8423-6e1d2dcaae4d"), SimilarAlbumId = Guid.Parse("9f1c4000-6f48-4bda-aa6d-22f0581629d1") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum13() => new Album
        {
            Id = Guid.Parse("5fefa585-5a2f-4f4b-b6f5-850768f7c7b8"),
            Title = "Evermore",
            ImageUrl = "https://example.com/evermore.jpg",
            ReleaseDate = new DateOnly(2020, 3, 20),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("5fefa585-5a2f-4f4b-b6f5-850768f7c7b8"), SimilarAlbumId = Guid.Parse("9f1c4000-6f48-4bda-aa6d-22f0581629d1") },
                new AlbumLink { AlbumId = Guid.Parse("5fefa585-5a2f-4f4b-b6f5-850768f7c7b8"), SimilarAlbumId = Guid.Parse("ae93288c-a804-4f40-a97c-6f3572da3b01") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum14() => new Album
        {
            Id = Guid.Parse("9f1c4000-6f48-4bda-aa6d-22f0581629d1"),
            Title = "A Moon Shaped Pool",
            ImageUrl = "https://example.com/a-moon-shaped-pool.jpg",
            ReleaseDate = new DateOnly(2016, 5, 8),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("9f1c4000-6f48-4bda-aa6d-22f0581629d1"), SimilarAlbumId = Guid.Parse("ae93288c-a804-4f40-a97c-6f3572da3b01") },
                new AlbumLink { AlbumId = Guid.Parse("9f1c4000-6f48-4bda-aa6d-22f0581629d1"), SimilarAlbumId = Guid.Parse("3e5b76a4-c2bf-4c7d-9abf-a59b3e8e0bc2") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum15() => new Album
        {
            Id = Guid.Parse("ae93288c-a804-4f40-a97c-6f3572da3b01"),
            Title = "DAMN",
            ImageUrl = "https://example.com/damn.jpg",
            ReleaseDate = new DateOnly(2017, 4, 14),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("ae93288c-a804-4f40-a97c-6f3572da3b01"), SimilarAlbumId = Guid.Parse("3e5b76a4-c2bf-4c7d-9abf-a59b3e8e0bc2") },
                new AlbumLink { AlbumId = Guid.Parse("ae93288c-a804-4f40-a97c-6f3572da3b01"), SimilarAlbumId = Guid.Parse("9d4a518b-6673-4a91-b179-2b8c68a4f3f3") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum16() => new Album
        {
            Id = Guid.Parse("3e5b76a4-c2bf-4c7d-9abf-a59b3e8e0bc2"),
            Title = "Lemonade",
            ImageUrl = "https://example.com/lemonade.jpg",
            ReleaseDate = new DateOnly(2016, 4, 23),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("3e5b76a4-c2bf-4c7d-9abf-a59b3e8e0bc2"), SimilarAlbumId = Guid.Parse("9d4a518b-6673-4a91-b179-2b8c68a4f3f3") },
                new AlbumLink { AlbumId = Guid.Parse("3e5b76a4-c2bf-4c7d-9abf-a59b3e8e0bc2"), SimilarAlbumId = Guid.Parse("c5f5b360-fbfd-4e17-aca3-e2a1d5b25895") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum17() => new Album
        {
            Id = Guid.Parse("9d4a518b-6673-4a91-b179-2b8c68a4f3f3"),
            Title = "21",
            ImageUrl = "https://example.com/21-adele.jpg",
            ReleaseDate = new DateOnly(2011, 1, 24),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("9d4a518b-6673-4a91-b179-2b8c68a4f3f3"), SimilarAlbumId = Guid.Parse("c5f5b360-fbfd-4e17-aca3-e2a1d5b25895") },
                new AlbumLink { AlbumId = Guid.Parse("9d4a518b-6673-4a91-b179-2b8c68a4f3f3"), SimilarAlbumId = Guid.Parse("57d9b8a7-d36b-4975-bd4a-a4c5dbe5c5cc") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum18() => new Album
        {
            Id = Guid.Parse("c5f5b360-fbfd-4e17-aca3-e2a1d5b25895"),
            Title = "Voyage",
            ImageUrl = "https://example.com/voyage.jpg",
            ReleaseDate = new DateOnly(2021, 11, 5),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("c5f5b360-fbfd-4e17-aca3-e2a1d5b25895"), SimilarAlbumId = Guid.Parse("57d9b8a7-d36b-4975-bd4a-a4c5dbe5c5cc") },
                new AlbumLink { AlbumId = Guid.Parse("c5f5b360-fbfd-4e17-aca3-e2a1d5b25895"), SimilarAlbumId = Guid.Parse("2d68f441-d532-4e0d-8f51-c5d086078dd9") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum19() => new Album
        {
            Id = Guid.Parse("57d9b8a7-d36b-4975-bd4a-a4c5dbe5c5cc"),
            Title = "Starboy",
            ImageUrl = "https://example.com/starboy.jpg",
            ReleaseDate = new DateOnly(2016, 11, 25),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("57d9b8a7-d36b-4975-bd4a-a4c5dbe5c5cc"), SimilarAlbumId = Guid.Parse("2d68f441-d532-4e0d-8f51-c5d086078dd9") },
                new AlbumLink { AlbumId = Guid.Parse("57d9b8a7-d36b-4975-bd4a-a4c5dbe5c5cc"), SimilarAlbumId = Guid.Parse("1b6d3a7a-d9d4-478e-b0c1-8d8c2a4f6c0f") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum20() => new Album
        {
            Id = Guid.Parse("2d68f441-d532-4e0d-8f51-c5d086078dd9"),
            Title = "Divide",
            ImageUrl = "https://example.com/divide.jpg",
            ReleaseDate = new DateOnly(2017, 3, 3),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("2d68f441-d532-4e0d-8f51-c5d086078dd9"), SimilarAlbumId = Guid.Parse("1b6d3a7a-d9d4-478e-b0c1-8d8c2a4f6c0f") },
                new AlbumLink { AlbumId = Guid.Parse("2d68f441-d532-4e0d-8f51-c5d086078dd9"), SimilarAlbumId = Guid.Parse("2e2f662e-c2df-4660-800c-3613b6001bf1") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum21() => new Album
        {
            Id = Guid.Parse("1b6d3a7a-d9d4-478e-b0c1-8d8c2a4f6c0f"),
            Title = "Vibras",
            ImageUrl = "https://example.com/vibras.jpg",
            ReleaseDate = new DateOnly(2018, 5, 25),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("1b6d3a7a-d9d4-478e-b0c1-8d8c2a4f6c0f"), SimilarAlbumId = Guid.Parse("2e2f662e-c2df-4660-800c-3613b6001bf1") },
                new AlbumLink { AlbumId = Guid.Parse("1b6d3a7a-d9d4-478e-b0c1-8d8c2a4f6c0f"), SimilarAlbumId = Guid.Parse("cc3cf857-e7f5-4c0b-9eaa-3f6e4c8d2045") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum22() => new Album
        {
            Id = Guid.Parse("2e2f662e-c2df-4660-800c-3613b6001bf1"),
            Title = "Astroworld",
            ImageUrl = "https://example.com/astroworld.jpg",
            ReleaseDate = new DateOnly(2018, 8, 3),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("2e2f662e-c2df-4660-800c-3613b6001bf1"), SimilarAlbumId = Guid.Parse("cc3cf857-e7f5-4c0b-9eaa-3f6e4c8d2045") },
                new AlbumLink { AlbumId = Guid.Parse("2e2f662e-c2df-4660-800c-3613b6001bf1"), SimilarAlbumId = Guid.Parse("995a2c8e-cdd5-4c0f-b875-8b6192b6b014") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum23() => new Album
        {
            Id = Guid.Parse("cc3cf857-e7f5-4c0b-9eaa-3f6e4c8d2045"),
            Title = "Hollywoods Bleeding",
            ImageUrl = "https://example.com/hollywoods-bleeding.jpg",
            ReleaseDate = new DateOnly(2019, 9, 6),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("cc3cf857-e7f5-4c0b-9eaa-3f6e4c8d2045"), SimilarAlbumId = Guid.Parse("995a2c8e-cdd5-4c0f-b875-8b6192b6b014") },
                new AlbumLink { AlbumId = Guid.Parse("cc3cf857-e7f5-4c0b-9eaa-3f6e4c8d2045"), SimilarAlbumId = Guid.Parse("fb9eac8d-0aec-4cc2-b5c6-f9eca5c6d408") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum24() => new Album
        {
            Id = Guid.Parse("995a2c8e-cdd5-4c0f-b875-8b6192b6b014"),
            Title = "Blonde",
            ImageUrl = "https://example.com/blonde.jpg",
            ReleaseDate = new DateOnly(2016, 8, 20),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("995a2c8e-cdd5-4c0f-b875-8b6192b6b014"), SimilarAlbumId = Guid.Parse("fb9eac8d-0aec-4cc2-b5c6-f9eca5c6d408") },
                new AlbumLink { AlbumId = Guid.Parse("995a2c8e-cdd5-4c0f-b875-8b6192b6b014"), SimilarAlbumId = Guid.Parse("0fd3aee7-0bd0-4482-a78d-17e3c7d8b4d5") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum25() => new Album
        {
            Id = Guid.Parse("fb9eac8d-0aec-4cc2-b5c6-f9eca5c6d408"),
            Title = "My Beautiful Dark Twisted Fantasy",
            ImageUrl = "https://example.com/my-beautiful-dark-twisted-fantasy.jpg",
            ReleaseDate = new DateOnly(2010, 11, 22),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("fb9eac8d-0aec-4cc2-b5c6-f9eca5c6d408"), SimilarAlbumId = Guid.Parse("0fd3aee7-0bd0-4482-a78d-17e3c7d8b4d5") },
                new AlbumLink { AlbumId = Guid.Parse("fb9eac8d-0aec-4cc2-b5c6-f9eca5c6d408"), SimilarAlbumId = Guid.Parse("41ad644e-bdec-472c-ae5d-9f2d1e7fcb0d") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum26() => new Album
        {
            Id = Guid.Parse("0fd3aee7-0bd0-4482-a78d-17e3c7d8b4d5"),
            Title = "Melodrama",
            ImageUrl = "https://example.com/melodrama.jpg",
            ReleaseDate = new DateOnly(2017, 6, 16),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("0fd3aee7-0bd0-4482-a78d-17e3c7d8b4d5"), SimilarAlbumId = Guid.Parse("41ad644e-bdec-472c-ae5d-9f2d1e7fcb0d") },
                new AlbumLink { AlbumId = Guid.Parse("0fd3aee7-0bd0-4482-a78d-17e3c7d8b4d5"), SimilarAlbumId = Guid.Parse("c7e1f8b6-d8df-4eca-b45e-736b5d63b4d5") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum27() => new Album
        {
            Id = Guid.Parse("41ad644e-bdec-472c-ae5d-9f2d1e7fcb0d"),
            Title = "Nothing Was the Same",
            ImageUrl = "https://example.com/nothing-was-the-same.jpg",
            ReleaseDate = new DateOnly(2013, 9, 24),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("41ad644e-bdec-472c-ae5d-9f2d1e7fcb0d"), SimilarAlbumId = Guid.Parse("c7e1f8b6-d8df-4eca-b45e-736b5d63b4d5") },
                new AlbumLink { AlbumId = Guid.Parse("41ad644e-bdec-472c-ae5d-9f2d1e7fcb0d"), SimilarAlbumId = Guid.Parse("aac3f3d7-3146-421f-981d-68f02d1352f0") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum28() => new Album
        {
            Id = Guid.Parse("c7e1f8b6-d8df-4eca-b45e-736b5d63b4d5"),
            Title = "ANTI",
            ImageUrl = "https://example.com/anti.jpg",
            ReleaseDate = new DateOnly(2016, 1, 28),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("c7e1f8b6-d8df-4eca-b45e-736b5d63b4d5"), SimilarAlbumId = Guid.Parse("aac3f3d7-3146-421f-981d-68f02d1352f0") },
                new AlbumLink { AlbumId = Guid.Parse("c7e1f8b6-d8df-4eca-b45e-736b5d63b4d5"), SimilarAlbumId = Guid.Parse("a393b54c-01fb-4fd3-a8bb-3395bf603934") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum29() => new Album
        {
            Id = Guid.Parse("aac3f3d7-3146-421f-981d-68f02d1352f0"),
            Title = "Future Nostalgia",
            ImageUrl = "https://example.com/future-nostalgia.jpg",
            ReleaseDate = new DateOnly(2020, 3, 27),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("aac3f3d7-3146-421f-981d-68f02d1352f0"), SimilarAlbumId = Guid.Parse("a393b54c-01fb-4fd3-a8bb-3395bf603934") },
                new AlbumLink { AlbumId = Guid.Parse("aac3f3d7-3146-421f-981d-68f02d1352f0"), SimilarAlbumId = Guid.Parse("77f3f764-0a2e-4be2-a753-5555af2c4d52") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum30() => new Album
        {
            Id = Guid.Parse("a393b54c-01fb-4fd3-a8bb-3395bf603934"),
            Title = "When We All Fall Asleep, Where Do We Go?",
            ImageUrl = "https://example.com/when-we-all-fall-asleep.jpg",
            ReleaseDate = new DateOnly(2019, 3, 29),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("a393b54c-01fb-4fd3-a8bb-3395bf603934"), SimilarAlbumId = Guid.Parse("77f3f764-0a2e-4be2-a753-5555af2c4d52") },
                new AlbumLink { AlbumId = Guid.Parse("a393b54c-01fb-4fd3-a8bb-3395bf603934"), SimilarAlbumId = Guid.Parse("71b4bdec-c4a4-413b-b874-e3fc2b8a1235") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum31() => new Album
        {
            Id = Guid.Parse("77f3f764-0a2e-4be2-a753-5555af2c4d52"),
            Title = "The Life of Pablo",
            ImageUrl = "https://example.com/the-life-of-pablo.jpg",
            ReleaseDate = new DateOnly(2016, 2, 14),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("77f3f764-0a2e-4be2-a753-5555af2c4d52"), SimilarAlbumId = Guid.Parse("71b4bdec-c4a4-413b-b874-e3fc2b8a1235") },
                new AlbumLink { AlbumId = Guid.Parse("77f3f764-0a2e-4be2-a753-5555af2c4d52"), SimilarAlbumId = Guid.Parse("39fb8791-3b22-4aad-99d1-27b51a5a3e37") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum32() => new Album
        {
            Id = Guid.Parse("71b4bdec-c4a4-413b-b874-e3fc2b8a1235"),
            Title = "To Pimp a Butterfly",
            ImageUrl = "https://example.com/to-pimp-a-butterfly.jpg",
            ReleaseDate = new DateOnly(2015, 3, 15),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("71b4bdec-c4a4-413b-b874-e3fc2b8a1235"), SimilarAlbumId = Guid.Parse("39fb8791-3b22-4aad-99d1-27b51a5a3e37") },
                new AlbumLink { AlbumId = Guid.Parse("71b4bdec-c4a4-413b-b874-e3fc2b8a1235"), SimilarAlbumId = Guid.Parse("7cd3c9a2-bc21-4797-b488-67e845a0e79b") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum33() => new Album
        {
            Id = Guid.Parse("39fb8791-3b22-4aad-99d1-27b51a5a3e37"),
            Title = "Tranquility Base Hotel & Casino",
            ImageUrl = "https://example.com/tranquility-base.jpg",
            ReleaseDate = new DateOnly(2018, 5, 11),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("39fb8791-3b22-4aad-99d1-27b51a5a3e37"), SimilarAlbumId = Guid.Parse("7cd3c9a2-bc21-4797-b488-67e845a0e79b") },
                new AlbumLink { AlbumId = Guid.Parse("39fb8791-3b22-4aad-99d1-27b51a5a3e37"), SimilarAlbumId = Guid.Parse("ade35692-6d10-4c2f-8c3b-ff7b638d06e7") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum34() => new Album
        {
            Id = Guid.Parse("7cd3c9a2-bc21-4797-b488-67e845a0e79b"),
            Title = "Currents",
            ImageUrl = "https://example.com/currents.jpg",
            ReleaseDate = new DateOnly(2015, 7, 17),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("7cd3c9a2-bc21-4797-b488-67e845a0e79b"), SimilarAlbumId = Guid.Parse("ade35692-6d10-4c2f-8c3b-ff7b638d06e7") },
                new AlbumLink { AlbumId = Guid.Parse("7cd3c9a2-bc21-4797-b488-67e845a0e79b"), SimilarAlbumId = Guid.Parse("89b992ff-29be-49d3-b786-8d2e8d44de55") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum35() => new Album
        {
            Id = Guid.Parse("ade35692-6d10-4c2f-8c3b-ff7b638d06e7"),
            Title = "Art Angels",
            ImageUrl = "https://example.com/art-angels.jpg",
            ReleaseDate = new DateOnly(2015, 11, 6),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("ade35692-6d10-4c2f-8c3b-ff7b638d06e7"), SimilarAlbumId = Guid.Parse("89b992ff-29be-49d3-b786-8d2e8d44de55") },
                new AlbumLink { AlbumId = Guid.Parse("ade35692-6d10-4c2f-8c3b-ff7b638d06e7"), SimilarAlbumId = Guid.Parse("ec2c9fe3-e8a2-4ec6-a7b5-885e10dd87ec") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum36() => new Album
        {
            Id = Guid.Parse("89b992ff-29be-49d3-b786-8d2e8d44de55"),
            Title = "Yeezus",
            ImageUrl = "https://example.com/yeezus.jpg",
            ReleaseDate = new DateOnly(2013, 6, 18),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("89b992ff-29be-49d3-b786-8d2e8d44de55"), SimilarAlbumId = Guid.Parse("ec2c9fe3-e8a2-4ec6-a7b5-885e10dd87ec") },
                new AlbumLink { AlbumId = Guid.Parse("89b992ff-29be-49d3-b786-8d2e8d44de55"), SimilarAlbumId = Guid.Parse("3ef8d9f9-88f8-4f39-b73d-261b5a7f2c8a") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum37() => new Album
        {
            Id = Guid.Parse("ec2c9fe3-e8a2-4ec6-a7b5-885e10dd87ec"),
            Title = "Ctrl",
            ImageUrl = "https://example.com/ctrl.jpg",
            ReleaseDate = new DateOnly(2017, 6, 9),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("ec2c9fe3-e8a2-4ec6-a7b5-885e10dd87ec"), SimilarAlbumId = Guid.Parse("3ef8d9f9-88f8-4f39-b73d-261b5a7f2c8a") },
                new AlbumLink { AlbumId = Guid.Parse("ec2c9fe3-e8a2-4ec6-a7b5-885e10dd87ec"), SimilarAlbumId = Guid.Parse("07fa60f8-8b91-4c33-bf53-1e4f3b79f597") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum38() => new Album
        {
            Id = Guid.Parse("3ef8d9f9-88f8-4f39-b73d-261b5a7f2c8a"),
            Title = "Cleopatra",
            ImageUrl = "https://example.com/cleopatra.jpg",
            ReleaseDate = new DateOnly(2016, 4, 8),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("3ef8d9f9-88f8-4f39-b73d-261b5a7f2c8a"), SimilarAlbumId = Guid.Parse("07fa60f8-8b91-4c33-bf53-1e4f3b79f597") },
                new AlbumLink { AlbumId = Guid.Parse("3ef8d9f9-88f8-4f39-b73d-261b5a7f2c8a"), SimilarAlbumId = Guid.Parse("d6a47f2d-bb31-4f1c-a9df-6b557b2f6b26") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum39() => new Album
        {
            Id = Guid.Parse("07fa60f8-8b91-4c33-bf53-1e4f3b79f597"),
            Title = "Flower Boy",
            ImageUrl = "https://example.com/flower-boy.jpg",
            ReleaseDate = new DateOnly(2017, 7, 21),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("07fa60f8-8b91-4c33-bf53-1e4f3b79f597"), SimilarAlbumId = Guid.Parse("d6a47f2d-bb31-4f1c-a9df-6b557b2f6b26") },
                new AlbumLink { AlbumId = Guid.Parse("07fa60f8-8b91-4c33-bf53-1e4f3b79f597"), SimilarAlbumId = Guid.Parse("b2d7f920-68b2-4ba4-a99e-abf3069f887c") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum40() => new Album
        {
            Id = Guid.Parse("d6a47f2d-bb31-4f1c-a9df-6b557b2f6b26"),
            Title = "In Rainbows",
            ImageUrl = "https://example.com/in-rainbows.jpg",
            ReleaseDate = new DateOnly(2007, 10, 10),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("d6a47f2d-bb31-4f1c-a9df-6b557b2f6b26"), SimilarAlbumId = Guid.Parse("b2d7f920-68b2-4ba4-a99e-abf3069f887c") },
                new AlbumLink { AlbumId = Guid.Parse("d6a47f2d-bb31-4f1c-a9df-6b557b2f6b26"), SimilarAlbumId = Guid.Parse("1c3a213b-aa9b-4c8b-a393-4c881522eebb") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum41() => new Album
        {
            Id = Guid.Parse("b2d7f920-68b2-4ba4-a99e-abf3069f887c"),
            Title = "Stoney",
            ImageUrl = "https://example.com/stoney.jpg",
            ReleaseDate = new DateOnly(2016, 12, 9),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("b2d7f920-68b2-4ba4-a99e-abf3069f887c"), SimilarAlbumId = Guid.Parse("1c3a213b-aa9b-4c8b-a393-4c881522eebb") },
                new AlbumLink { AlbumId = Guid.Parse("b2d7f920-68b2-4ba4-a99e-abf3069f887c"), SimilarAlbumId = Guid.Parse("a8a214b9-c3d1-420e-9e63-3418c9367e81") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum42() => new Album
        {
            Id = Guid.Parse("1c3a213b-aa9b-4c8b-a393-4c881522eebb"),
            Title = "Kid A",
            ImageUrl = "https://example.com/kid-a.jpg",
            ReleaseDate = new DateOnly(2000, 10, 2),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("1c3a213b-aa9b-4c8b-a393-4c881522eebb"), SimilarAlbumId = Guid.Parse("a8a214b9-c3d1-420e-9e63-3418c9367e81") },
                new AlbumLink { AlbumId = Guid.Parse("1c3a213b-aa9b-4c8b-a393-4c881522eebb"), SimilarAlbumId = Guid.Parse("8f5296a1-0df2-49d5-9b69-6c6afe6c8e18") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum43() => new Album
        {
            Id = Guid.Parse("a8a214b9-c3d1-420e-9e63-3418c9367e81"),
            Title = "Ghost Stories",
            ImageUrl = "https://example.com/ghost-stories.jpg",
            ReleaseDate = new DateOnly(2014, 5, 16),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("a8a214b9-c3d1-420e-9e63-3418c9367e81"), SimilarAlbumId = Guid.Parse("8f5296a1-0df2-49d5-9b69-6c6afe6c8e18") },
                new AlbumLink { AlbumId = Guid.Parse("a8a214b9-c3d1-420e-9e63-3418c9367e81"), SimilarAlbumId = Guid.Parse("5df3c97b-d624-4d55-8a33-4e4d49abdeba") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum44() => new Album
        {
            Id = Guid.Parse("8f5296a1-0df2-49d5-9b69-6c6afe6c8e18"),
            Title = "Awaken, My Love!",
            ImageUrl = "https://example.com/awaken-my-love.jpg",
            ReleaseDate = new DateOnly(2016, 12, 2),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("8f5296a1-0df2-49d5-9b69-6c6afe6c8e18"), SimilarAlbumId = Guid.Parse("5df3c97b-d624-4d55-8a33-4e4d49abdeba") },
                new AlbumLink { AlbumId = Guid.Parse("8f5296a1-0df2-49d5-9b69-6c6afe6c8e18"), SimilarAlbumId = Guid.Parse("2fd6f336-fc89-45e8-baae-2c8d3ec384cb") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum45() => new Album
        {
            Id = Guid.Parse("5df3c97b-d624-4d55-8a33-4e4d49abdeba"),
            Title = "4 Your Eyes Only",
            ImageUrl = "https://example.com/4-your-eyes-only.jpg",
            ReleaseDate = new DateOnly(2016, 12, 9),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("5df3c97b-d624-4d55-8a33-4e4d49abdeba"), SimilarAlbumId = Guid.Parse("2fd6f336-fc89-45e8-baae-2c8d3ec384cb") },
                new AlbumLink { AlbumId = Guid.Parse("5df3c97b-d624-4d55-8a33-4e4d49abdeba"), SimilarAlbumId = Guid.Parse("71d6e3ac-413b-46f1-9aaf-c36d4056b7c2") }
            },
            IsActive = true
        };

        public static Album GetMockedAlbum46() => new Album
        {
            Id = Guid.Parse("2fd6f336-fc89-45e8-baae-2c8d3ec384cb"),
            Title = "Man on the Moon",
            ImageUrl = "https://example.com/man-on-the-moon.jpg",
            ReleaseDate = new DateOnly(2009, 9, 15),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("2fd6f336-fc89-45e8-baae-2c8d3ec384cb"), SimilarAlbumId = Guid.Parse("71d6e3ac-413b-46f1-9aaf-c36d4056b7c2") },
                new AlbumLink { AlbumId = Guid.Parse("2fd6f336-fc89-45e8-baae-2c8d3ec384cb"), SimilarAlbumId = Guid.Parse("67a03545-13d2-45b6-bf01-668c9ee315bb") }
            },
            IsActive = false
        };

        public static Album GetMockedAlbum47() => new Album
        {
            Id = Guid.Parse("71d6e3ac-413b-46f1-9aaf-c36d4056b7c2"),
            Title = "American Teen",
            ImageUrl = "https://example.com/american-teen.jpg",
            ReleaseDate = new DateOnly(2017, 3, 3),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("71d6e3ac-413b-46f1-9aaf-c36d4056b7c2"), SimilarAlbumId = Guid.Parse("67a03545-13d2-45b6-bf01-668c9ee315bb") },
                new AlbumLink { AlbumId = Guid.Parse("71d6e3ac-413b-46f1-9aaf-c36d4056b7c2"), SimilarAlbumId = Guid.Parse("b8107cbb-18fb-49b0-876b-86777a3bafff") }
            },
            IsActive = false
        };

        public static Album GetMockedAlbum48() => new Album
        {
            Id = Guid.Parse("67a03545-13d2-45b6-bf01-668c9ee315bb"),
            Title = "Carrie & Lowell",
            ImageUrl = "https://example.com/carrie-and-lowell.jpg",
            ReleaseDate = new DateOnly(2015, 3, 31),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("67a03545-13d2-45b6-bf01-668c9ee315bb"), SimilarAlbumId = Guid.Parse("b8107cbb-18fb-49b0-876b-86777a3bafff") },
                new AlbumLink { AlbumId = Guid.Parse("67a03545-13d2-45b6-bf01-668c9ee315bb"), SimilarAlbumId = Guid.Parse("11d77e95-f4f2-4a6f-980b-674d81d8c1d8") }
            },
            IsActive = false
        };

        public static Album GetMockedAlbum49() => new Album
        {
            Id = Guid.Parse("b8107cbb-18fb-49b0-876b-86777a3bafff"),
            Title = "Beauty Behind the Madness",
            ImageUrl = "https://example.com/beauty-behind-the-madness.jpg",
            ReleaseDate = new DateOnly(2015, 8, 28),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("b8107cbb-18fb-49b0-876b-86777a3bafff"), SimilarAlbumId = Guid.Parse("11d77e95-f4f2-4a6f-980b-674d81d8c1d8") },
                new AlbumLink { AlbumId = Guid.Parse("b8107cbb-18fb-49b0-876b-86777a3bafff"), SimilarAlbumId = Guid.Parse("8a145bd2-7b7f-4188-bcdd-1c3a4a7c5e45") }
            },
            IsActive = false
        };

        public static Album GetMockedAlbum50() => new Album
        {
            Id = Guid.Parse("11d77e95-f4f2-4a6f-980b-674d81d8c1d8"),
            Title = "Reputation",
            ImageUrl = "https://example.com/reputation.jpg",
            ReleaseDate = new DateOnly(2017, 11, 10),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("11d77e95-f4f2-4a6f-980b-674d81d8c1d8"), SimilarAlbumId = Guid.Parse("8a145bd2-7b7f-4188-bcdd-1c3a4a7c5e45") },
                new AlbumLink { AlbumId = Guid.Parse("11d77e95-f4f2-4a6f-980b-674d81d8c1d8"), SimilarAlbumId = Guid.Parse("6ee76a77-2be4-42e3-8417-e60d282cffcb") }
            },
            IsActive = false
        };

        public static Album GetMockedAlbum51() => new Album
        {
            Id = Guid.Parse("8a145bd2-7b7f-4188-bcdd-1c3a4a7c5e45"),
            Title = "Cold Mode",
            ImageUrl = "https://example.com/cold-mode.jpg",
            ReleaseDate = new DateOnly(2023, 6, 18),
            SimilarAlbums = new List<AlbumLink>
            {
                new AlbumLink { AlbumId = Guid.Parse("8a145bd2-7b7f-4188-bcdd-1c3a4a7c5e45"), SimilarAlbumId = Guid.Parse("6ee76a77-2be4-42e3-8417-e60d282cffcb") },
                new AlbumLink { AlbumId = Guid.Parse("8a145bd2-7b7f-4188-bcdd-1c3a4a7c5e45"), SimilarAlbumId = Guid.Parse("b58f5f3f-d5e8-49f3-8b12-cfe33f762b4f") }
            },
            IsActive = true
        };

        public static AlbumDto GetMockedAlbumDto1() => ToDto(GetMockedAlbum1());

        public static AlbumDto GetMockedAlbumDto2() => ToDto(GetMockedAlbum2());

        public static AlbumDto GetMockedAlbumDto3() => ToDto(GetMockedAlbum3());

        public static AlbumDto GetMockedAlbumDto4() => ToDto(GetMockedAlbum4());

        public static AlbumDto GetMockedAlbumDto5() => ToDto(GetMockedAlbum5());

        public static AlbumDto GetMockedAlbumDto6() => ToDto(GetMockedAlbum6());

        public static AlbumDto GetMockedAlbumDto7() => ToDto(GetMockedAlbum7());

        public static AlbumDto GetMockedAlbumDto8() => ToDto(GetMockedAlbum8());

        public static AlbumDto GetMockedAlbumDto9() => ToDto(GetMockedAlbum9());

        public static AlbumDto GetMockedAlbumDto10() => ToDto(GetMockedAlbum10());

        public static AlbumDto GetMockedAlbumDto11() => ToDto(GetMockedAlbum11());

        public static AlbumDto GetMockedAlbumDto12() => ToDto(GetMockedAlbum12());

        public static AlbumDto GetMockedAlbumDto13() => ToDto(GetMockedAlbum13());

        public static AlbumDto GetMockedAlbumDto14() => ToDto(GetMockedAlbum14());

        public static AlbumDto GetMockedAlbumDto15() => ToDto(GetMockedAlbum15());

        public static AlbumDto GetMockedAlbumDto16() => ToDto(GetMockedAlbum16());

        public static AlbumDto GetMockedAlbumDto17() => ToDto(GetMockedAlbum17());

        public static AlbumDto GetMockedAlbumDto18() => ToDto(GetMockedAlbum18());

        public static AlbumDto GetMockedAlbumDto19() => ToDto(GetMockedAlbum19());

        public static AlbumDto GetMockedAlbumDto20() => ToDto(GetMockedAlbum20());

        public static AlbumDto GetMockedAlbumDto21() => ToDto(GetMockedAlbum21());

        public static AlbumDto GetMockedAlbumDto22() => ToDto(GetMockedAlbum22());

        public static AlbumDto GetMockedAlbumDto23() => ToDto(GetMockedAlbum23());

        public static AlbumDto GetMockedAlbumDto24() => ToDto(GetMockedAlbum24());

        public static AlbumDto GetMockedAlbumDto25() => ToDto(GetMockedAlbum25());

        public static AlbumDto GetMockedAlbumDto26() => ToDto(GetMockedAlbum26());

        public static AlbumDto GetMockedAlbumDto27() => ToDto(GetMockedAlbum27());

        public static AlbumDto GetMockedAlbumDto28() => ToDto(GetMockedAlbum28());

        public static AlbumDto GetMockedAlbumDto29() => ToDto(GetMockedAlbum29());

        public static AlbumDto GetMockedAlbumDto30() => ToDto(GetMockedAlbum30());

        public static AlbumDto GetMockedAlbumDto31() => ToDto(GetMockedAlbum31());

        public static AlbumDto GetMockedAlbumDto32() => ToDto(GetMockedAlbum32());

        public static AlbumDto GetMockedAlbumDto33() => ToDto(GetMockedAlbum33());

        public static AlbumDto GetMockedAlbumDto34() => ToDto(GetMockedAlbum34());

        public static AlbumDto GetMockedAlbumDto35() => ToDto(GetMockedAlbum35());

        public static AlbumDto GetMockedAlbumDto36() => ToDto(GetMockedAlbum36());

        public static AlbumDto GetMockedAlbumDto37() => ToDto(GetMockedAlbum37());

        public static AlbumDto GetMockedAlbumDto38() => ToDto(GetMockedAlbum38());

        public static AlbumDto GetMockedAlbumDto39() => ToDto(GetMockedAlbum39());

        public static AlbumDto GetMockedAlbumDto40() => ToDto(GetMockedAlbum40());

        public static AlbumDto GetMockedAlbumDto41() => ToDto(GetMockedAlbum41());

        public static AlbumDto GetMockedAlbumDto42() => ToDto(GetMockedAlbum42());

        public static AlbumDto GetMockedAlbumDto43() => ToDto(GetMockedAlbum43());

        public static AlbumDto GetMockedAlbumDto44() => ToDto(GetMockedAlbum44());

        public static AlbumDto GetMockedAlbumDto45() => ToDto(GetMockedAlbum45());

        public static AlbumDto GetMockedAlbumDto46() => ToDto(GetMockedAlbum46());

        public static AlbumDto GetMockedAlbumDto47() => ToDto(GetMockedAlbum47());

        public static AlbumDto GetMockedAlbumDto48() => ToDto(GetMockedAlbum48());

        public static AlbumDto GetMockedAlbumDto49() => ToDto(GetMockedAlbum49());

        public static AlbumDto GetMockedAlbumDto50() => ToDto(GetMockedAlbum50());

        public static AlbumDto GetMockedAlbumDto51() => ToDto(GetMockedAlbum51());

        private static AlbumDto ToDto(Album album) => new AlbumDto
        {
            Id = album.Id,
            Title = album.Title,
            ImageUrl = album.ImageUrl,
            ReleaseDate = album.ReleaseDate,
            SimilarAlbumsIds = album.SimilarAlbums.Select(albumLink => albumLink.SimilarAlbumId).ToList(),
            IsActive = album.IsActive
        };
    }
}