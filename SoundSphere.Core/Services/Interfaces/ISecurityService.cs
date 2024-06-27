using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface ISecurityService
    {
        public string GetToken(User user, string role);

        public bool ValidateToken(string tokenString);

        public byte[] GenerateSalt();

        public string HashPassword(string password, byte[] salt);
    }
}