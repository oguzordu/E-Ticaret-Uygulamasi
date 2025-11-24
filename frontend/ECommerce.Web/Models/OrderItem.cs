namespace ECommerce.Web.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public Product Product { get; set; } = null!;
}

