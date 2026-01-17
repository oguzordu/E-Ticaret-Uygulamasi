using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerce.API.Data;
using ECommerce.API.DTOs;
using ECommerce.API.Models;

namespace ECommerce.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SettingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<UserSettingsDto>> GetSettings()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var settings = await _context.UserSettings.FirstOrDefaultAsync(s => s.UserId == userId);
        
        if (settings == null)
        {
            // Return defaults if no settings found
            return new UserSettingsDto { DarkModeEnabled = false };
        }

        return new UserSettingsDto 
        { 
            DarkModeEnabled = settings.DarkModeEnabled
        };
    }

    [HttpPost("theme")]
    public async Task<IActionResult> UpdateTheme([FromBody] bool darkModeEnabled)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var settings = await _context.UserSettings.FirstOrDefaultAsync(s => s.UserId == userId);
        
        if (settings == null)
        {
            settings = new UserSettings { UserId = userId, DarkModeEnabled = darkModeEnabled };
            _context.UserSettings.Add(settings);
        }
        else
        {
            settings.DarkModeEnabled = darkModeEnabled;
        }

        await _context.SaveChangesAsync();
        return Ok(settings.DarkModeEnabled);
    }
}
