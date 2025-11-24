using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ECommerce.API.Data;
using ECommerce.API.DTOs;

namespace ECommerce.API.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly IUserService _userService;

    public OrderService(ApplicationDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersAsync(string userId, bool isAdmin)
    {
        var query = _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ThenInclude(p => p.Category).AsQueryable();
        // Admin ise tüm siparişleri göster, normal kullanıcı sadece kendi siparişlerini görsün
        if (!isAdmin) query = query.Where(o => o.UserId == userId);

        return await query.OrderByDescending(o => o.OrderDate).Select(o => new OrderDto
        {
            Id = o.Id, OrderDate = o.OrderDate, TotalAmount = o.TotalAmount, Status = o.Status,
            OrderItems = o.OrderItems.Select(oi => new OrderItemDto
            {
                Id = oi.Id, Quantity = oi.Quantity, Price = oi.Price,
                Product = new ProductDto
                {
                    Id = oi.Product.Id, Name = oi.Product.Name, Price = oi.Product.Price,
                    Stock = oi.Product.Stock, CategoryId = oi.Product.CategoryId, CategoryName = oi.Product.Category.Name
                }
            }).ToList()
        }).ToListAsync();
    }

    public async Task<OrderDto?> GetOrderByIdAsync(int id, string userId, bool isAdmin)
    {
        var query = _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ThenInclude(p => p.Category).AsQueryable();
        if (!isAdmin) query = query.Where(o => o.UserId == userId);

        var order = await query.FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return null;

        return new OrderDto
        {
            Id = order.Id, OrderDate = order.OrderDate, TotalAmount = order.TotalAmount, Status = order.Status,
            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
            {
                Id = oi.Id, Quantity = oi.Quantity, Price = oi.Price,
                Product = new ProductDto
                {
                    Id = oi.Product.Id, Name = oi.Product.Name, Price = oi.Product.Price,
                    Stock = oi.Product.Stock, CategoryId = oi.Product.CategoryId, CategoryName = oi.Product.Category.Name
                }
            }).ToList()
        };
    }

    public async Task<OrderDto?> CreateOrderAsync(string userId, CreateOrderDto dto)
    {
        var cartItems = await _context.CartItems.Include(c => c.Product).Where(c => c.UserId == userId).ToListAsync();
        if (!cartItems.Any()) return null;

        var order = new Models.Order { UserId = userId, OrderDate = DateTime.UtcNow, Status = "Hazırlanıyor" };
        decimal totalAmount = 0;

        foreach (var cartItem in cartItems)
        {
            if (cartItem.Product.Stock < cartItem.Quantity) return null;
            var orderItem = new Models.OrderItem { ProductId = cartItem.ProductId, Quantity = cartItem.Quantity, Price = cartItem.Product.Price };
            order.OrderItems.Add(orderItem);
            totalAmount += orderItem.Price * orderItem.Quantity;
            cartItem.Product.Stock -= cartItem.Quantity;
        }

        order.TotalAmount = totalAmount;
        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();

        return new OrderDto { Id = order.Id, OrderDate = order.OrderDate, TotalAmount = order.TotalAmount, Status = order.Status };
    }

    public async Task<bool> UpdateOrderStatusAsync(ClaimsPrincipal user, int orderId, string status)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return false;
        var appUser = await _userService.GetUserByIdAsync(userId);
        if (appUser == null || !appUser.IsAdmin) return false;

        var order = await _context.Orders.FindAsync(orderId);
        if (order == null) return false;

        order.Status = status;
        await _context.SaveChangesAsync();
        return true;
    }
}
