using Microsoft.AspNetCore.Mvc;
using ECommerce.Web.Services;
using ECommerce.Web.Models;

namespace ECommerce.Web.Controllers;

public class AccountController : Controller
{
    private readonly ApiService _apiService;

    public AccountController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Favorites()
    {
        var favorites = await _apiService.GetFavoritesAsync();
        return View(favorites);
    }
}
