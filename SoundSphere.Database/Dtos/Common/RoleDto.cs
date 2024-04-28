using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Common
{
    public class RoleDto
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public RoleType Type { get; set; }

        public override bool Equals(object? obj) => obj is RoleDto roleDto && Id.Equals(roleDto.Id) && Type == roleDto.Type;

        public override int GetHashCode() => HashCode.Combine(Id, Type);
    }
}