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
        private readonly IList<SongDto> _activeSongDtos = SongMock.GetMockedActiveSongDtos();
        private readonly IList<SongDto> _paginatedSongDtos = SongMock.GetMockedPaginatedSongDtos();
        private readonly IList<SongDto> _activePaginatedSongDtos = SongMock.GetMockedActivePaginatedSongDtos();
        private readonly SongPaginationRequest _paginationRequest = SongMock.GetMockedPaginationRequest();
        private readonly IList<Album> _albums = AlbumMock.GetMockedAlbums();
        private readonly IList<Artist> _artists = ArtistMock.GetMockedArtists();

        public SongControllerIntegrationTest()
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
            await context.Artists.AddRangeAsync(_artists);
            await context.Songs.AddRangeAsync(_songs);
            await context.SaveChangesAsync();
            await action();
            context.SongLinks.RemoveRange(context.SongLinks);
            context.Songs.RemoveRange(context.Songs);
            context.Albums.RemoveRange(context.Albums);
            context.Artists.RemoveRange(context.Artists);
            await context.SaveChangesAsync();
        }

        public void Dispose() { _factory.Dispose(); _httpClient.Dispose(); }

        [Fact] public async Task FindAll_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync(Constants.ApiSong);
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<SongDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_songDtos);
        });

        [Fact] public async Task FindAllActive_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiSong}/active");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<SongDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_activeSongDtos);
        });

        [Fact] public async Task FindAllPagination_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync($"{Constants.ApiSong}/pagination", new StringContent(JsonConvert.SerializeObject(_paginationRequest)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<SongDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_paginatedSongDtos);
        });

        [Fact] public async Task FindAllActivePagination_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync($"{Constants.ApiSong}/active/pagination", new StringContent(JsonConvert.SerializeObject(_paginationRequest)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<SongDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_activePaginatedSongDtos);
        });

        [Fact] public async Task FindById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiSong}/{Constants.ValidSongGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<SongDto>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(_songDto1);
        });

        [Fact] public async Task FindById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiSong}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.SongNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task Save_Test() => await Execute(async () =>
        {
            SongDto newSongDto = SongMock.GetMockedSongDto5();
            var saveResponse = await _httpClient.PostAsync(Constants.ApiSong, new StringContent(JsonConvert.SerializeObject(newSongDto)));
            saveResponse.Should().NotBeNull();
            saveResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var saveResponseBody = JsonConvert.DeserializeObject<SongDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResponseBody.Should().Be(newSongDto);

            var getAllResponse = await _httpClient.GetAsync(Constants.ApiSong);
            getAllResponse.Should().NotBeNull();
            getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getAllResponseBody = JsonConvert.DeserializeObject<IList<SongDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResponseBody.Should().Contain(newSongDto);
        });

        [Fact] public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            Song updatedSong = GetSong(_song2, _song1.IsActive);
            SongDto updatedSongDto = ToDto(updatedSong);
            var updateResponse = await _httpClient.PutAsync($"{Constants.ApiSong}/{Constants.ValidSongGuid}", new StringContent(JsonConvert.SerializeObject(updatedSongDto)));
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResponseBody = JsonConvert.DeserializeObject<SongDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResponseBody.Should().Be(updatedSongDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiSong}/{Constants.ValidSongGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResponseBody = JsonConvert.DeserializeObject<SongDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(updatedSongDto);
        });

        [Fact] public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PutAsync($"{Constants.ApiSong}/{Constants.InvalidGuid}", new StringContent(JsonConvert.SerializeObject(_songDto2)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.SongNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task DisableById_ValidId_Test() => await Execute(async () =>
        {
            Song disabledSong = GetSong(_song1, false);
            SongDto disabledSongDto = ToDto(disabledSong);
            var deleteResponse = await _httpClient.DeleteAsync($"{Constants.ApiSong}/{Constants.ValidSongGuid}");
            deleteResponse.Should().NotBeNull();
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var deleteResponseBody = JsonConvert.DeserializeObject<SongDto>(await deleteResponse.Content.ReadAsStringAsync());
            deleteResponseBody.Should().Be(disabledSongDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiSong}/{Constants.ValidSongGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResponseBody = JsonConvert.DeserializeObject<SongDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(disabledSongDto);
        });

        [Fact] public async Task DisableById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{Constants.ApiSong}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.SongNotFound, Constants.InvalidGuid) });
        });

        private Song GetSong(Song song, bool isActive) => new Song
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

        private SongDto ToDto(Song song) => new SongDto
        {
            Id = song.Id,
            Title = song.Title,
            ImageUrl = song.ImageUrl,
            Genre = song.Genre,
            ReleaseDate = song.ReleaseDate,
            DurationSeconds = song.DurationSeconds,
            AlbumId = song.Album.Id,
            ArtistsIds = song.Artists.Select(artist => artist.Id).ToList(),
            SimilarSongsIds = song.SimilarSongs.Select(songLink => songLink.SimilarSongId).ToList(),
            IsActive = song.IsActive
        };
    }
}