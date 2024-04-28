using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Tests.Mocks;
using System.Net;

namespace SoundSphere.Tests.Integration.Controllers
{
    public class PlaylistControllerIntegrationTest : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;

        private readonly Playlist _playlist1 = PlaylistMock.GetMockedPlaylist1();
        private readonly Playlist _playlist2 = PlaylistMock.GetMockedPlaylist2();
        private readonly IList<Playlist> _playlists = PlaylistMock.GetMockedPlaylists();
        private readonly PlaylistDto _playlistDto1 = PlaylistMock.GetMockedPlaylistDto1();
        private readonly PlaylistDto _playlistDto2 = PlaylistMock.GetMockedPlaylistDto2();
        private readonly IList<PlaylistDto> _playlistDtos = PlaylistMock.GetMockedPlaylistDtos();
        private readonly IList<PlaylistDto> _activePlaylistDtos = PlaylistMock.GetMockedActivePlaylistDtos();
        private readonly IList<User> _users = UserMock.GetMockedUsers();
        private readonly IList<Song> _songs = SongMock.GetMockedSongs();

        public PlaylistControllerIntegrationTest()
        {
            _fixture = new DbFixture();
            _factory = new CustomWebAppFactory(_fixture);
            _httpClient = _factory.CreateClient();
        }

        private async Task Execute(Func<Task> action)
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SoundSphereDbContext>();
            await context.Users.AddRangeAsync(_users);
            await context.Songs.AddRangeAsync(_songs);
            await context.Playlists.AddRangeAsync(_playlists);
            await context.SaveChangesAsync();
            await action();
            context.Playlists.RemoveRange(context.Playlists);
            context.Users.RemoveRange(context.Users);
            context.Songs.RemoveRange(context.Songs);
            await context.SaveChangesAsync();
        }

        public void Dispose() { _factory.Dispose(); _httpClient.Dispose(); }

        [Fact] public async Task FindAll_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync(Constants.ApiPlaylist);
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<IList<PlaylistDto>>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(_playlistDtos);
        });

        [Fact] public async Task FindAllActive_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiPlaylist}/active");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<IList<PlaylistDto>>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(_activePlaylistDtos);
        });

        [Fact] public async Task FindById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiPlaylist}/{Constants.ValidPlaylistGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<PlaylistDto>(await response.Content.ReadAsStringAsync());
            result.Should().Be(_playlistDto1);
        });

        [Fact] public async Task FindById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiPlaylist}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.PlaylistNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task Save_Test() => await Execute(async () =>
        {
            PlaylistDto newPlaylistDto = PlaylistMock.GetMockedPlaylistDto3();
            var saveResponse = await _httpClient.PostAsync(Constants.ApiPlaylist, new StringContent(JsonConvert.SerializeObject(newPlaylistDto)));
            saveResponse.Should().NotBeNull();
            saveResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var saveResult = JsonConvert.DeserializeObject<PlaylistDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResult.Should().BeEquivalentTo(newPlaylistDto, options => options.Excluding(playlist => playlist.CreatedAt));

            var getAllResponse = await _httpClient.GetAsync(Constants.ApiPlaylist);
            getAllResponse.Should().NotBeNull();
            getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getAllResult = JsonConvert.DeserializeObject<IList<PlaylistDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResult.Should().Contain(newPlaylistDto);
        });

        [Fact] public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            Playlist updatedPlaylist = new Playlist
            {
                Id = Constants.ValidPlaylistGuid,
                Title = _playlist2.Title,
                User = _playlist1.User,
                Songs = _playlist1.Songs,
                CreatedAt = _playlist1.CreatedAt,
                IsActive = _playlist1.IsActive
            };
            PlaylistDto updatedPlaylistDto = ConvertToDto(updatedPlaylist);
            var updateResponse = await _httpClient.PutAsync($"{Constants.ApiPlaylist}/{Constants.ValidPlaylistGuid}", new StringContent(JsonConvert.SerializeObject(updatedPlaylistDto)));
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResult = JsonConvert.DeserializeObject<PlaylistDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResult.Should().Be(updatedPlaylistDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiPlaylist}/{Constants.ValidPlaylistGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResult = JsonConvert.DeserializeObject<PlaylistDto>(await getResponse.Content.ReadAsStringAsync());
            getResult.Should().Be(updatedPlaylistDto);
        });

        [Fact] public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PutAsync($"{Constants.ApiPlaylist}/{Constants.InvalidGuid}", new StringContent(JsonConvert.SerializeObject(_playlistDto2)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.PlaylistNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task DisableById_ValidId_Test() => await Execute(async () =>
        {
            Playlist disabledPlaylist = new Playlist
            {
                Id = Constants.ValidPlaylistGuid,
                Title = _playlist1.Title,
                User = _playlist1.User,
                Songs = _playlist1.Songs,
                CreatedAt = _playlist1.CreatedAt,
                IsActive = false
            };
            PlaylistDto disabledPlaylistDto = ConvertToDto(disabledPlaylist);
            var disableResponse = await _httpClient.DeleteAsync($"{Constants.ApiPlaylist}/{Constants.ValidPlaylistGuid}");
            disableResponse.Should().NotBeNull();
            disableResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var disableResult = JsonConvert.DeserializeObject<PlaylistDto>(await disableResponse.Content.ReadAsStringAsync());
            disableResult.Should().Be(disabledPlaylistDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiPlaylist}/{Constants.ValidPlaylistGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResult = JsonConvert.DeserializeObject<PlaylistDto>(await getResponse.Content.ReadAsStringAsync());
            getResult.Should().Be(disabledPlaylistDto);
        });

        [Fact] public async Task DisableById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{Constants.ApiPlaylist}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.PlaylistNotFound, Constants.InvalidGuid) });
        });

        private PlaylistDto ConvertToDto(Playlist playlist) => new PlaylistDto
        {
            Id = playlist.Id,
            Title = playlist.Title,
            UserId = playlist.User.Id,
            SongsIds = playlist.Songs.Select(song => song.Id).ToList(),
            CreatedAt = playlist.CreatedAt,
            IsActive = playlist.IsActive
        };
    }
}