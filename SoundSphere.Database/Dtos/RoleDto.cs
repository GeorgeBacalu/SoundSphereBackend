using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public RoleType Type { get; set; }
    }
}