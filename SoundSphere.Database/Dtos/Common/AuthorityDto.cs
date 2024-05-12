using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Common
{
    public class AuthorityDto
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public AuthorityType Type { get; set; }

        public override bool Equals(object? obj) => obj is AuthorityDto authorityDto && Id.Equals(authorityDto.Id) && Type == authorityDto.Type;

        public override int GetHashCode() => HashCode.Combine(Id, Type);
    }
}