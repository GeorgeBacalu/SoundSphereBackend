using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos
{
    public class AuthorityDto
    {
        public Guid Id { get; set; }
        public AuthorityType Type { get; set; }
    }
}