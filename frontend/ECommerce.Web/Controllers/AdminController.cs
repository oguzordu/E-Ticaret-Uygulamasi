using Microsoft.AspNetCore.Mvc;
using ECommerce.Web.Services;
using ECommerce.Web.Models;

namespace ECommerce.Web.Controllers;

public class AdminController : Controller
{
    private readonly ApiService _apiService;

    public AdminController(ApiService apiService)
    {
        _apiService = apiService;
    }

    private bool IsAdmin()
    {
        return HttpContext.Session.GetString("IsAdmin") == "True";
    }

    public async Task<IActionResult> Products()
    {
        if (!IsAdmin())
            return Forbid();

        var products = await _apiService.GetProductsAsync() ?? new List<Product>();
        return View(products);
    }

    public async Task<IActionResult> Categories()
    {
        if (!IsAdmin())
            return Forbid();

        var categories = await _apiService.GetCategoriesAsync() ?? new List<Category>();
        return View(categories);
    }

    public async Task<IActionResult> Orders()
    {
        if (!IsAdmin())
            return Forbid();

        var orders = await _apiService.GetOrdersAsync() ?? new List<Order>();
        return View(orders);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
    {
        if (!IsAdmin())
            return Forbid();

        var success = await _apiService.UpdateOrderStatusAsync(orderId, status);
        if (success)
        {
            TempData["Success"] = "Sipariş durumu güncellendi.";
        }
        else
        {
            TempData["Error"] = "Sipariş durumu güncellenemedi.";
        }
        
        return RedirectToAction("Orders");
    }
}
