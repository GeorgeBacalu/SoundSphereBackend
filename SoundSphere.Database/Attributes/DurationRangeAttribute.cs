using SoundSphere.Database.Dtos.Request.Models;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Attributes
{
    public class DurationRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            DurationRange? durationRange = value as DurationRange;
            return (durationRange?.MinSeconds == null && durationRange?.MaxSeconds == null) || (durationRange?.MinSeconds < durationRange?.MaxSeconds) ? ValidationResult.Success : new ValidationResult("MinSeconds must be less than MaxSeconds");
        }
    }
}