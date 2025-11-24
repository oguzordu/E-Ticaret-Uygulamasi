using Microsoft.AspNetCore.Mvc;
using ECommerce.Web.Services;
using ECommerce.Web.Models;

namespace ECommerce.Web.Controllers;

public class OrdersController : Controller
{
    private readonly ApiService _apiService;

    public OrdersController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }

        var orders = await _apiService.GetOrdersAsync() ?? new List<Order>();
        return View(orders);
    }

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }

        var order = await _apiService.CreateOrderAsync();
        if (order != null)
        {
            TempData["Success"] = "Siparişiniz başarıyla oluşturuldu!";
            return RedirectToAction("Index");
        }
        
        TempData["Error"] = "Sipariş oluşturulurken bir hata oluştu";
        return RedirectToAction("Index", "Cart");
    }
}
