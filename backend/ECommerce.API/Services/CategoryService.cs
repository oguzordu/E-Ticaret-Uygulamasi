using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ECommerce.API.Data;
using ECommerce.API.DTOs;

namespace ECommerce.API.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;
    private readonly IUserService _userService;

    public CategoryService(ApplicationDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        => await _context.Categories.Select(c => new CategoryDto { Id = c.Id, Name = c.Name }).ToListAsync();

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        return category == null ? null : new CategoryDto { Id = category.Id, Name = category.Name };
    }

    public async Task<CategoryDto?> CreateCategoryAsync(ClaimsPrincipal user, CategoryDto categoryDto)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return null;
        var appUser = await _userService.GetUserByIdAsync(userId);
        if (appUser == null || !appUser.IsAdmin) return null;

        var category = new Models.Category { Name = categoryDto.Name };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        categoryDto.Id = category.Id;
        return categoryDto;
    }

    public async Task<bool> UpdateCategoryAsync(ClaimsPrincipal user, int id, CategoryDto categoryDto)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return false;
        var appUser = await _userService.GetUserByIdAsync(userId);
        if (appUser == null || !appUser.IsAdmin) return false;

        var category = await _context.Categories.FindAsync(id);
        if (category == null) return false;

        category.Name = categoryDto.Name;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCategoryAsync(ClaimsPrincipal user, int id)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return false;
        var appUser = await _userService.GetUserByIdAsync(userId);
        if (appUser == null || !appUser.IsAdmin) return false;

        var category = await _context.Categories.FindAsync(id);
        if (category == null) return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }
}
