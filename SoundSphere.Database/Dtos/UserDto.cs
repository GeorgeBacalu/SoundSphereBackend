using SoundSphere.Database.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(75, ErrorMessage = "Name can't be longer than 75 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Mobile is required")]
        [RegularExpression(@"^(00|\+?40|0)(7\d{2}|\d{2}[13]|[2-37]\d|8[02-9]|9[0-2])\s?\d{3}\s?\d{3}$", ErrorMessage = "Invalid mobile format")]
        public string Mobile { get; set; } = null!;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(150, ErrorMessage = "Address can't be longer than 150 characters")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Birthday is required")]
        [Date(ErrorMessage = "Birthday can't be in the future")]
        public DateOnly Birthday { get; set; }

        [Required(ErrorMessage = "Avatar is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string Avatar { get; set; } = null!;

        [Required(ErrorMessage = "RoleId is required")]
        public Guid RoleId { get; set; }

        [MaxLength(4, ErrorMessage = "There can't be more than 4 authorities")]
        public IList<Guid> AuthoritiesIds { get; set; } = new List<Guid>();
        
        public bool IsActive { get; set; } = true;

        public override bool Equals(object? obj) => obj is UserDto userDto &&
            Id.Equals(userDto.Id) &&
            Name == userDto.Name &&
            Email == userDto.Email &&
            Mobile == userDto.Mobile &&
            Address == userDto.Address &&
            Birthday.Equals(userDto.Birthday) &&
            Avatar == userDto.Avatar &&
            RoleId.Equals(userDto.RoleId) &&
            AuthoritiesIds.SequenceEqual(userDto.AuthoritiesIds) &&
            IsActive == userDto.IsActive;

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Name);
            hash.Add(Email);
            hash.Add(Mobile);
            hash.Add(Address);
            hash.Add(Birthday);
            hash.Add(Avatar);
            hash.Add(RoleId);
            hash.Add(AuthoritiesIds);
            hash.Add(IsActive);
            return hash.ToHashCode();
        }
    }
}