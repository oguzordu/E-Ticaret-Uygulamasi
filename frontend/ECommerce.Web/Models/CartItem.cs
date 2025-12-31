namespace ECommerce.Web.Models;

public class CartItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? CategoryName { get; set; } // Optional if needed by view
    public Product? Product { get; set; } // Keep for backward compatibility if needed
}

