using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Request.Auth
{
    public record LoginRequest(
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        string Email,

        [Required(ErrorMessage = "Password is required")]
        string Password);
}