using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ECommerce.Web.Models;

namespace ECommerce.Web.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string ApiBaseUrl = "https://localhost:7125/api";

    public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _httpClient.BaseAddress = new Uri(ApiBaseUrl);
    }

    private void SetAuthHeader()
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("Token");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    // Auth
    public async Task<AuthResponse?> RegisterAsync(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Auth/register", new
        {
            Email = email,
            Password = password
        });
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }
        return null;
    }

    public async Task<AuthResponse?> LoginAsync(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Auth/login", new
        {
            Email = email,
            Password = password
        });
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }
        return null;
    }

    // Products
    public async Task<List<Product>?> GetProductsAsync(int? categoryId = null, string? search = null)
    {
        SetAuthHeader();
        var url = $"{ApiBaseUrl}/Products?";
        if (categoryId.HasValue) url += $"categoryId={categoryId}&";
        if (!string.IsNullOrEmpty(search)) url += $"search={Uri.EscapeDataString(search)}&";
        
        return await _httpClient.GetFromJsonAsync<List<Product>>(url.TrimEnd('&', '?'));
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        SetAuthHeader();
        try
        {
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/Products/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Product>();
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
    public async Task<bool> UpdateProductStockAsync(int id, int stock)
    {
        SetAuthHeader();
        var response = await _httpClient.PatchAsJsonAsync($"{ApiBaseUrl}/Products/{id}/stock", new { stock = stock });
        return response.IsSuccessStatusCode;
    }

    // Categories
    public async Task<List<Category>?> GetCategoriesAsync()
    {
        SetAuthHeader();
        return await _httpClient.GetFromJsonAsync<List<Category>>($"{ApiBaseUrl}/Categories");
    }

    // Cart
    public async Task<List<CartItem>?> GetCartAsync()
    {
        SetAuthHeader();
        return await _httpClient.GetFromJsonAsync<List<CartItem>>($"{ApiBaseUrl}/Cart");
    }

    public async Task<bool> AddToCartAsync(int productId, int quantity)
    {
        SetAuthHeader();
        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Cart/add", new { productId, quantity });
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCartItemAsync(int id, int quantity)
    {
        SetAuthHeader();
        var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/Cart/{id}", new { quantity });
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveFromCartAsync(int id)
    {
        SetAuthHeader();
        var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/Cart/{id}");
        return response.IsSuccessStatusCode;
    }

    // Orders
    public async Task<List<Order>?> GetOrdersAsync()
    {
        SetAuthHeader();
        return await _httpClient.GetFromJsonAsync<List<Order>>($"{ApiBaseUrl}/Orders");
    }

    public async Task<Order?> CreateOrderAsync()
    {
        SetAuthHeader();
        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Orders", new { });
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Order>();
        }
        return null;
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
    {
        SetAuthHeader();
        var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/Orders/{orderId}/status", new
        {
            Status = status
        });
        return response.IsSuccessStatusCode;
    }

    // Favorites
    public async Task<List<Favorite>?> GetFavoritesAsync()
    {
        SetAuthHeader();
        try {
            return await _httpClient.GetFromJsonAsync<List<Favorite>>($"{ApiBaseUrl}/Favorites");
        } catch { return new List<Favorite>(); }
    }

    public async Task<bool> ToggleFavoriteAsync(int productId)
    {
        SetAuthHeader();
        var response = await _httpClient.PostAsync($"{ApiBaseUrl}/Favorites/toggle/{productId}", null);
        return response.IsSuccessStatusCode;
    }

    // Settings
    public async Task<UserSettings?> GetSettingsAsync()
    {
        SetAuthHeader();
        try {
            return await _httpClient.GetFromJsonAsync<UserSettings>($"{ApiBaseUrl}/Settings");
        } catch { return null; }
    }

    public async Task<bool> UpdateThemeAsync(bool darkModeEnabled)
    {
        SetAuthHeader();
        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Settings/theme", darkModeEnabled);
        return response.IsSuccessStatusCode;
    }

    // Admin
    public async Task<bool> CreateCategoryAsync(string name)
    {
        SetAuthHeader();
        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Categories", new { Name = name });
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCategoryAsync(int id, string name)
    {
        SetAuthHeader();
        var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/Categories/{id}", new { Id = id, Name = name });
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        SetAuthHeader();
        var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/Categories/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateProductAsync(int id, Product product)
    {
        SetAuthHeader();
        var dto = new 
        { 
            Id = id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
            CategoryId = product.CategoryId
        };
        var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/Products/{id}", dto);
        return response.IsSuccessStatusCode;
    }
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
}
