namespace ECommerce.Web.Models;

public class AdminDashboardViewModel
{
    public int TotalProducts { get; set; }
    public int TotalOrders { get; set; }
    public int TotalCategories { get; set; }
    public decimal TotalRevenue { get; set; }
    public int PendingOrders { get; set; }
}
