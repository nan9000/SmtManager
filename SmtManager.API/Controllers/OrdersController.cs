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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderDto request)
    {
        try
        {
            await _orderService.UpdateOrderAsync(id, request.OrderNumber, request.Description, request.Status, request.OrderBoards);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Order not found" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _orderService.DeleteOrderAsync(id);
            return NoContent();
        }
        catch (Exception ex) // GenericRepository might throw InvalidOperationException
        {
            // Ideally check if it's a "Not Found" scenario or "Constraint" scenario
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadForProduction(int id)
    {
        if (id <= 0) return BadRequest(new { message = "Invalid order ID" });

        var result = await _orderService.GetOrderDownloadAsync(id);
        if (result == null) return NotFound(new { message = "Order not found" });

        return File(result.FileContent, "application/json", result.FileName);
    }
}