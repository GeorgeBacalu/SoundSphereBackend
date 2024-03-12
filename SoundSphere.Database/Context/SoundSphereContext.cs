using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Context
{
    public class SoundSphereContext : DbContext
    {
        public SoundSphereContext(DbContextOptions<SoundSphereContext> options) : base(options) {}

        public DbSet<User> Users { get; set; } = null!;
    }
}