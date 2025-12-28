using AutoMapper;
using BMS.API.Constants;
using BMS.API.Contracts;
using BMS.API.Data.Entities;
using BMS.API.Models;
using BMS.API.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BMS.API.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JWTOptions _jwt;
    private readonly IMapper _mapper;

    public AuthService(UserManager<ApplicationUser> userManager, IMapper mapper, IOptions<JWTOptions> jwt)
    {
        _userManager = userManager;
        _mapper = mapper;
        _jwt = jwt.Value;
    }

    public async Task<AuthModel> LoginAsync(LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if(user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return new AuthModel { Message = "Invalid email or password" };
        
        var roles = await _userManager.GetRolesAsync(user);
        var (accessToken, accessTokenExpiration) = await GenerateAccessTokenAsync(user, roles.ToList());

        return new AuthModel
        {
            Token = accessToken,
            ExpiresOn = accessTokenExpiration,
            IsAuthenticated = true,
            Email = user.Email!,
            UserName = user.UserName!,
            Roles = roles.ToList(),
        };
    }

    public async Task<AuthModel> RegisterAsync(RegisterModel model)
    {
        if(await _userManager.FindByEmailAsync(model.Email) is not null)
            return new AuthModel { Message = "Email is already exists", Errors = ["Email is already exists"] };
        
        if(await _userManager.FindByNameAsync(model.UserName) is not null)
            return new AuthModel { Message = "UserName is already taken" , Errors = ["UserName is already taken"] };

        var user = _mapper.Map<ApplicationUser>(model);

        var result = await _userManager.CreateAsync(user, model.Password);

        if(!result.Succeeded)
        {
            return new AuthModel
            {
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        await _userManager.AddToRoleAsync(user, ApplicationRoles.User);

        var roles = await _userManager.GetRolesAsync(user);
        var (accessToken, accessTokenExpiration) = await GenerateAccessTokenAsync(user, roles.ToList());

        return new AuthModel
        {
            Token = accessToken,
            ExpiresOn = accessTokenExpiration,
            IsAuthenticated = true,
            Email = user.Email!,
            UserName = user.UserName!,
            Roles = roles.ToList(),
        };
    }

    private async Task<(string, DateTime)> GenerateAccessTokenAsync(ApplicationUser user, List<string> roles) 
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));

        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Country, user.Country),
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            audience: _jwt.Audience,
            issuer: _jwt.Issuer,
            signingCredentials: signingCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return (tokenString, jwtSecurityToken.ValidTo);
    }
}
