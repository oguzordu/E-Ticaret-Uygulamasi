using Microsoft.AspNetCore.Mvc;
using ECommerce.Web.Services;
using ECommerce.Web.Models;

namespace ECommerce.Web.Controllers;

public class CartController : Controller
{
    private readonly ApiService _apiService;

    public CartController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var cartItems = await _apiService.GetCartAsync() ?? new List<CartItem>();
        return View(cartItems);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }

        var success = await _apiService.AddToCartAsync(productId, quantity);
        if (success)
        {
            return RedirectToAction("Index");
        }
        
        TempData["Error"] = "Sepete eklenirken bir hata oluştu";
        return RedirectToAction("ProductDetail", "Home", new { id = productId });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int id, int quantity)
    {
        var success = await _apiService.UpdateCartItemAsync(id, quantity);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Remove(int id)
    {
        await _apiService.RemoveFromCartAsync(id);
        return RedirectToAction("Index");
    }
}

