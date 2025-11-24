using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.DTOs;
using ECommerce.API.Services;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        => Ok(await _categoryService.GetCategoriesAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        return category == null ? NotFound() : Ok(category);
    }

    [HttpPost, Authorize]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryDto dto)
    {
        var category = await _categoryService.CreateCategoryAsync(User, dto);
        return category == null ? Forbid() : CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    [HttpPut("{id}"), Authorize]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto dto)
        => await _categoryService.UpdateCategoryAsync(User, id, dto) ? NoContent() : NotFound();

    [HttpDelete("{id}"), Authorize]
    public async Task<IActionResult> DeleteCategory(int id)
        => await _categoryService.DeleteCategoryAsync(User, id) ? NoContent() : NotFound();
}
