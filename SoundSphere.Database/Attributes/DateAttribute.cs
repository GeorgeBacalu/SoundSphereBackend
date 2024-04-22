using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Attributes
{
    public class DateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext context) =>
            value is DateOnly date && date > DateOnly.FromDateTime(DateTime.Now)
                 ? new ValidationResult(ErrorMessage ?? "Date can't be in the future")
                 : !(value is DateOnly)
                   ? new ValidationResult("Invalid data type")
                   : ValidationResult.Success;
    }
}