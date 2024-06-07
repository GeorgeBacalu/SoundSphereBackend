using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static Newtonsoft.Json.JsonConvert;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.AlbumMock;
using static System.Net.HttpStatusCode;

namespace SoundSphere.Tests.Integration.Controllers
{
    public class AlbumControllerIntegrationTest : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;

        private readonly Album _album1 = GetMockedAlbum1();
        private readonly Album _album2 = GetMockedAlbum2();
        private readonly IList<Album> _albums = GetMockedAlbums();
        private readonly AlbumDto _albumDto1 = GetMockedAlbumDto1();
        private readonly AlbumDto _albumDto2 = GetMockedAlbumDto2();
        private readonly IList<AlbumDto> _albumDtos = GetMockedAlbumDtos();
        private readonly IList<AlbumDto> _paginatedAlbumDtos = GetMockedPaginatedAlbumDtos();
        private readonly AlbumPaginationRequest _paginationRequest = GetMockedAlbumsPaginationRequest();

        public AlbumControllerIntegrationTest()
        {
            _fixture = new DbFixture();
            _factory = new CustomWebAppFactory(_fixture);
            _httpClient = _factory.CreateClient();
        }

        private async Task Execute(Func<Task> action)
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SoundSphereDbContext>();
            await context.Albums.AddRangeAsync(_albums);
            await context.SaveChangesAsync();
            await action();
            context.AlbumLinks.RemoveRange(context.AlbumLinks);
            context.Albums.RemoveRange(context.Albums);
            await context.SaveChangesAsync();
        }

        public void Dispose() { _factory.Dispose(); _httpClient.Dispose(); }

        [Fact] public async Task GetAll_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync($"{ApiAlbum}/get", new StringContent(SerializeObject(_paginationRequest)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<IList<AlbumDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_paginatedAlbumDtos);
        });

        [Fact] public async Task GetById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{ApiAlbum}/{ValidAlbumGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<AlbumDto>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(_albumDto1);
        });

        [Fact] public async Task GetById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{ApiAlbum}/{InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(AlbumNotFound, InvalidGuid) });
        });

        [Fact] public async Task Add_Test() => await Execute(async () =>
        {
            AlbumDto newAlbumDto = GetMockedAlbumDto51();
            var saveResponse = await _httpClient.PostAsync(ApiAlbum, new StringContent(SerializeObject(newAlbumDto)));
            saveResponse.Should().NotBeNull();
            saveResponse.StatusCode.Should().Be(Created);
            var saveResponseBody = DeserializeObject<AlbumDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResponseBody.Should().Be(newAlbumDto);

            var getAllResponse = await _httpClient.GetAsync(ApiAlbum);
            getAllResponse.Should().NotBeNull();
            getAllResponse.StatusCode.Should().Be(OK);
            var getAllResponseBody = DeserializeObject<IList<AlbumDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResponseBody.Should().Contain(newAlbumDto);
        });

        [Fact] public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            Album updatedAlbum = GetAlbum(_album2, true);
            AlbumDto updatedAlbumDto = ToDto(updatedAlbum);
            var updateResponse = await _httpClient.PutAsync($"{ApiAlbum}/{ValidAlbumGuid}", new StringContent(SerializeObject(updatedAlbumDto)));
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(OK);
            var updateResponseBody = DeserializeObject<AlbumDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResponseBody.Should().Be(updatedAlbumDto);

            var getResponse = await _httpClient.GetAsync($"{ApiAlbum}/{ValidAlbumGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(OK);
            var getResponseBody = DeserializeObject<AlbumDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(updatedAlbumDto);
        });

        [Fact] public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PutAsync($"{ApiAlbum}/{InvalidGuid}", new StringContent(SerializeObject(_albumDto2)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(AlbumNotFound, InvalidGuid) });
        });

        [Fact] public async Task DeleteById_ValidId_Test() => await Execute(async () =>
        {
            Album deletedAlbum = GetAlbum(_album1, false);
            AlbumDto deletedAlbumDto = ToDto(deletedAlbum);
            var deleteResponse = await _httpClient.DeleteAsync($"{ApiAlbum}/{ValidAlbumGuid}");
            deleteResponse.Should().NotBeNull();
            deleteResponse.StatusCode.Should().Be(OK);
            var deleteResponseBody = DeserializeObject<AlbumDto>(await deleteResponse.Content.ReadAsStringAsync());
            deleteResponseBody.Should().Be(deletedAlbumDto);

            var getResponse = await _httpClient.GetAsync($"{ApiAlbum}/{ValidAlbumGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(OK);
            var getResponseBody = DeserializeObject<AlbumDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(deletedAlbumDto);
        });

        [Fact] public async Task DeleteById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{ApiAlbum}/{InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(AlbumNotFound, InvalidGuid) });
        });

        private Album GetAlbum(Album album, bool isActive) => new Album
        {
            Id = ValidAlbumGuid,
            Title = album.Title,
            ImageUrl = album.ImageUrl,
            ReleaseDate = album.ReleaseDate,
            SimilarAlbums = album.SimilarAlbums,
            CreatedAt = album.CreatedAt,
            UpdatedAt = album.UpdatedAt,
            DeletedAt = album.DeletedAt
        };

        private AlbumDto ToDto(Album album) => new AlbumDto
        {
            Id = album.Id,
            Title = album.Title,
            ImageUrl = album.ImageUrl,
            ReleaseDate = album.ReleaseDate,
            SimilarAlbumsIds = album.SimilarAlbums.Select(albumLink => albumLink.SimilarAlbumId).ToList(),
            CreatedAt = album.CreatedAt,
            UpdatedAt = album.UpdatedAt,
            DeletedAt = album.DeletedAt
        };
    }
}