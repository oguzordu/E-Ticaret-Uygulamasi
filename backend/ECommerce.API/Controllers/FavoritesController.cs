using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerce.API.Data;
using ECommerce.API.DTOs;
using ECommerce.API.Models;

namespace ECommerce.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FavoritesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<FavoriteDto>>> GetFavorites()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var favorites = await _context.Favorites
            .Include(f => f.Product)
            .ThenInclude(p => p.Category)
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.AddedDate)
            .Select(f => new FavoriteDto
            {
                Id = f.Id,
                ProductId = f.ProductId,
                Product = new ProductDto
                {
                    Id = f.Product.Id,
                    Name = f.Product.Name,
                    Price = f.Product.Price,
                    Stock = f.Product.Stock,
                    CategoryId = f.Product.CategoryId,
                    CategoryName = f.Product.Category.Name
                }
            })
            .ToListAsync();

        return favorites;
    }

    [HttpPost("toggle/{productId}")]
    public async Task<IActionResult> ToggleFavorite(int productId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var existingFavorite = await _context.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

        if (existingFavorite != null)
        {
            _context.Favorites.Remove(existingFavorite);
            await _context.SaveChangesAsync();
            return Ok(new { ispFavorite = false });
        }

        var favorite = new Favorite
        {
            UserId = userId,
            ProductId = productId
        };

        _context.Favorites.Add(favorite);
        await _context.SaveChangesAsync();
        return Ok(new { isFavorite = true });
    }
}
