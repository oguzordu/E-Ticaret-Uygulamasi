using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.DTOs;
using ECommerce.API.Services;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]"), Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService) => _cartService = cartService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCart()
        => Ok(await _cartService.GetCartItemsAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty));

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
        => await _cartService.AddToCartAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty, dto.ProductId, dto.Quantity)
            ? Ok(new { message = "Item added to cart" })
            : BadRequest(new { message = "Sepete eklenirken bir hata oluştu" });

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCartItem(int id, [FromBody] UpdateCartItemDto dto)
        => await _cartService.UpdateCartItemAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty, id, dto.Quantity)
            ? Ok(new { message = "Cart item updated" })
            : BadRequest(new { message = "Sepet öğesi güncellenirken bir hata oluştu" });

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveFromCart(int id)
        => await _cartService.RemoveFromCartAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty, id)
            ? Ok(new { message = "Item removed from cart" })
            : NotFound();
}
