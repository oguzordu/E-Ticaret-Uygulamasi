using Microsoft.AspNetCore.Mvc;
using ECommerce.Web.Services;
using ECommerce.Web.Models;

namespace ECommerce.Web.Controllers;

public class HomeController : Controller
{
    private readonly ApiService _apiService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ApiService apiService, ILogger<HomeController> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(int? categoryId, string? search)
    {
        var products = await _apiService.GetProductsAsync(categoryId, search) ?? new List<Product>();
        var categories = await _apiService.GetCategoriesAsync() ?? new List<Category>();
        
        ViewBag.Categories = categories;
        ViewBag.SelectedCategoryId = categoryId;
        ViewBag.SearchTerm = search;
        
        return View(products);
    }

    public async Task<IActionResult> ProductDetail(int id)
    {
        var product = await _apiService.GetProductAsync(id);
        if (product == null)
            return NotFound();
        
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleFavorite(int productId)
    {
        var success = await _apiService.ToggleFavoriteAsync(productId);
        // İsteğin geldiği fabrikayı (kaynağı) kontrol et, eğer ajax değilse geri yönlendir
        return Redirect(Request.Headers["Referer"].ToString());
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTheme([FromBody] bool darkModeEnabled)
    {
        await _apiService.UpdateThemeAsync(darkModeEnabled);
        return Ok();
    }
}
