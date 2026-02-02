using SmtManager.Application.DTOs;
using SmtManager.Core.Entities;

namespace SmtManager.Application.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int id);
    Task<Order> CreateOrderAsync(CreateOrderDto orderDto);
    Task<(byte[] FileContent, string FileName)?> GetOrderDownloadAsync(int id);
}
