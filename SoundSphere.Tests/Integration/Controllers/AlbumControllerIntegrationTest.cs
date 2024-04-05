using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Tests.Mocks;
using System.Net;
using System.Text;

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

        public AlbumControllerIntegrationTest()
        {
            _fixture = new DbFixture();
            _factory = new CustomWebAppFactory(_fixture);
            _httpClient = _factory.CreateClient();
        }

        private async Task Execute(Func<Task> action)
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SoundSphereContext>();
            await context.Albums.AddRangeAsync(_albums);
            await context.SaveChangesAsync();
            await action();
            context.AlbumLinks.RemoveRange(context.AlbumLinks);
            context.Albums.RemoveRange(context.Albums);
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _factory.Dispose();
            _httpClient.Dispose();
        }

        [Fact] public async Task FindAll_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync(Constants.ApiAlbum);
            response?.Should().NotBeNull();
            response?.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<IList<AlbumDto>>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(_albumDtos);
        });

        [Fact] public async Task FindById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiAlbum}/{Constants.ValidAlbumGuid}");
            response?.Should().NotBeNull();
            response?.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<AlbumDto>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(_albumDto1);
        });

        [Fact] public async Task FindById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiAlbum}/{Constants.InvalidGuid}");
            response?.Should().NotBeNull();
            response?.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(new ProblemDetails 
            { 
                Title = "Resource not found", 
                Status = StatusCodes.Status404NotFound, 
                Detail = $"Album with id {Constants.InvalidGuid} not found!" 
            });
        });

        [Fact] public async Task Save_Test() => await Execute(async () =>
        {
            AlbumDto newAlbumDto = AlbumMock.GetMockedAlbumDto3();
            var requestBody = new StringContent(JsonConvert.SerializeObject(newAlbumDto), Encoding.UTF8, "application/json");
            var saveResponse = await _httpClient.PostAsync(Constants.ApiAlbum, requestBody);
            saveResponse?.Should().NotBeNull();
            saveResponse?.StatusCode.Should().Be(HttpStatusCode.Created);
            var saveResult = JsonConvert.DeserializeObject<AlbumDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResult.Should().BeEquivalentTo(newAlbumDto);

            var getAllResponse = await _httpClient.GetAsync(Constants.ApiAlbum);
            getAllResponse?.Should().NotBeNull();
            getAllResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
            var getAllResult = JsonConvert.DeserializeObject<IList<AlbumDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResult.Should().Contain(newAlbumDto);
        });

        [Fact] public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            Album updatedAlbum = CreateTestAlbum(_album2, _album1.IsActive);
            AlbumDto updatedAlbumDto = ConvertToDto(updatedAlbum);
            var requestBody = new StringContent(JsonConvert.SerializeObject(_albumDto2), Encoding.UTF8, "application/json");
            var updateResponse = await _httpClient.PutAsync($"{Constants.ApiAlbum}/{Constants.ValidAlbumGuid}", requestBody);
            updateResponse?.Should().NotBeNull();
            updateResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResult = JsonConvert.DeserializeObject<AlbumDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResult.Should().BeEquivalentTo(updatedAlbumDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiAlbum}/{Constants.ValidAlbumGuid}");
            getResponse?.Should().NotBeNull();
            getResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResult = JsonConvert.DeserializeObject<AlbumDto>(await getResponse.Content.ReadAsStringAsync());
            getResult.Should().BeEquivalentTo(updatedAlbumDto);
        });

        [Fact] public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var requestBody = new StringContent(JsonConvert.SerializeObject(_albumDto2), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{Constants.ApiAlbum}/{Constants.InvalidGuid}", requestBody);
            response?.Should().NotBeNull();
            response?.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(new ProblemDetails
            {
                Title = "Resource not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Album with id {Constants.InvalidGuid} not found!"
            });
        });

        [Fact] public async Task DisableById_ValidId_Test() => await Execute(async () =>
        {
            Album disabledAlbum = CreateTestAlbum(_album1, false);
            AlbumDto disabledAlbumDto = ConvertToDto(disabledAlbum);
            var disableResponse = await _httpClient.DeleteAsync($"{Constants.ApiAlbum}/{Constants.ValidAlbumGuid}");
            disableResponse?.Should().NotBeNull();
            disableResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
            var disableResult = JsonConvert.DeserializeObject<AlbumDto>(await disableResponse.Content.ReadAsStringAsync());
            disableResult.Should().BeEquivalentTo(disabledAlbumDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiAlbum}/{Constants.ValidAlbumGuid}");
            getResponse?.Should().NotBeNull();
            getResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResult = JsonConvert.DeserializeObject<AlbumDto>(await getResponse.Content.ReadAsStringAsync());
            getResult.Should().BeEquivalentTo(disabledAlbumDto);
        });

        [Fact] public async Task DisableById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{Constants.ApiAlbum}/{Constants.InvalidGuid}");
            response?.Should().NotBeNull();
            response?.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(new ProblemDetails
            {
                Title = "Resource not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Album with id {Constants.InvalidGuid} not found!"
            });
        });

        private Album CreateTestAlbum(Album album, bool isActive) => new Album
        {
            Id = Constants.ValidAlbumGuid,
            Title = album.Title,
            ImageUrl = album.ImageUrl,
            ReleaseDate = album.ReleaseDate,
            SimilarAlbums = album.SimilarAlbums,
            IsActive = isActive
        };

        private AlbumDto ConvertToDto(Album album) => new AlbumDto
        {
            Id = album.Id,
            Title = album.Title,
            ImageUrl = album.ImageUrl,
            ReleaseDate = album.ReleaseDate,
            SimilarAlbumsIds = album.SimilarAlbums
                .Select(albumLink => albumLink.SimilarAlbumId)
                .ToList(),
            IsActive = album.IsActive
        };
    }
}