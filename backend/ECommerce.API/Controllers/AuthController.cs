using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using ECommerce.API.Data;
using ECommerce.API.DTOs;
using ECommerce.API.Models;
using ECommerce.API.Services;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(ApplicationDbContext context, JwtTokenService jwtTokenService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Email kontrolü
        if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            return BadRequest(new { message = "Bu email zaten kullanılıyor" });

        // Username kontrolü
        if (await _context.Users.AnyAsync(u => u.Username == model.Email))
            return BadRequest(new { message = "Bu kullanıcı adı zaten kullanılıyor" });

        // Şifreyi hash'le
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

        var user = new ApplicationUser
        {
            Username = model.Email,
            Email = model.Email,
            PasswordHash = passwordHash
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = _jwtTokenService.GenerateToken(user);

        return Ok(new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            UserId = user.Id,
            IsAdmin = user.IsAdmin
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (user == null)
            return Unauthorized(new { message = "Geçersiz email veya şifre" });

        // Şifre kontrolü
        if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            return Unauthorized(new { message = "Geçersiz email veya şifre" });

        var token = _jwtTokenService.GenerateToken(user);

        return Ok(new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            UserId = user.Id,
            IsAdmin = user.IsAdmin
        });
    }
}
