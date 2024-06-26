using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Request.Auth
{
    public record ChangePasswordRequest(
        [Required(ErrorMessage = "Old password is required")]
        string OldPassword,
        
        [Required(ErrorMessage = "New password is required")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+-=()])(\S){8,30}$", ErrorMessage = "Invalid new password format")]
        string NewPassword,
        
        [Required(ErrorMessage = "Confirm password is required")]
        string ConfirmPassword);
}