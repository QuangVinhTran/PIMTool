using System;
using System.ComponentModel.DataAnnotations;

namespace PIMTool.Core.Attributes;

public class RequiredDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return (DateTime)value != null && (DateTime)value != default;
    }
}