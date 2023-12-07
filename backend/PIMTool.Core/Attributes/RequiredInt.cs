using System.ComponentModel.DataAnnotations;

namespace PIMTool.Core.Attributes;

public class RequiredInt : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        return value == default ? new ValidationResult("Int value is required") : ValidationResult.Success;
    }
}