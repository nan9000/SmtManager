using SmtManager.Application.DTOs;
using SmtManager.Core.Entities;

namespace SmtManager.Application.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int id);
    Task<Order> CreateOrderAsync(CreateOrderDto orderDto);
    Task UpdateOrderAsync(int id, string orderNumber, string description, string status, List<OrderBoardDto>? orderBoards = null);
    Task DeleteOrderAsync(int id);
    Task<FileContentResultDto?> GetOrderDownloadAsync(int id);
}

public class FileContentResultDto
{
    public byte[] FileContent { get; set; } = Array.Empty<byte>();
    public string ContentType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
}
