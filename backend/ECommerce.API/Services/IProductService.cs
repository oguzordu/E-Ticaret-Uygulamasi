using System.Security.Claims;
using ECommerce.API.DTOs;

namespace ECommerce.API.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProductsAsync(int? categoryId, string? search, decimal? minPrice, decimal? maxPrice);
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ProductDto?> CreateProductAsync(ClaimsPrincipal user, ProductDto productDto);
    Task<bool> UpdateProductAsync(ClaimsPrincipal user, int id, ProductDto productDto);
    Task<bool> DeleteProductAsync(ClaimsPrincipal user, int id);
}
