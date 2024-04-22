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
            var result = JsonConvert.DeserializeObject<IList<UserDto>>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(_userDtos);
        });

        [Fact] public async Task FindAllActive_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiUser}/active");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<IList<UserDto>>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(_activeUserDtos);
        });

        [Fact] public async Task FindById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiUser}/{Constants.ValidUserGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync());
            result.Should().Be(_userDto1);
        });

        [Fact] public async Task FindById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiUser}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.UserNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task Save_Test() => await Execute(async () =>
        {
            UserDto newUserDto = UserMock.GetMockedUserDto3();
            var saveResponse = await _httpClient.PostAsync(Constants.ApiUser, new StringContent(JsonConvert.SerializeObject(newUserDto)));
            saveResponse.Should().NotBeNull();
            saveResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var saveResult = JsonConvert.DeserializeObject<UserDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResult.Should().Be(newUserDto);

            var getAllResponse = await _httpClient.GetAsync(Constants.ApiUser);
            getAllResponse.Should().NotBeNull();
            getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getAllResult = JsonConvert.DeserializeObject<IList<UserDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResult.Should().Contain(newUserDto);
        });

        [Fact] public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            User updatedUser = CreateTestUser(_user2, _user1.IsActive);
            UserDto updatedUserDto = ConvertToDto(updatedUser);
            var updateResponse = await _httpClient.PutAsync($"{Constants.ApiUser}/{Constants.ValidUserGuid}", new StringContent(JsonConvert.SerializeObject(updatedUserDto)));
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResult = JsonConvert.DeserializeObject<UserDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResult.Should().Be(updatedUserDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiUser}/{Constants.ValidUserGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResult = JsonConvert.DeserializeObject<UserDto>(await getResponse.Content.ReadAsStringAsync());
            getResult.Should().Be(updatedUserDto);
        });

        [Fact] public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PutAsync($"{Constants.ApiUser}/{Constants.InvalidGuid}", new StringContent(JsonConvert.SerializeObject(_userDto2)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.UserNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task DisableById_ValidId_Test() => await Execute(async () =>
        {
            User disabledUser = CreateTestUser(_user1, false);
            UserDto disabledUserDto = ConvertToDto(disabledUser);
            var disableResponse = await _httpClient.DeleteAsync($"{Constants.ApiUser}/{Constants.ValidUserGuid}");
            disableResponse.Should().NotBeNull();
            disableResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var disableResult = JsonConvert.DeserializeObject<UserDto>(await disableResponse.Content.ReadAsStringAsync());
            disableResult.Should().Be(disabledUserDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiUser}/{Constants.ValidUserGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResult = JsonConvert.DeserializeObject<UserDto>(await getResponse.Content.ReadAsStringAsync());
            getResult.Should().Be(disabledUserDto);
        });

        [Fact] public async Task DisableById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{Constants.ApiUser}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.UserNotFound, Constants.InvalidGuid) });
        });

        private User CreateTestUser(User user, bool isActive) => new User
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

        private UserDto ConvertToDto(User user) => new UserDto
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