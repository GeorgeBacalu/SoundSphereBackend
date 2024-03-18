using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        [EmailAddress] public string Email { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateOnly Birthday { get; set; }
        public string Avatar { get; set; } = null!;
        public Guid RoleId { get; set; } // ManyToOne with Role
        public IList<Guid> AuthoritiesIds { get; set; } = new List<Guid>(); // ManyToMany with Authority
        public bool IsActive { get; set; } = true;
    }
}
