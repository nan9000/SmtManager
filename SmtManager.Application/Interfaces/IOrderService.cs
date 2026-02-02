using SmtManager.Application.DTOs;
using SmtManager.Core.Entities;

namespace SmtManager.Application.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int id);
    Task<Order> CreateOrderAsync(CreateOrderDto orderDto);
    Task UpdateOrderAsync(int id, string orderNumber, string description, string status);
    Task DeleteOrderAsync(int id);
    Task<(byte[] FileContent, string FileName)?> GetOrderDownloadAsync(int id);
}
