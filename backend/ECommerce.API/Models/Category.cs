namespace ECommerce.API.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    // Navigation properties
    public ICollection<Product> Products { get; set; } = new List<Product>();
}

