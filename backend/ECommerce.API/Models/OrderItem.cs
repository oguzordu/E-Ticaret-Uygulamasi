namespace ECommerce.API.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; } // Sipariş anındaki fiyat
    
    // Foreign keys
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    
    // Navigation properties
    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
}

