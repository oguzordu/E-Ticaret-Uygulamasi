using System.Security.Claims;
using ECommerce.API.DTOs;

namespace ECommerce.API.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(int id);
    Task<CategoryDto?> CreateCategoryAsync(ClaimsPrincipal user, CategoryDto categoryDto);
    Task<bool> UpdateCategoryAsync(ClaimsPrincipal user, int id, CategoryDto categoryDto);
    Task<bool> DeleteCategoryAsync(ClaimsPrincipal user, int id);
}
