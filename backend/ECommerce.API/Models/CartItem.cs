namespace ECommerce.API.Models;

public class CartItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    
    // Foreign keys
    public string UserId { get; set; } = string.Empty;
    public int ProductId { get; set; }
    
    // Navigation properties
    public ApplicationUser User { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
