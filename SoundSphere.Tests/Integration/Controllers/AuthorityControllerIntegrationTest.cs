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
    public class AuthorityControllerIntegrationTest : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;

        private readonly IList<Authority> _authorities = AuthorityMock.GetMockedAuthorities();
        private readonly AuthorityDto _authorityDto1 = AuthorityMock.GetMockedAuthorityDto1();
        private readonly IList<AuthorityDto> _authorityDtos = AuthorityMock.GetMockedAuthorityDtos();

        public AuthorityControllerIntegrationTest()
        {
            _fixture = new DbFixture();
            _factory = new CustomWebAppFactory(_fixture);
            _httpClient = _factory.CreateClient();
        }

        private async Task Execute(Func<Task> action)
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SoundSphereDbContext>();
            await context.Authorities.AddRangeAsync(_authorities);
            await context.SaveChangesAsync();
            await action();
            context.Authorities.RemoveRange(context.Authorities);
            await context.SaveChangesAsync();
        }

        public void Dispose() { _factory.Dispose(); _httpClient.Dispose(); }

        [Fact] public async Task FindAll_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync(Constants.ApiAuthority);
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<IList<AuthorityDto>>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(_authorityDtos);
        });

        [Fact] public async Task FindById_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiAuthority}/{Constants.ValidAuthorityGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<AuthorityDto>(await response.Content.ReadAsStringAsync());
            result.Should().Be(_authorityDto1);
        });

        [Fact] public async Task FindById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiAuthority}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.AuthorityNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task Save_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync(Constants.ApiAuthority, new StringContent(JsonConvert.SerializeObject(_authorityDto1)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().Be(new ProblemDetails { Title = "Internal server error", Status = StatusCodes.Status500InternalServerError, Detail = "Cannot insert duplicate key row in object 'dbo.Authorities' with unique index 'IX_Authorities_Type'. The duplicate key value is (Create)." });
        });
    }
}