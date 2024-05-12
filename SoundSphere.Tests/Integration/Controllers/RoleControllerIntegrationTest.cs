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
    public class RoleControllerIntegrationTest : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;

        private readonly IList<Role> _roles = RoleMock.GetMockedRoles();
        private readonly RoleDto _roleDto1 = RoleMock.GetMockedRoleDto1();
        private readonly IList<RoleDto> _roleDtos = RoleMock.GetMockedRoleDtos();

        public RoleControllerIntegrationTest()
        {
            _fixture = new DbFixture();
            _factory = new CustomWebAppFactory(_fixture);
            _httpClient = _factory.CreateClient();
        }

        private async Task Execute(Func<Task> action)
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SoundSphereDbContext>();
            await context.Roles.AddRangeAsync(_roles);
            await context.SaveChangesAsync();
            await action();
            context.Roles.RemoveRange(context.Roles);
            await context.SaveChangesAsync();
        }

        public void Dispose() { _factory.Dispose(); _httpClient.Dispose(); }

        [Fact] public async Task FindAll_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync(Constants.ApiRole);
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<RoleDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_roleDtos);
        });

        [Fact] public async Task FindById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiRole}/{_roleDto1.Id}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<RoleDto>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(_roleDto1);
        });

        [Fact] public async Task FindById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiRole}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.RoleNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task Save_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync(Constants.ApiRole, new StringContent(JsonConvert.SerializeObject(_roleDto1)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Internal server error", Status = StatusCodes.Status400BadRequest, Detail = "Cannot insert duplicate key row in object 'dbo.Roles' with unique index 'IX_Roles_Type'. The duplicate key value is (Create)." });
        });
    }
}