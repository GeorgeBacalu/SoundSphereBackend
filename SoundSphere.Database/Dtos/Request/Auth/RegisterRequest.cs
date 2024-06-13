using SoundSphere.Database.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Request.Auth
{
    public record RegisterRequest(
        [Required(ErrorMessage = "Name is required")]
        [StringLength(75, ErrorMessage = "Name can't be longer than 75 characters")]
        string Name,

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        string Email,

        /**
         * Password must contain:
         * - at least one digit
         * - at least one lowercase letter
         * - at least one uppercase letter
         * - at least one special character
         * - no whitespace
         * - between 8 and 30 characters
         */
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+-=()])(\S){8,30}$", ErrorMessage = "Invalid password format")]
        string Password,

        /**
         * Mobile must follow the following format:
         * - should start with 00 or +40 or 0
         * - followed by the mobile prefix
         * - optional space
         * - followed by 3 digits representing the first part of the number
         * - options space
         * - followed by 3 digits representing the second part of the number
         */
        [Required(ErrorMessage = "Mobile is required")]
        [RegularExpression(@"^(00|\+?40|0)(7\d{2}|\d{2}[13]|[2-37]\d|8[02-9]|9[0-2])\s?\d{3}\s?\d{3}$", ErrorMessage = "Invalid mobile format")]
        string Mobile,

        [Required(ErrorMessage = "Address is required")]
        [StringLength(150, ErrorMessage = "Address can't be longer than 150 characters")]
        string Address,

        [Required(ErrorMessage = "Birthday is required")]
        [Date(ErrorMessage = "Birthday can't be in the future")]
        DateOnly Birthday,

        [Required(ErrorMessage = "Avatar is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        string Avatar,

        [Required(ErrorMessage = "RoleId is required")]
        Guid RoleId);
}