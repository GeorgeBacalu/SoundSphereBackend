using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SoundSphere.Database.Context;

namespace SoundSphere.Tests.Integration
{
    public class DbFixture
    {
        private readonly IConfiguration _configuration;

        public DbFixture() => _configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.Test.json").Build();

        public SoundSphereDbContext CreateContext() => new SoundSphereDbContext(new DbContextOptionsBuilder<SoundSphereDbContext>().UseSqlServer(_configuration.GetConnectionString("DefaultConnection")).Options);
    }
}