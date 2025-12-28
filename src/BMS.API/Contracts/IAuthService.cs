using BMS.API.Models;

namespace BMS.API.Contracts;

public interface IAuthService
{
    Task<AuthModel> RegisterAsync(RegisterModel model);
    Task<AuthModel> LoginAsync(LoginModel model);
}
