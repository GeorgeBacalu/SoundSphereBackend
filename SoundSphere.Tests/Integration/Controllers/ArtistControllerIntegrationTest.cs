using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos;
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
            var result = JsonConvert.DeserializeObject<IList<ArtistDto>>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(_artistDtos);
        });

        [Fact] public async Task FindAllActive_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiArtist}/active");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<IList<ArtistDto>>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(_activeArtistDtos);
        });

        [Fact] public async Task FindById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiArtist}/{Constants.ValidArtistGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<ArtistDto>(await response.Content.ReadAsStringAsync());
            result.Should().Be(_artistDto1);
        });

        [Fact] public async Task FindById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiArtist}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.ArtistNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task Save_Test() => await Execute(async () =>
        {
            ArtistDto newArtistDto = ArtistMock.GetMockedArtistDto3();
            var saveResponse = await _httpClient.PostAsync(Constants.ApiArtist, new StringContent(JsonConvert.SerializeObject(newArtistDto)));
            saveResponse.Should().NotBeNull();
            saveResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var saveResult = JsonConvert.DeserializeObject<ArtistDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResult.Should().Be(newArtistDto);

            var getAllResponse = await _httpClient.GetAsync(Constants.ApiArtist);
            getAllResponse.Should().NotBeNull();
            getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getAllResult = JsonConvert.DeserializeObject<IList<ArtistDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResult.Should().Contain(newArtistDto);
        });

        [Fact] public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            Artist updatedArtist = CreateTestArtist(_artist2, _artist1.IsActive);
            ArtistDto updatedArtistDto = ConvertToDto(updatedArtist);
            var updateResponse = await _httpClient.PutAsync($"{Constants.ApiArtist}/{Constants.ValidArtistGuid}", new StringContent(JsonConvert.SerializeObject(updatedArtistDto)));
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResult = JsonConvert.DeserializeObject<ArtistDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResult.Should().Be(updatedArtistDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiArtist}/{Constants.ValidArtistGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResult = JsonConvert.DeserializeObject<ArtistDto>(await getResponse.Content.ReadAsStringAsync());
            getResult.Should().Be(updatedArtistDto);
        });

        [Fact] public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PutAsync($"{Constants.ApiArtist}/{Constants.InvalidGuid}", new StringContent(JsonConvert.SerializeObject(_artistDto2)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.ArtistNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task DisableById_ValidId_Test() => await Execute(async () =>
        {
            Artist disabledArtist = CreateTestArtist(_artist1, false);
            ArtistDto disabledArtistDto = ConvertToDto(disabledArtist);
            var disableResponse = await _httpClient.DeleteAsync($"{Constants.ApiArtist}/{Constants.ValidArtistGuid}");
            disableResponse.Should().NotBeNull();
            disableResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var disableResult = JsonConvert.DeserializeObject<ArtistDto>(await disableResponse.Content.ReadAsStringAsync());
            disableResult.Should().Be(disabledArtistDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiArtist}/{Constants.ValidArtistGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResult = JsonConvert.DeserializeObject<ArtistDto>(await getResponse.Content.ReadAsStringAsync());
            getResult.Should().Be(disabledArtistDto);
        });

        [Fact] public async Task DisableById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{Constants.ApiArtist}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.ArtistNotFound, Constants.InvalidGuid) });
        });

        private Artist CreateTestArtist(Artist artist, bool isActive) => new Artist
        {
            Id = Constants.ValidArtistGuid,
            Name = artist.Name,
            ImageUrl = artist.ImageUrl,
            Bio = artist.Bio,
            SimilarArtists = artist.SimilarArtists,
            IsActive = isActive
        };

        private ArtistDto ConvertToDto(Artist artist) => new ArtistDto
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