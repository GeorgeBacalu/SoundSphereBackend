using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SoundSphereContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions => sqlOptions.MigrationsAssembly("SoundSphere.Api")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();