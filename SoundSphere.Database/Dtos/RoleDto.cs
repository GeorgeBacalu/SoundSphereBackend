using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos
{
    public class RoleDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public RoleType Type { get; set; }
    }
}