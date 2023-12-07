using System;
using System.ComponentModel.DataAnnotations;
using PIMTool.Core.Helpers;

namespace PIMTool.Core.Attributes;

public class RequiredGuidAttribute : ValidationAttribute
{
    private string ErrorMessage { get; set; }

    public RequiredGuidAttribute(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    public RequiredGuidAttribute()
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null || (Guid)value == default)
        {
            return new ValidationResult(ErrorMessage.IsNotNullNorEmpty() ? ErrorMessage : "Key is required");
        }
        return ValidationResult.Success;
    }
}