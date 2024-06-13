using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Attributes
{
    public class DateRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) => 
            (value as dynamic)?.StartDate < (value as dynamic)?.EndDate ? ValidationResult.Success : new ValidationResult("Start date must be earlier than end date");
    }
}