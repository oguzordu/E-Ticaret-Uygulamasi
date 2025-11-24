using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ECommerce.API.Data;
using ECommerce.API.DTOs;

namespace ECommerce.API.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly IUserService _userService;

    public ProductService(ApplicationDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync(int? categoryId, string? search, decimal? minPrice, decimal? maxPrice)
    {
        var query = _context.Products.Include(p => p.Category).AsQueryable();
        if (categoryId.HasValue) query = query.Where(p => p.CategoryId == categoryId.Value);
        if (!string.IsNullOrEmpty(search)) query = query.Where(p => p.Name.Contains(search));
        if (minPrice.HasValue) query = query.Where(p => p.Price >= minPrice.Value);
        if (maxPrice.HasValue) query = query.Where(p => p.Price <= maxPrice.Value);

        return await query.Select(p => new ProductDto
        {
            Id = p.Id, Name = p.Name, Price = p.Price,
            Stock = p.Stock, CategoryId = p.CategoryId, CategoryName = p.Category.Name
        }).ToListAsync();
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return null;
        
        return new ProductDto
        {
            Id = product.Id, 
            Name = product.Name, 
            Price = product.Price,
            Stock = product.Stock, 
            CategoryId = product.CategoryId, 
            CategoryName = product.Category?.Name ?? "Kategori Yok"
        };
    }

    public async Task<ProductDto?> CreateProductAsync(ClaimsPrincipal user, ProductDto productDto)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return null;
        var appUser = await _userService.GetUserByIdAsync(userId);
        if (appUser == null || !appUser.IsAdmin) return null;

        var product = new Models.Product
        {
            Name = productDto.Name, Price = productDto.Price,
            Stock = productDto.Stock, CategoryId = productDto.CategoryId
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        productDto.Id = product.Id;
        return productDto;
    }

    public async Task<bool> UpdateProductAsync(ClaimsPrincipal user, int id, ProductDto productDto)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return false;
        var appUser = await _userService.GetUserByIdAsync(userId);
        if (appUser == null || !appUser.IsAdmin) return false;

        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        product.Name = productDto.Name; product.Price = productDto.Price;
        product.Stock = productDto.Stock; product.CategoryId = productDto.CategoryId;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteProductAsync(ClaimsPrincipal user, int id)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return false;
        var appUser = await _userService.GetUserByIdAsync(userId);
        if (appUser == null || !appUser.IsAdmin) return false;

        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}
