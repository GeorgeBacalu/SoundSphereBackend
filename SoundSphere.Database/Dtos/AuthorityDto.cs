using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos
{
    public class AuthorityDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public AuthorityType Type { get; set; }
    }
}