using Microsoft.AspNetCore.Mvc;
using ECommerce.Web.Services;

namespace ECommerce.Web.Controllers;

public class AuthController : Controller
{
    private readonly ApiService _apiService;

    public AuthController(ApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var response = await _apiService.LoginAsync(email, password);
        if (response != null)
        {
            HttpContext.Session.SetString("Token", response.Token);
            HttpContext.Session.SetString("Email", response.Email);
            HttpContext.Session.SetString("UserId", response.UserId);
            HttpContext.Session.SetString("IsAdmin", response.IsAdmin.ToString());
            return RedirectToAction("Index", "Home");
        }
        
        ViewBag.Error = "Geçersiz email veya şifre";
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string email, string password)
    {
        var response = await _apiService.RegisterAsync(email, password);
        if (response != null)
        {
            HttpContext.Session.SetString("Token", response.Token);
            HttpContext.Session.SetString("Email", response.Email);
            HttpContext.Session.SetString("UserId", response.UserId);
            HttpContext.Session.SetString("IsAdmin", response.IsAdmin.ToString());
            return RedirectToAction("Index", "Home");
        }
        
        ViewBag.Error = "Kayıt başarısız";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
