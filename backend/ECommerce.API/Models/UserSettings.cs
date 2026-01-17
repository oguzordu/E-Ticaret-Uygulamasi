using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Models;

public class UserSettings
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    public bool DarkModeEnabled { get; set; }
}
