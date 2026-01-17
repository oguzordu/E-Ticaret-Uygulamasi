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

    public async Task<IActionResult> Index()
    {
        if (!IsAdmin())
            return Forbid();

        var products = await _apiService.GetProductsAsync() ?? new List<Product>();
        var orders = await _apiService.GetOrdersAsync() ?? new List<Order>();
        var categories = await _apiService.GetCategoriesAsync() ?? new List<Category>();

        var model = new AdminDashboardViewModel
        {
            TotalProducts = products.Count,
            TotalOrders = orders.Count,
            TotalCategories = categories.Count,
            TotalRevenue = orders.Sum(o => o.TotalAmount),
            PendingOrders = orders.Count(o => o.Status == "Hazırlanıyor" || o.Status == "Kargoda")
        };

        return View(model);
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
    [HttpPost]
    public async Task<IActionResult> UpdateStock(int id, int stock)
    {
        if (!IsAdmin())
            return Forbid();

        var success = await _apiService.UpdateProductStockAsync(id, stock);
        if (success)
        {
            TempData["Success"] = "Stok güncellendi.";
        }
        else
        {
            TempData["Error"] = "Stok güncellenemedi.";
        }
        
        return RedirectToAction("Products");
    }

    [HttpGet]
    public IActionResult AddCategory()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            TempData["Error"] = "Kategori adı boş olamaz.";
            return View();
        }

        var success = await _apiService.CreateCategoryAsync(name);
        if (success)
        {
            TempData["Success"] = "Kategori başarıyla eklendi.";
            return RedirectToAction("Categories"); // Or Categories
        }
        
        TempData["Error"] = "Kategori eklenirken bir hata oluştu.";
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> EditCategory(int id)
    {
        // For simplicity, we can fetch all categories and find the specific one, 
        // or add a GetCategoryAsync method to ApiService. 
        // Given typically small list, getting all is fine for now, or use GetCategoryAsync if available.
        // Wait, backend has GetCategory(id). Let's use that if we can, but ApiService doesn't have it exposed.
        // Let's quickly add GetCategoryAsync if needed, or just iterate list for now as list is small.
        // Actually, let's just stick to fetching all and filtering, or since we don't have GetCategoryAsync in ApiService yet,
        // let's assume we can fetch all.
        
        var categories = await _apiService.GetCategoriesAsync();
        var category = categories?.FirstOrDefault(c => c.Id == id);
        
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> EditCategory(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
        {
            TempData["Error"] = "Kategori adı boş olamaz.";
            return View(category);
        }

        var success = await _apiService.UpdateCategoryAsync(category.Id, category.Name);
        if (success)
        {
            TempData["Success"] = "Kategori başarıyla güncellendi.";
            return RedirectToAction("Categories");
        }

        TempData["Error"] = "Kategori güncellenirken bir hata oluştu.";
        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        if (!IsAdmin()) return Forbid();

        var success = await _apiService.DeleteCategoryAsync(id);
        if (success)
        {
            TempData["Success"] = "Kategori silindi.";
        }
        else
        {
            TempData["Error"] = "Kategori silinemedi. Bu kategoriye bağlı ürünler olabilir.";
        }
        return RedirectToAction("Categories");
    }

    [HttpGet]
    public async Task<IActionResult> EditProduct(int id)
    {
        var product = await _apiService.GetProductAsync(id);
        if (product == null) return NotFound();

        var categories = await _apiService.GetCategoriesAsync() ?? new List<Category>();
        ViewBag.Categories = categories;

        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> EditProduct(Product product)
    {
        var success = await _apiService.UpdateProductAsync(product.Id, product);
        if (success)
        {
            TempData["Success"] = "Ürün başarıyla güncellendi.";
            return RedirectToAction("Products");
        }

        var categories = await _apiService.GetCategoriesAsync() ?? new List<Category>();
        ViewBag.Categories = categories;
        TempData["Error"] = "Ürün güncellenirken bir hata oluştu.";
        return View(product);
    }
}
