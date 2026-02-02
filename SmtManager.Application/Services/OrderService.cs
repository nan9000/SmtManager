using System.Text;
using System.Text.Json;
using SmtManager.Application.DTOs;
using SmtManager.Application.Interfaces;
using SmtManager.Core.Entities;

namespace SmtManager.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Order> CreateOrderAsync(CreateOrderDto orderDto)
    {
        if (string.IsNullOrWhiteSpace(orderDto.OrderNumber))
            throw new ArgumentException("Order number is required");

        if (orderDto.OrderNumber.Length > 50)
            throw new ArgumentException("Order number too long");

        if (string.IsNullOrWhiteSpace(orderDto.Description))
            throw new ArgumentException("Description is required");

        if (orderDto.Description.Length > 500)
            throw new ArgumentException("Description too long");

        var order = new Order
        {
            OrderNumber = orderDto.OrderNumber,
            Description = orderDto.Description,
            OrderDate = DateTime.UtcNow
        };

        await _repository.AddAsync(order);

        return order;
    }

    public async Task UpdateOrderAsync(int id, string orderNumber, string description, string status)
    {
        var order = await _repository.GetByIdAsync(id);
        if (order == null) throw new KeyNotFoundException($"Order with ID {id} not found.");

        if (string.IsNullOrWhiteSpace(orderNumber)) throw new ArgumentException("Order number is required");
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required");

        order.OrderNumber = orderNumber;
        order.Description = description;
        order.Status = status;

        await _repository.UpdateAsync(order);
    }

    public async Task DeleteOrderAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<(byte[] FileContent, string FileName)?> GetOrderDownloadAsync(int id)
    {
        var order = await _repository.GetOrderWithDetailsAsync(id);
        if (order == null) return null;

        var json = SerializeOrderToJson(order);
        var bytes = Encoding.UTF8.GetBytes(json);
        var fileName = GenerateDownloadFileName(order.OrderNumber);

        return (bytes, fileName);
    }

    private string SerializeOrderToJson(Order order)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        return JsonSerializer.Serialize(order, options);
    }

    private string GenerateDownloadFileName(string orderNumber)
    {
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        return $"order_{orderNumber}_{timestamp}.json";
    }
}
