using ECommerce.API.DTOs;

namespace ECommerce.API.Services;

public interface ICartService
{
    Task<IEnumerable<CartItemDto>> GetCartItemsAsync(string userId);
    Task<bool> AddToCartAsync(string userId, int productId, int quantity);
    Task<bool> UpdateCartItemAsync(string userId, int cartItemId, int quantity);
    Task<bool> RemoveFromCartAsync(string userId, int cartItemId);
}

