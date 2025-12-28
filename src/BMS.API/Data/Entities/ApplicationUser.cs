using Microsoft.AspNetCore.Identity;

namespace BMS.API.Data.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Country { get; set; }
}
