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
    public class ArtistControllerIntegrationTest : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;

        private readonly Artist _artist1 = ArtistMock.GetMockedArtist1();
        private readonly Artist _artist2 = ArtistMock.GetMockedArtist2();
        private readonly IList<Artist> _artists = ArtistMock.GetMockedArtists();
        private readonly ArtistDto _artistDto1 = ArtistMock.GetMockedArtistDto1();
        private readonly ArtistDto _artistDto2 = ArtistMock.GetMockedArtistDto2();
        private readonly IList<ArtistDto> _artistDtos = ArtistMock.GetMockedArtistDtos();
        private readonly IList<ArtistDto> _activeArtistDtos = ArtistMock.GetMockedActiveArtistDtos();
        private readonly IList<ArtistDto> _paginatedArtistDtos = ArtistMock.GetMockedPaginatedArtistDtos();
        private readonly IList<ArtistDto> _activePaginatedArtistDtos = ArtistMock.GetMockedActivePaginatedArtistDtos();
        private readonly ArtistPaginationRequest _paginationRequest = ArtistMock.GetMockedPaginationRequest();

        public ArtistControllerIntegrationTest()
        {
            _fixture = new DbFixture();
            _factory = new CustomWebAppFactory(_fixture);
            _httpClient = _factory.CreateClient();
        }

        private async Task Execute(Func<Task> action)
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SoundSphereDbContext>();
            await context.Artists.AddRangeAsync(_artists);
            await context.SaveChangesAsync();
            await action();
            context.ArtistLinks.RemoveRange(context.ArtistLinks);
            context.Artists.RemoveRange(context.Artists);
            await context.SaveChangesAsync();
        }

        public void Dispose() { _factory.Dispose(); _httpClient.Dispose(); }

        [Fact] public async Task FindAll_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync(Constants.ApiArtist);
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<ArtistDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_artistDtos);
        });

        [Fact] public async Task FindAllActive_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiArtist}/active");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<ArtistDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_activeArtistDtos);
        });

        [Fact] public async Task FindAllPagination_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync($"{Constants.ApiArtist}/pagination", new StringContent(JsonConvert.SerializeObject(_paginationRequest)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<ArtistDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_paginatedArtistDtos);
        });

        [Fact] public async Task FindAllActivePagination_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync($"{Constants.ApiArtist}/active/pagination", new StringContent(JsonConvert.SerializeObject(_paginationRequest)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<ArtistDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_activePaginatedArtistDtos);
        });

        [Fact] public async Task FindById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiArtist}/{Constants.ValidArtistGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<ArtistDto>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(_artistDto1);
        });

        [Fact] public async Task FindById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiArtist}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.ArtistNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task Save_Test() => await Execute(async () =>
        {
            ArtistDto newArtistDto = ArtistMock.GetMockedArtistDto51();
            var saveResponse = await _httpClient.PostAsync(Constants.ApiArtist, new StringContent(JsonConvert.SerializeObject(newArtistDto)));
            saveResponse.Should().NotBeNull();
            saveResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var saveResponseBody = JsonConvert.DeserializeObject<ArtistDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResponseBody.Should().Be(newArtistDto);

            var getAllResponse = await _httpClient.GetAsync(Constants.ApiArtist);
            getAllResponse.Should().NotBeNull();
            getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getAllResponseBody = JsonConvert.DeserializeObject<IList<ArtistDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResponseBody.Should().Contain(newArtistDto);
        });

        [Fact] public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            Artist updatedArtist = GetArtist(_artist2, _artist1.IsActive);
            ArtistDto updatedArtistDto = ToDto(updatedArtist);
            var updateResponse = await _httpClient.PutAsync($"{Constants.ApiArtist}/{Constants.ValidArtistGuid}", new StringContent(JsonConvert.SerializeObject(updatedArtistDto)));
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResponseBody = JsonConvert.DeserializeObject<ArtistDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResponseBody.Should().Be(updatedArtistDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiArtist}/{Constants.ValidArtistGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResponseBody = JsonConvert.DeserializeObject<ArtistDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(updatedArtistDto);
        });

        [Fact] public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PutAsync($"{Constants.ApiArtist}/{Constants.InvalidGuid}", new StringContent(JsonConvert.SerializeObject(_artistDto2)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.ArtistNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task DisableById_ValidId_Test() => await Execute(async () =>
        {
            Artist disabledArtist = GetArtist(_artist1, false);
            ArtistDto disabledArtistDto = ToDto(disabledArtist);
            var deleteResponse = await _httpClient.DeleteAsync($"{Constants.ApiArtist}/{Constants.ValidArtistGuid}");
            deleteResponse.Should().NotBeNull();
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var deleteResponseBody = JsonConvert.DeserializeObject<ArtistDto>(await deleteResponse.Content.ReadAsStringAsync());
            deleteResponseBody.Should().Be(disabledArtistDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiArtist}/{Constants.ValidArtistGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResponseBody = JsonConvert.DeserializeObject<ArtistDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(disabledArtistDto);
        });

        [Fact] public async Task DisableById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{Constants.ApiArtist}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.ArtistNotFound, Constants.InvalidGuid) });
        });

        private Artist GetArtist(Artist artist, bool isActive) => new Artist
        {
            Id = Constants.ValidArtistGuid,
            Name = artist.Name,
            ImageUrl = artist.ImageUrl,
            Bio = artist.Bio,
            SimilarArtists = artist.SimilarArtists,
            IsActive = isActive
        };

        private ArtistDto ToDto(Artist artist) => new ArtistDto
        {
            Id = artist.Id,
            Name = artist.Name,
            ImageUrl = artist.ImageUrl,
            Bio = artist.Bio,
            SimilarArtistsIds = artist.SimilarArtists.Select(artistLink => artistLink.SimilarArtistId).ToList(),
            IsActive = artist.IsActive
        };
    }
}