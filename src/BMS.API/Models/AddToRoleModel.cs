using System.ComponentModel.DataAnnotations;

namespace BMS.API.Models;

public class AddToRoleModel
{
    [Required]
    [StringLength(200)]
    public string Role { get; set; }
}
