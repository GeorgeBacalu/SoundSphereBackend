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
using static SoundSphere.Tests.Mocks.SongMock;
using static SoundSphere.Tests.Mocks.AlbumMock;
using static SoundSphere.Tests.Mocks.ArtistMock;
using static System.Net.HttpStatusCode;

namespace SoundSphere.Tests.Integration.Controllers
{
    public class SongControllerIntegrationTest : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;

        private readonly Song _song1 = GetMockedSong1();
        private readonly Song _song2 = GetMockedSong2();
        private readonly IList<Song> _songs = GetMockedSongs();
        private readonly SongDto _songDto1 = GetMockedSongDto1();
        private readonly SongDto _songDto2 = GetMockedSongDto2();
        private readonly IList<SongDto> _songDtos = GetMockedSongDtos();
        private readonly IList<SongDto> _paginatedSongDtos = GetMockedPaginatedSongDtos();
        private readonly SongPaginationRequest _paginationRequest = GetMockedSongsPaginationRequest();
        private readonly IList<Album> _albums = GetMockedAlbums();
        private readonly IList<Artist> _artists = GetMockedArtists();

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

        [Fact] public async Task GetAllActivePagination_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync($"{ApiSong}/get", new StringContent(SerializeObject(_paginationRequest)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<IList<SongDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_paginatedSongDtos);
        });

        [Fact] public async Task GetById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{ApiSong}/{ValidSongGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<SongDto>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(_songDto1);
        });

        [Fact] public async Task GetById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{ApiSong}/{InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(SongNotFound, InvalidGuid) });
        });

        [Fact] public async Task Add_Test() => await Execute(async () =>
        {
            SongDto newSongDto = GetMockedSongDto90();
            var saveResponse = await _httpClient.PostAsync(ApiSong, new StringContent(SerializeObject(newSongDto)));
            saveResponse.Should().NotBeNull();
            saveResponse.StatusCode.Should().Be(Created);
            var saveResponseBody = DeserializeObject<SongDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResponseBody.Should().Be(newSongDto);

            var getAllResponse = await _httpClient.GetAsync(ApiSong);
            getAllResponse.Should().NotBeNull();
            getAllResponse.StatusCode.Should().Be(OK);
            var getAllResponseBody = DeserializeObject<IList<SongDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResponseBody.Should().Contain(newSongDto);
        });

        [Fact] public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            Song updatedSong = GetSong(_song2, true);
            SongDto updatedSongDto = ToDto(updatedSong);
            var updateResponse = await _httpClient.PutAsync($"{ApiSong}/{ValidSongGuid}", new StringContent(SerializeObject(updatedSongDto)));
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(OK);
            var updateResponseBody = DeserializeObject<SongDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResponseBody.Should().Be(updatedSongDto);

            var getResponse = await _httpClient.GetAsync($"{ApiSong}/{ValidSongGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(OK);
            var getResponseBody = DeserializeObject<SongDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(updatedSongDto);
        });

        [Fact] public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PutAsync($"{ApiSong}/{InvalidGuid}", new StringContent(SerializeObject(_songDto2)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(SongNotFound, InvalidGuid) });
        });

        [Fact] public async Task DeleteById_ValidId_Test() => await Execute(async () =>
        {
            Song deletedSong = GetSong(_song1, false);
            SongDto deletedSongDto = ToDto(deletedSong);
            var deleteResponse = await _httpClient.DeleteAsync($"{ApiSong}/{ValidSongGuid}");
            deleteResponse.Should().NotBeNull();
            deleteResponse.StatusCode.Should().Be(OK);
            var deleteResponseBody = DeserializeObject<SongDto>(await deleteResponse.Content.ReadAsStringAsync());
            deleteResponseBody.Should().Be(deletedSongDto);

            var getResponse = await _httpClient.GetAsync($"{ApiSong}/{ValidSongGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(OK);
            var getResponseBody = DeserializeObject<SongDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(deletedSongDto);
        });

        [Fact] public async Task DeleteById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{ApiSong}/{InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(SongNotFound, InvalidGuid) });
        });

        private Song GetSong(Song song, bool isActive) => new Song
        {
            Id = ValidSongGuid,
            Title = song.Title,
            ImageUrl = song.ImageUrl,
            Genre = song.Genre,
            ReleaseDate = song.ReleaseDate,
            DurationSeconds = song.DurationSeconds,
            Album = song.Album,
            Artists = song.Artists,
            SimilarSongs = song.SimilarSongs,
            CreatedAt = song.CreatedAt,
            UpdatedAt = song.UpdatedAt,
            DeletedAt = song.DeletedAt
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
            CreatedAt = song.CreatedAt,
            UpdatedAt = song.UpdatedAt,
            DeletedAt = song.DeletedAt
        };
    }
}