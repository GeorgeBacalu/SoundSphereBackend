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
    public class SongControllerIntegrationTest : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;

        private readonly Song _song1 = SongMock.GetMockedSong1();
        private readonly Song _song2 = SongMock.GetMockedSong2();
        private readonly IList<Song> _songs = SongMock.GetMockedSongs();
        private readonly SongDto _songDto1 = SongMock.GetMockedSongDto1();
        private readonly SongDto _songDto2 = SongMock.GetMockedSongDto2();
        private readonly IList<SongDto> _songDtos = SongMock.GetMockedSongDtos();

        public SongControllerIntegrationTest()
        {
            _fixture = new DbFixture();
            _factory = new CustomWebAppFactory(_fixture);
            _httpClient = _factory.CreateClient();
        }

        private async Task Execute(Func<Task> action)
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SoundSphereContext>();
            await context.Songs.AddRangeAsync(_songs);
            await context.SaveChangesAsync();
            await action();
            context.Songs.RemoveRange(context.Songs);
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _factory.Dispose();
            _httpClient.Dispose();
        }

        [Fact] public async Task FindAll_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync(Constants.ApiSong);
            response?.Should().NotBeNull();
            response?.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<IList<SongDto>>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(_songDtos);
        });

        [Fact] public async Task FindById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiSong}/{Constants.ValidSongGuid}");
            response?.Should().NotBeNull();
            response?.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<SongDto>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(_songDto1);
        });

        [Fact] public async Task FindById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiSong}/{Constants.InvalidGuid}");
            response?.Should().NotBeNull();
            response?.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(new ProblemDetails
            {
                Title = "Resource not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Song with id {Constants.InvalidGuid} not found!",
            });
        });

        [Fact] public async Task Save_Test() => await Execute(async () =>
        {
            SongDto newSongDto = SongMock.GetMockedSongDto5();
            var requestBody = new StringContent(JsonConvert.SerializeObject(newSongDto), Encoding.UTF8, "application/json");
            var saveResponse = await _httpClient.PostAsync(Constants.ApiSong, requestBody);
            saveResponse?.Should().NotBeNull();
            saveResponse?.StatusCode.Should().Be(HttpStatusCode.Created);
            var saveResult = JsonConvert.DeserializeObject<SongDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResult.Should().BeEquivalentTo(newSongDto);

            var getAllResponse = await _httpClient.GetAsync(Constants.ApiSong);
            getAllResponse?.Should().NotBeNull();
            getAllResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
            var getAllResult = JsonConvert.DeserializeObject<IList<SongDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResult.Should().Contain(newSongDto);
        });

        [Fact] public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            Song updatedSong = CreateTestSong(_song2, _song1.IsActive);
            SongDto updatedSongDto = ConvertToDto(updatedSong);
            var requestBody = new StringContent(JsonConvert.SerializeObject(_songDto2), Encoding.UTF8, "application/json");
            var updateResponse = await _httpClient.PutAsync($"{Constants.ApiSong}/{Constants.ValidSongGuid}", requestBody);
            updateResponse?.Should().NotBeNull();
            updateResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResult = JsonConvert.DeserializeObject<SongDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResult.Should().BeEquivalentTo(updatedSongDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiSong}/{Constants.ValidSongGuid}");
            getResponse?.Should().NotBeNull();
            getResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResult = JsonConvert.DeserializeObject<SongDto>(await getResponse.Content.ReadAsStringAsync());
            getResult.Should().BeEquivalentTo(updatedSongDto);
        });

        [Fact] public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var requestBody = new StringContent(JsonConvert.SerializeObject(_songDto2), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{Constants.ApiSong}/{Constants.InvalidGuid}", requestBody);
            response?.Should().NotBeNull();
            response?.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(new ProblemDetails
            {
                Title = "Resource not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Song with id {Constants.InvalidGuid} not found!",
            });
        });

        [Fact] public async Task DisableById_ValidId_Test() => await Execute(async () =>
        {
            Song disabledSong = CreateTestSong(_song1, false);
            SongDto disabledSongDto = ConvertToDto(disabledSong);
            var disableResponse = await _httpClient.DeleteAsync($"{Constants.ApiSong}/{Constants.ValidSongGuid}");
            disableResponse?.Should().NotBeNull();
            disableResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
            var disableResult = JsonConvert.DeserializeObject<SongDto>(await disableResponse.Content.ReadAsStringAsync());
            disableResult.Should().BeEquivalentTo(disabledSongDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiSong}/{Constants.ValidSongGuid}");
            getResponse?.Should().NotBeNull();
            getResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResult = JsonConvert.DeserializeObject<SongDto>(await getResponse.Content.ReadAsStringAsync());
            getResult.Should().BeEquivalentTo(disabledSongDto);
        });

        [Fact] public async Task DisableById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{Constants.ApiSong}/{Constants.InvalidGuid}");
            response?.Should().NotBeNull();
            response?.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(new ProblemDetails
            {
                Title = "Resource not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Song with id {Constants.InvalidGuid} not found!",
            });
        });

        private Song CreateTestSong(Song song, bool isActive) => new Song
        {
            Id = Constants.ValidSongGuid,
            Title = song.Title,
            ImageUrl = song.ImageUrl,
            Genre = song.Genre,
            ReleaseDate = song.ReleaseDate,
            DurationSeconds = song.DurationSeconds,
            Album = song.Album,
            Artists = song.Artists,
            SimilarSongs = song.SimilarSongs,
            IsActive = isActive
        };

        private SongDto ConvertToDto(Song song) => new SongDto
        {
            Id = song.Id,
            Title = song.Title,
            ImageUrl = song.ImageUrl,
            Genre = song.Genre,
            ReleaseDate = song.ReleaseDate,
            DurationSeconds = song.DurationSeconds,
            AlbumId = song.Album.Id,
            ArtistsIds = song.Artists
                .Select(artist => artist.Id)
                .ToList(),
            SimilarSongsIds = song.SimilarSongs
                .Select(songLink => songLink.SimilarSongId)
                .ToList(),
            IsActive = song.IsActive
        };
    }
}