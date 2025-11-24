using ECommerce.API.Models;

namespace ECommerce.API.Services;

public interface IUserService
{
    Task<ApplicationUser?> GetUserByIdAsync(string userId);
}

