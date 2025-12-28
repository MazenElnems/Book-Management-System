using BMS.API.Models;

namespace BMS.API.Contracts;

public interface IUsersService
{
    Task<string> AddToRoleAsync(Guid userId, AddToRoleModel model);
}
