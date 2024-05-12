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
    public class UserControllerIntegrationTest : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;

        private readonly User _user1 = UserMock.GetMockedUser1();
        private readonly User _user2 = UserMock.GetMockedUser2();
        private readonly IList<User> _users = UserMock.GetMockedUsers();
        private readonly UserDto _userDto1 = UserMock.GetMockedUserDto1();
        private readonly UserDto _userDto2 = UserMock.GetMockedUserDto2();
        private readonly IList<UserDto> _userDtos = UserMock.GetMockedUserDtos();
        private readonly IList<UserDto> _activeUserDtos = UserMock.GetMockedActiveUserDtos();
        private readonly IList<UserDto> _paginatedUserDtos = UserMock.GetMockedPaginatedUserDtos();
        private readonly IList<UserDto> _activePaginatedUserDtos = UserMock.GetMockedActivePaginatedUserDtos();
        private readonly UserPaginationRequest _paginationRequest = UserMock.GetMockedPaginationRequest();
        private readonly IList<Role> _roles = RoleMock.GetMockedRoles();
        private readonly IList<Authority> _authorities = AuthorityMock.GetMockedAuthorities();

        public UserControllerIntegrationTest()
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
            await context.Roles.AddRangeAsync(_roles);
            await context.Authorities.AddRangeAsync(_authorities);
            await context.SaveChangesAsync();
            await action();
            context.Users.RemoveRange(context.Users);
            context.Roles.RemoveRange(context.Roles);
            context.Authorities.RemoveRange(context.Authorities);
            await context.SaveChangesAsync();
        }

        public void Dispose() { _factory.Dispose(); _httpClient.Dispose(); }

        [Fact] public async Task FindAll_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync(Constants.ApiUser);
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<UserDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_userDtos);
        });

        [Fact] public async Task FindAllActive_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiUser}/active");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<UserDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_activeUserDtos);
        });

        [Fact] public async Task FindAllPagination_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync($"{Constants.ApiUser}/pagination", new StringContent(JsonConvert.SerializeObject(_paginationRequest)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<UserDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_paginatedUserDtos);
        });

        [Fact] public async Task FindAllActivePagination_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync($"{Constants.ApiUser}/active/pagination", new StringContent(JsonConvert.SerializeObject(_paginationRequest)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<UserDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_activePaginatedUserDtos);
        });

        [Fact] public async Task FindById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiUser}/{Constants.ValidUserGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(_userDto1);
        });

        [Fact] public async Task FindById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiUser}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.UserNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task Save_Test() => await Execute(async () =>
        {
            UserDto newUserDto = UserMock.GetMockedUserDto3();
            var saveResponse = await _httpClient.PostAsync(Constants.ApiUser, new StringContent(JsonConvert.SerializeObject(newUserDto)));
            saveResponse.Should().NotBeNull();
            saveResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var saveResponseBody = JsonConvert.DeserializeObject<UserDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResponseBody.Should().Be(newUserDto);

            var getAllResponse = await _httpClient.GetAsync(Constants.ApiUser);
            getAllResponse.Should().NotBeNull();
            getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getAllResponseBody = JsonConvert.DeserializeObject<IList<UserDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResponseBody.Should().Contain(newUserDto);
        });

        [Fact] public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            User updatedUser = GetUser(_user2, _user1.IsActive);
            UserDto updatedUserDto = ToDto(updatedUser);
            var updateResponse = await _httpClient.PutAsync($"{Constants.ApiUser}/{Constants.ValidUserGuid}", new StringContent(JsonConvert.SerializeObject(updatedUserDto)));
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResponseBody = JsonConvert.DeserializeObject<UserDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResponseBody.Should().Be(updatedUserDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiUser}/{Constants.ValidUserGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResponseBody = JsonConvert.DeserializeObject<UserDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(updatedUserDto);
        });

        [Fact] public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PutAsync($"{Constants.ApiUser}/{Constants.InvalidGuid}", new StringContent(JsonConvert.SerializeObject(_userDto2)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.UserNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task DisableById_ValidId_Test() => await Execute(async () =>
        {
            User disabledUser = GetUser(_user1, false);
            UserDto disabledUserDto = ToDto(disabledUser);
            var deleteResponse = await _httpClient.DeleteAsync($"{Constants.ApiUser}/{Constants.ValidUserGuid}");
            deleteResponse.Should().NotBeNull();
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var deleteResponseBody = JsonConvert.DeserializeObject<UserDto>(await deleteResponse.Content.ReadAsStringAsync());
            deleteResponseBody.Should().Be(disabledUserDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiUser}/{Constants.ValidUserGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResponseBody = JsonConvert.DeserializeObject<UserDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(disabledUserDto);
        });

        [Fact] public async Task DisableById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{Constants.ApiUser}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.UserNotFound, Constants.InvalidGuid) });
        });

        private User GetUser(User user, bool isActive) => new User
        {
            Id = Constants.ValidUserGuid,
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Mobile = user.Mobile,
            Address = user.Address,
            Birthday = user.Birthday,
            Avatar = user.Avatar,
            Role = user.Role,
            Authorities = user.Authorities,
            IsActive = isActive
        };

        private UserDto ToDto(User user) => new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Mobile = user.Mobile,
            Address = user.Address,
            Birthday = user.Birthday,
            Avatar = user.Avatar,
            RoleId = user.Role.Id,
            AuthoritiesIds = user.Authorities.Select(authority => authority.Id).ToList(),
            IsActive = user.IsActive
        };
    }
}