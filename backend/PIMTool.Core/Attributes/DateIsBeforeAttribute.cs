using System;
using System.ComponentModel.DataAnnotations;
using PIMTool.Core.Helpers;

namespace PIMTool.Core.Attributes;

public class DateIsBeforeAttribute : ValidationAttribute
{
    private string DateToCompare { get; set; }
    private string ErrorMessage { get; set; }
    public DateIsBeforeAttribute(string dateToCompare, string errorMessage)
    {
        DateToCompare = dateToCompare;
        ErrorMessage = errorMessage;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var validationFailMessage = ErrorMessage.IsNotNullNorEmpty() ? ErrorMessage : "Date validation failed";
        var dateStr = validationContext.ObjectType.GetProperty(DateToCompare)?.GetValue(validationContext.ObjectInstance, null);
        if (dateStr is null || dateStr.ToString().IsNullOrEmpty())
        {
            return ValidationResult.Success;
        }
        var date = (DateTime)dateStr;
        if (value is null || date == default)
        {
            return ValidationResult.Success;
        }
        var contextDate = DateTime.Parse(value.ToString());
        
        return (contextDate < date) ? ValidationResult.Success : new ValidationResult(validationFailMessage);
    }
}