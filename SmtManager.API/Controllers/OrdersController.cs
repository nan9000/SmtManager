using Microsoft.AspNetCore.Mvc;
using SmtManager.Application.DTOs;
using SmtManager.Application.Interfaces;
using SmtManager.Core.Entities;

namespace SmtManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrderById(int id)
    {
        if (id <= 0) return BadRequest(new { message = "Invalid order ID" });

        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null) return NotFound(new { message = "Order not found" });

        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<Order>> Create([FromBody] CreateOrderDto orderDto)
    {
        var createdOrder = await _orderService.CreateOrderAsync(orderDto);
        return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadForProduction(int id)
    {
        if (id <= 0) return BadRequest(new { message = "Invalid order ID" });

        var result = await _orderService.GetOrderDownloadAsync(id);
        if (result == null) return NotFound(new { message = "Order not found" });

        return File(result.Value.FileContent, "application/json", result.Value.FileName);
    }
}