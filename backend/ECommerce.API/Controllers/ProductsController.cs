using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.DTOs;
using ECommerce.API.Services;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(
        [FromQuery] int? categoryId, [FromQuery] string? search,
        [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        => Ok(await _productService.GetProductsAsync(categoryId, search, minPrice, maxPrice));

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return product == null ? NotFound() : Ok(product);
    }

    [HttpPost, Authorize]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductDto dto)
    {
        var product = await _productService.CreateProductAsync(User, dto);
        return product == null ? Forbid() : CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}"), Authorize]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto dto)
        => await _productService.UpdateProductAsync(User, id, dto) ? NoContent() : NotFound();

    [HttpDelete("{id}"), Authorize]
    public async Task<IActionResult> DeleteProduct(int id)
        => await _productService.DeleteProductAsync(User, id) ? NoContent() : NotFound();
}
