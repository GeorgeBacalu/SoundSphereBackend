using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SoundSphere.Core.Services;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Context;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using System.Reflection;
using System.Text.Json.Serialization;

public class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<SoundSphereContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions => sqlOptions.MigrationsAssembly("SoundSphere.Api")));
        builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
        builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(SoundSphere.Core.Mappings.AutoMapperProfile).Assembly);
        builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

        builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
        builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
        builder.Services.AddScoped<IAuthorityRepository, AuthorityRepository>();
        builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
        builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<ISongRepository, SongRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddScoped<IAlbumService, AlbumService>();
        builder.Services.AddScoped<IArtistService, ArtistService>();
        builder.Services.AddScoped<IAuthorityService, AuthorityService>();
        builder.Services.AddScoped<IFeedbackService, FeedbackService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<IPlaylistService, PlaylistService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<ISongService, SongService>();
        builder.Services.AddScoped<IUserService, UserService>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "SoundSphere API", Description = "This is a sample REST API documentation for a music streaming service.", Version = "1.0" });
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
        });

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            ExecuteSql(app.Services, Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.sql"));
        }
        app.UseHttpsRedirection();
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    static void ExecuteSql(IServiceProvider services, string path)
    {
        var context = services.CreateScope().ServiceProvider.GetRequiredService<SoundSphereContext>();
        if (!File.Exists(path)) throw new FileNotFoundException("SQL script file not found!", path);
        context.Database.ExecuteSqlRaw(File.ReadAllText(path));
    }
}