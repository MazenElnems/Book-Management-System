using BMS.API.Contracts;
using BMS.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var result = await _authService.LoginAsync(model);

        if (!result.IsAuthenticated)
            return BadRequest(new
            {
                result.IsAuthenticated,
                result.Message
            });

        return Ok(new
        {
            result.IsAuthenticated,
            result.Token,
            result.ExpiresOn,
            result.UserName,
            result.Email,
            result.Roles
        });
    }
        
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        var result = await _authService.RegisterAsync(model);

        if (!result.IsAuthenticated)
            return BadRequest(new
            {
                result.IsAuthenticated,
                result.Errors,
                result.Message
            });

        return Ok(new
        {
            result.IsAuthenticated,
            result.Token,
            result.ExpiresOn,
            result.UserName,
            result.Email,
            result.Roles
        });
    }
}
