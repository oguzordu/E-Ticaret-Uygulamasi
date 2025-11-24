namespace ECommerce.Web.Models;

public class CartItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public Product Product { get; set; } = null!;
}

