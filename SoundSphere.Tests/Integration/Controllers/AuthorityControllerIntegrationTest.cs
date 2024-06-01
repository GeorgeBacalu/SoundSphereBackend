using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static Newtonsoft.Json.JsonConvert;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.AuthorityMock;
using static System.Net.HttpStatusCode;

namespace SoundSphere.Tests.Integration.Controllers
{
    public class AuthorityControllerIntegrationTest : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;

        private readonly IList<Authority> _authorities = GetMockedAuthorities();
        private readonly AuthorityDto _authorityDto1 = GetMockedAuthorityDto1();
        private readonly IList<AuthorityDto> _authorityDtos = GetMockedAuthorityDtos();

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

        [Fact] public async Task GetAll_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync(ApiAuthority);
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<IList<AuthorityDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_authorityDtos);
        });

        [Fact] public async Task GetById_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{ApiAuthority}/{ValidAuthorityGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<AuthorityDto>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(_authorityDto1);
        });

        [Fact] public async Task GetById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{ApiAuthority}/{InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(AuthorityNotFound, InvalidGuid) });
        });

        [Fact] public async Task Add_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync(ApiAuthority, new StringContent(SerializeObject(_authorityDto1)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(BadRequest);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Internal server error", Status = Status500InternalServerError, Detail = "Cannot insert duplicate key row in object 'dbo.Authorities' with unique index 'IX_Authorities_Type'. The duplicate key value is (Create)." });
        });
    }
}