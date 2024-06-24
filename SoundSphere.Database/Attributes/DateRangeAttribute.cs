using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Attributes
{
    public class DateRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            dynamic? dvalue = value as dynamic;
            return (dvalue?.StartDate == null && dvalue?.EndDate == null) || (dvalue?.StartDate < dvalue?.EndDate) ? ValidationResult.Success : new ValidationResult("Start date must be earlier than end date");
        }
    }
}