using SoundSphere.Database.Dtos.Request.Models;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Attributes
{
    public class DurationRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) =>
            (value as DurationRange)?.MinSeconds < (value as DurationRange)?.MaxSeconds ? ValidationResult.Success : new ValidationResult("MinSeconds must be less than MaxSeconds");
    }
}