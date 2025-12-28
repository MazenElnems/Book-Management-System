using BMS.API.Constants;
using BMS.API.Contracts;
using BMS.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [Authorize(Roles = ApplicationRoles.Admin)]
    [HttpPut("{id}/roles")]
    public async Task<IActionResult> AddToRole(Guid id, [FromBody] AddToRoleModel model)
    {
        var result = await _usersService.AddToRoleAsync(id, model);

        if (!string.IsNullOrEmpty(result))
            return BadRequest(new
            {
                message = result
            });

        return Ok(model);
    }
}
