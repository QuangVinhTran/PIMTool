using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using PIMTool.Core.Helpers;

namespace PIMTool.Core.Attributes;

public class AcceptFileExtensionsAttribute : ValidationAttribute
{
    private string? _extensions;

    public string Extensions
    {
        get => _extensions.IsNullOrEmpty() ? "" : _extensions;
        set => _extensions = value;
    }

    public AcceptFileExtensionsAttribute(string extensions)
    {
        Extensions = extensions;
        ErrorMessage = "File extension is not supported";
    }

    public AcceptFileExtensionsAttribute(string extensions, string errorMessage) : this(extensions)
    {
        ErrorMessage = errorMessage;
    }

    private bool ValidateExtension(string filename)
    {
        return Extensions.Split(",").Contains(Path.GetExtension(filename).ToLowerInvariant());
    }
    
    public override bool IsValid(object? value) =>
        value == null || value is IFormFile valueAsFormFile && ValidateExtension(valueAsFormFile.FileName);
}