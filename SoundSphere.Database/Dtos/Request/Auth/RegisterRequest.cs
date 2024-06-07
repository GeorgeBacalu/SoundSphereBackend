namespace SoundSphere.Database.Dtos.Request.Auth
{
    public record RegisterRequest(string Name, string Email, string Password, string Mobile, string Address, DateOnly Birthday, string Avatar, Guid RoleId);
}