using System.ComponentModel.DataAnnotations;

namespace BMS.API.Models;

public class RegisterModel
{
    [StringLength(100)]
    public string FirstName { get; set; }
    [StringLength(100)]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string UserName { get; set; }
    public string Country { get; set; }
    [Required]
    public string Password { get; set; }
}
