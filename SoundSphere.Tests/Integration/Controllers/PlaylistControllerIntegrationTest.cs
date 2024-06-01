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
using static SoundSphere.Tests.Mocks.PlaylistMock;
using static SoundSphere.Tests.Mocks.UserMock;
using static SoundSphere.Tests.Mocks.SongMock;
using static System.Net.HttpStatusCode;

namespace SoundSphere.Tests.Integration.Controllers
{
    public class PlaylistControllerIntegrationTest : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;

        private readonly Playlist _playlist1 = GetMockedPlaylist1();
        private readonly Playlist _playlist2 = GetMockedPlaylist2();
        private readonly IList<Playlist> _playlists = GetMockedPlaylists();
        private readonly PlaylistDto _playlistDto1 = GetMockedPlaylistDto1();
        private readonly PlaylistDto _playlistDto2 = GetMockedPlaylistDto2();
        private readonly IList<PlaylistDto> _playlistDtos = GetMockedPlaylistDtos();
        private readonly IList<PlaylistDto> _activePlaylistDtos = GetMockedActivePlaylistDtos();
        private readonly IList<PlaylistDto> _paginatedPlaylistDtos = GetMockedPaginatedPlaylistDtos();
        private readonly IList<PlaylistDto> _activePaginatedPlaylistDtos = GetMockedActivePaginatedPlaylistDtos();
        private readonly PlaylistPaginationRequest _paginationRequest = GetMockedPlaylistsPaginationRequest();
        private readonly IList<User> _users = GetMockedUsers();
        private readonly IList<Song> _songs = GetMockedSongs();

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

        [Fact] public async Task GetAll_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync(ApiPlaylist);
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<IList<PlaylistDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_playlistDtos);
        });

        [Fact] public async Task GetAllActive_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{ApiPlaylist}/active");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<IList<PlaylistDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_activePlaylistDtos);
        });

        [Fact] public async Task GetAllPagination_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync($"{ApiPlaylist}/pagination", new StringContent(SerializeObject(_paginationRequest)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<IList<PlaylistDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_paginatedPlaylistDtos);
        });

        [Fact] public async Task GetAllActivePagination_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync($"{ApiPlaylist}/active/pagination", new StringContent(SerializeObject(_paginationRequest)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<IList<PlaylistDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_activePaginatedPlaylistDtos);
        });

        [Fact] public async Task GetById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{ApiPlaylist}/{ValidPlaylistGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<PlaylistDto>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(_playlistDto1);
        });

        [Fact] public async Task GetById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{ApiPlaylist}/{InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(PlaylistNotFound, InvalidGuid) });
        });

        [Fact] public async Task Add_Test() => await Execute(async () =>
        {
            PlaylistDto newPlaylistDto = GetMockedPlaylistDto24();
            var saveResponse = await _httpClient.PostAsync(ApiPlaylist, new StringContent(SerializeObject(newPlaylistDto)));
            saveResponse.Should().NotBeNull();
            saveResponse.StatusCode.Should().Be(Created);
            var saveResponseBody = DeserializeObject<PlaylistDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResponseBody.Should().BeEquivalentTo(newPlaylistDto, options => options.Excluding(playlist => playlist.CreatedAt));

            var getAllResponse = await _httpClient.GetAsync(ApiPlaylist);
            getAllResponse.Should().NotBeNull();
            getAllResponse.StatusCode.Should().Be(OK);
            var getAllResponseBody = DeserializeObject<IList<PlaylistDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResponseBody.Should().Contain(newPlaylistDto);
        });

        [Fact] public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            Playlist updatedPlaylist = new Playlist
            {
                Id = ValidPlaylistGuid,
                Title = _playlist2.Title,
                User = _playlist1.User,
                Songs = _playlist1.Songs,
                CreatedAt = _playlist1.CreatedAt,
                IsActive = _playlist1.IsActive
            };
            PlaylistDto updatedPlaylistDto = ToDto(updatedPlaylist);
            var updateResponse = await _httpClient.PutAsync($"{ApiPlaylist}/{ValidPlaylistGuid}", new StringContent(SerializeObject(updatedPlaylistDto)));
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(OK);
            var updateResponseBody = DeserializeObject<PlaylistDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResponseBody.Should().Be(updatedPlaylistDto);

            var getResponse = await _httpClient.GetAsync($"{ApiPlaylist}/{ValidPlaylistGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(OK);
            var getResponseBody = DeserializeObject<PlaylistDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(updatedPlaylistDto);
        });

        [Fact] public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PutAsync($"{ApiPlaylist}/{InvalidGuid}", new StringContent(SerializeObject(_playlistDto2)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(PlaylistNotFound, InvalidGuid) });
        });

        [Fact] public async Task DeleteById_ValidId_Test() => await Execute(async () =>
        {
            Playlist deletedPlaylist = new Playlist
            {
                Id = ValidPlaylistGuid,
                Title = _playlist1.Title,
                User = _playlist1.User,
                Songs = _playlist1.Songs,
                CreatedAt = _playlist1.CreatedAt,
                IsActive = false
            };
            PlaylistDto deletedPlaylistDto = ToDto(deletedPlaylist);
            var deleteResponse = await _httpClient.DeleteAsync($"{ApiPlaylist}/{ValidPlaylistGuid}");
            deleteResponse.Should().NotBeNull();
            deleteResponse.StatusCode.Should().Be(OK);
            var deleteResponseBody = DeserializeObject<PlaylistDto>(await deleteResponse.Content.ReadAsStringAsync());
            deleteResponseBody.Should().Be(deletedPlaylistDto);

            var getResponse = await _httpClient.GetAsync($"{ApiPlaylist}/{ValidPlaylistGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(OK);
            var getResponseBody = DeserializeObject<PlaylistDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(deletedPlaylistDto);
        });

        [Fact] public async Task DeleteById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{ApiPlaylist}/{InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(PlaylistNotFound, InvalidGuid) });
        });

        private PlaylistDto ToDto(Playlist playlist) => new PlaylistDto
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