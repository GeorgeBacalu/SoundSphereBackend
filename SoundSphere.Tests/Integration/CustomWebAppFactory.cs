using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace SoundSphere.Tests.Integration
{
    public class CustomWebAppFactory : WebApplicationFactory<Program>
    {
        private readonly DbFixture _fixture;

        public CustomWebAppFactory(DbFixture fixture) => _fixture = fixture;

        protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.ConfigureServices(services => services.AddScoped(_ => _fixture.CreateContext()));
    }
}