using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.DTOs;
using ECommerce.API.Services;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]"), Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService) => _orderService = orderService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        var isAdmin = User.FindFirst("IsAdmin")?.Value == "True";
        return Ok(await _orderService.GetOrdersAsync(userId, isAdmin));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        var isAdmin = User.FindFirst("IsAdmin")?.Value == "True";
        var order = await _orderService.GetOrderByIdAsync(id, userId, isAdmin);
        return order == null ? NotFound() : Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto dto)
    {
        var order = await _orderService.CreateOrderAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty, dto);
        return order == null
            ? BadRequest(new { message = "Sipariş oluşturulurken bir hata oluştu" })
            : CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    [HttpPut("{id}/status"), Authorize]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto dto)
        => await _orderService.UpdateOrderStatusAsync(User, id, dto.Status)
            ? Ok(new { message = "Order status updated" })
            : NotFound();
}
