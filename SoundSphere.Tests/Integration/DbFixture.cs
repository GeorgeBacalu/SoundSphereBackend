using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SoundSphere.Database.Context;

namespace SoundSphere.Tests.Integration
{
    public class DbFixture
    {
        private readonly IConfiguration _configuration;

        public DbFixture() => _configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\SoundSphere.Api"))
            .AddJsonFile("appsettings.Test.json")
            .Build();

        public SoundSphereContext CreateContext() => new SoundSphereContext(new DbContextOptionsBuilder<SoundSphereContext>()
            .UseSqlServer(_configuration.GetConnectionString("DefaultConnection")).Options);
    }
}