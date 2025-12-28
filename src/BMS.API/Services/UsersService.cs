using BMS.API.Contracts;
using BMS.API.Data.Entities;
using BMS.API.Models;
using Microsoft.AspNetCore.Identity;

namespace BMS.API.Services;

public class UsersService : IUsersService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public UsersService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<string> AddToRoleAsync(Guid userId, AddToRoleModel model)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            return "Invalid user ID";

        if (!await _roleManager.RoleExistsAsync(model.Role))
            return "Role is invalid";

        if (await _userManager.IsInRoleAsync(user, model.Role))
            return "User already in role";

        var result = await _userManager.AddToRoleAsync(user, model.Role);

        if (!result.Succeeded)
            return "Something went wrong";

        return string.Empty;
    }
}
