using Microsoft.EntityFrameworkCore;
using ECommerce.API.Data;
using ECommerce.API.DTOs;

namespace ECommerce.API.Services;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CartItemDto>> GetCartItemsAsync(string userId)
    {
        return await _context.CartItems
            .Include(c => c.Product)
            .ThenInclude(p => p.Category)
            .Where(c => c.UserId == userId)
            .Select(c => new CartItemDto
            {
                Id = c.Id,
                Quantity = c.Quantity,
                Product = new ProductDto
                {
                    Id = c.Product.Id,
                    Name = c.Product.Name,
                    Price = c.Product.Price,
                    Stock = c.Product.Stock,
                    CategoryId = c.Product.CategoryId,
                    CategoryName = c.Product.Category.Name
                }
            })
            .ToListAsync();
    }

    public async Task<bool> AddToCartAsync(string userId, int productId, int quantity)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null || product.Stock < quantity)
            return false;

        var existingCartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity += quantity;
            if (existingCartItem.Quantity > product.Stock)
                return false;
        }
        else
        {
            var cartItem = new Models.CartItem
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            };
            _context.CartItems.Add(cartItem);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateCartItemAsync(string userId, int cartItemId, int quantity)
    {
        var cartItem = await _context.CartItems
            .Include(c => c.Product)
            .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);

        if (cartItem == null || cartItem.Product.Stock < quantity)
            return false;

        cartItem.Quantity = quantity;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveFromCartAsync(string userId, int cartItemId)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);

        if (cartItem == null)
            return false;

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
        return true;
    }
}
