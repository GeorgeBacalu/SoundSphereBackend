using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static Newtonsoft.Json.JsonConvert;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.ArtistMock;
using static System.Net.HttpStatusCode;

namespace SoundSphere.Tests.Integration.Controllers
{
    public class ArtistControllerIntegrationTest : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;

        private readonly Artist _artist1 = GetMockedArtist1();
        private readonly Artist _artist2 = GetMockedArtist2();
        private readonly IList<Artist> _artists = GetMockedArtists();
        private readonly ArtistDto _artistDto1 = GetMockedArtistDto1();
        private readonly ArtistDto _artistDto2 = GetMockedArtistDto2();
        private readonly IList<ArtistDto> _artistDtos = GetMockedArtistDtos();
        private readonly IList<ArtistDto> _activeArtistDtos = GetMockedActiveArtistDtos();
        private readonly IList<ArtistDto> _paginatedArtistDtos = GetMockedPaginatedArtistDtos();
        private readonly IList<ArtistDto> _activePaginatedArtistDtos = GetMockedActivePaginatedArtistDtos();
        private readonly ArtistPaginationRequest _paginationRequest = GetMockedArtistsPaginationRequest();

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

        [Fact] public async Task GetAll_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync(ApiArtist);
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<IList<ArtistDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_artistDtos);
        });

        [Fact] public async Task GetAllActive_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{ApiArtist}/active");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<IList<ArtistDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_activeArtistDtos);
        });

        [Fact] public async Task GetAllPagination_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync($"{ApiArtist}/pagination", new StringContent(SerializeObject(_paginationRequest)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<IList<ArtistDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_paginatedArtistDtos);
        });

        [Fact] public async Task GetAllActivePagination_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync($"{ApiArtist}/active/pagination", new StringContent(SerializeObject(_paginationRequest)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<IList<ArtistDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_activePaginatedArtistDtos);
        });

        [Fact] public async Task GetById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{ApiArtist}/{ValidArtistGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<ArtistDto>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(_artistDto1);
        });

        [Fact] public async Task GetById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{ApiArtist}/{InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(ArtistNotFound, InvalidGuid) });
        });

        [Fact] public async Task Add_Test() => await Execute(async () =>
        {
            ArtistDto newArtistDto = GetMockedArtistDto51();
            var saveResponse = await _httpClient.PostAsync(ApiArtist, new StringContent(SerializeObject(newArtistDto)));
            saveResponse.Should().NotBeNull();
            saveResponse.StatusCode.Should().Be(Created);
            var saveResponseBody = DeserializeObject<ArtistDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResponseBody.Should().Be(newArtistDto);

            var getAllResponse = await _httpClient.GetAsync(ApiArtist);
            getAllResponse.Should().NotBeNull();
            getAllResponse.StatusCode.Should().Be(OK);
            var getAllResponseBody = DeserializeObject<IList<ArtistDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResponseBody.Should().Contain(newArtistDto);
        });

        [Fact] public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            Artist updatedArtist = GetArtist(_artist2, _artist1.IsActive);
            ArtistDto updatedArtistDto = ToDto(updatedArtist);
            var updateResponse = await _httpClient.PutAsync($"{ApiArtist}/{ValidArtistGuid}", new StringContent(SerializeObject(updatedArtistDto)));
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(OK);
            var updateResponseBody = DeserializeObject<ArtistDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResponseBody.Should().Be(updatedArtistDto);

            var getResponse = await _httpClient.GetAsync($"{ApiArtist}/{ValidArtistGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(OK);
            var getResponseBody = DeserializeObject<ArtistDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(updatedArtistDto);
        });

        [Fact] public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PutAsync($"{ApiArtist}/{InvalidGuid}", new StringContent(SerializeObject(_artistDto2)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(ArtistNotFound, InvalidGuid) });
        });

        [Fact] public async Task DeleteById_ValidId_Test() => await Execute(async () =>
        {
            Artist deletedArtist = GetArtist(_artist1, false);
            ArtistDto deletedArtistDto = ToDto(deletedArtist);
            var deleteResponse = await _httpClient.DeleteAsync($"{ApiArtist}/{ValidArtistGuid}");
            deleteResponse.Should().NotBeNull();
            deleteResponse.StatusCode.Should().Be(OK);
            var deleteResponseBody = DeserializeObject<ArtistDto>(await deleteResponse.Content.ReadAsStringAsync());
            deleteResponseBody.Should().Be(deletedArtistDto);

            var getResponse = await _httpClient.GetAsync($"{ApiArtist}/{ValidArtistGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(OK);
            var getResponseBody = DeserializeObject<ArtistDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(deletedArtistDto);
        });

        [Fact] public async Task DeleteById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{ApiArtist}/{InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(ArtistNotFound, InvalidGuid) });
        });

        private Artist GetArtist(Artist artist, bool isActive) => new Artist
        {
            Id = ValidArtistGuid,
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