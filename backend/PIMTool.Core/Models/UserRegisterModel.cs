using System;
using System.ComponentModel.DataAnnotations;

namespace PIMTool.Core.Models;

public class UserRegisterModel
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime? BirthDate { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Password { get; set; }
}