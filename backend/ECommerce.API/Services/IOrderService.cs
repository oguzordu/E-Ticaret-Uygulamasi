using System.Security.Claims;
using ECommerce.API.DTOs;

namespace ECommerce.API.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetOrdersAsync(string userId, bool isAdmin);
    Task<OrderDto?> GetOrderByIdAsync(int id, string userId, bool isAdmin);
    Task<OrderDto?> CreateOrderAsync(string userId, CreateOrderDto dto);
    Task<bool> UpdateOrderStatusAsync(ClaimsPrincipal user, int orderId, string status);
}
