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
            var result = JsonConvert.DeserializeObject<IList<SongDto>>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(_songDtos);
        });

        [Fact] public async Task FindAllActive_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiSong}/active");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<IList<SongDto>>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(_activeSongDtos);
        });

        [Fact] public async Task FindById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiSong}/{Constants.ValidSongGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<SongDto>(await response.Content.ReadAsStringAsync());
            result.Should().Be(_songDto1);
        });

        [Fact] public async Task FindById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiSong}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.SongNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task Save_Test() => await Execute(async () =>
        {
            SongDto newSongDto = SongMock.GetMockedSongDto5();
            var saveResponse = await _httpClient.PostAsync(Constants.ApiSong, new StringContent(JsonConvert.SerializeObject(newSongDto)));
            saveResponse.Should().NotBeNull();
            saveResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var saveResult = JsonConvert.DeserializeObject<SongDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResult.Should().Be(newSongDto);

            var getAllResponse = await _httpClient.GetAsync(Constants.ApiSong);
            getAllResponse.Should().NotBeNull();
            getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getAllResult = JsonConvert.DeserializeObject<IList<SongDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResult.Should().Contain(newSongDto);
        });

        [Fact] public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            Song updatedSong = CreateTestSong(_song2, _song1.IsActive);
            SongDto updatedSongDto = ConvertToDto(updatedSong);
            var updateResponse = await _httpClient.PutAsync($"{Constants.ApiSong}/{Constants.ValidSongGuid}", new StringContent(JsonConvert.SerializeObject(updatedSongDto)));
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResult = JsonConvert.DeserializeObject<SongDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResult.Should().Be(updatedSongDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiSong}/{Constants.ValidSongGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResult = JsonConvert.DeserializeObject<SongDto>(await getResponse.Content.ReadAsStringAsync());
            getResult.Should().Be(updatedSongDto);
        });

        [Fact] public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PutAsync($"{Constants.ApiSong}/{Constants.InvalidGuid}", new StringContent(JsonConvert.SerializeObject(_songDto2)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.SongNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task DisableById_ValidId_Test() => await Execute(async () =>
        {
            Song disabledSong = CreateTestSong(_song1, false);
            SongDto disabledSongDto = ConvertToDto(disabledSong);
            var disableResponse = await _httpClient.DeleteAsync($"{Constants.ApiSong}/{Constants.ValidSongGuid}");
            disableResponse.Should().NotBeNull();
            disableResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var disableResult = JsonConvert.DeserializeObject<SongDto>(await disableResponse.Content.ReadAsStringAsync());
            disableResult.Should().Be(disabledSongDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiSong}/{Constants.ValidSongGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResult = JsonConvert.DeserializeObject<SongDto>(await getResponse.Content.ReadAsStringAsync());
            getResult.Should().Be(disabledSongDto);
        });

        [Fact] public async Task DisableById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{Constants.ApiSong}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.SongNotFound, Constants.InvalidGuid) });
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
            ArtistsIds = song.Artists.Select(artist => artist.Id).ToList(),
            SimilarSongsIds = song.SimilarSongs.Select(songLink => songLink.SimilarSongId).ToList(),
            IsActive = song.IsActive
        };
    }
}