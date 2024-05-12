using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Tests.Mocks;
using System.Net;

namespace SoundSphere.Tests.Integration.Controllers
{
    public class AlbumControllerIntegrationTest : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;

        private readonly Album _album1 = AlbumMock.GetMockedAlbum1();
        private readonly Album _album2 = AlbumMock.GetMockedAlbum2();
        private readonly IList<Album> _albums = AlbumMock.GetMockedAlbums();
        private readonly AlbumDto _albumDto1 = AlbumMock.GetMockedAlbumDto1();
        private readonly AlbumDto _albumDto2 = AlbumMock.GetMockedAlbumDto2();
        private readonly IList<AlbumDto> _albumDtos = AlbumMock.GetMockedAlbumDtos();
        private readonly IList<AlbumDto> _activeAlbumDtos = AlbumMock.GetMockedActiveAlbumDtos();
        private readonly IList<AlbumDto> _paginatedAlbumDtos = AlbumMock.GetMockedPaginatedAlbumDtos();
        private readonly IList<AlbumDto> _activePaginatedAlbumDtos = AlbumMock.GetMockedActivePaginatedAlbumDtos();
        private readonly AlbumPaginationRequest _paginationRequest = AlbumMock.GetMockedPaginationRequest();

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

        [Fact] public async Task FindAll_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync(Constants.ApiAlbum);
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<AlbumDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_albumDtos);
        });

        [Fact] public async Task FindAllActive_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiAlbum}/active");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<AlbumDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_activeAlbumDtos);
        });

        [Fact] public async Task FindAllPagination_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync($"{Constants.ApiAlbum}/pagination", new StringContent(JsonConvert.SerializeObject(_paginationRequest)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<AlbumDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_paginatedAlbumDtos);
        });

        [Fact] public async Task FindAllActivePagination_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync($"{Constants.ApiAlbum}/active/pagination", new StringContent(JsonConvert.SerializeObject(_paginationRequest)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<AlbumDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_activePaginatedAlbumDtos);
        });

        [Fact] public async Task FindById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiAlbum}/{Constants.ValidAlbumGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<AlbumDto>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(_albumDto1);
        });

        [Fact] public async Task FindById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiAlbum}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.AlbumNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task Save_Test() => await Execute(async () =>
        {
            AlbumDto newAlbumDto = AlbumMock.GetMockedAlbumDto51();
            var saveResponse = await _httpClient.PostAsync(Constants.ApiAlbum, new StringContent(JsonConvert.SerializeObject(newAlbumDto)));
            saveResponse.Should().NotBeNull();
            saveResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var saveResponseBody = JsonConvert.DeserializeObject<AlbumDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResponseBody.Should().Be(newAlbumDto);

            var getAllResponse = await _httpClient.GetAsync(Constants.ApiAlbum);
            getAllResponse.Should().NotBeNull();
            getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getAllResponseBody = JsonConvert.DeserializeObject<IList<AlbumDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResponseBody.Should().Contain(newAlbumDto);
        });

        [Fact] public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            Album updatedAlbum = GetAlbum(_album2, _album1.IsActive);
            AlbumDto updatedAlbumDto = ToDto(updatedAlbum);
            var updateResponse = await _httpClient.PutAsync($"{Constants.ApiAlbum}/{Constants.ValidAlbumGuid}", new StringContent(JsonConvert.SerializeObject(updatedAlbumDto)));
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResponseBody = JsonConvert.DeserializeObject<AlbumDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResponseBody.Should().Be(updatedAlbumDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiAlbum}/{Constants.ValidAlbumGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResponseBody = JsonConvert.DeserializeObject<AlbumDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(updatedAlbumDto);
        });

        [Fact] public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PutAsync($"{Constants.ApiAlbum}/{Constants.InvalidGuid}", new StringContent(JsonConvert.SerializeObject(_albumDto2)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.AlbumNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task DisableById_ValidId_Test() => await Execute(async () =>
        {
            Album disabledAlbum = GetAlbum(_album1, false);
            AlbumDto disabledAlbumDto = ToDto(disabledAlbum);
            var deleteResponse = await _httpClient.DeleteAsync($"{Constants.ApiAlbum}/{Constants.ValidAlbumGuid}");
            deleteResponse.Should().NotBeNull();
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var deleteResponseBody = JsonConvert.DeserializeObject<AlbumDto>(await deleteResponse.Content.ReadAsStringAsync());
            deleteResponseBody.Should().Be(disabledAlbumDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiAlbum}/{Constants.ValidAlbumGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResponseBody = JsonConvert.DeserializeObject<AlbumDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(disabledAlbumDto);
        });

        [Fact] public async Task DisableById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{Constants.ApiAlbum}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.AlbumNotFound, Constants.InvalidGuid) });
        });

        private Album GetAlbum(Album album, bool isActive) => new Album
        {
            Id = Constants.ValidAlbumGuid,
            Title = album.Title,
            ImageUrl = album.ImageUrl,
            ReleaseDate = album.ReleaseDate,
            SimilarAlbums = album.SimilarAlbums,
            IsActive = isActive
        };

        private AlbumDto ToDto(Album album) => new AlbumDto
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