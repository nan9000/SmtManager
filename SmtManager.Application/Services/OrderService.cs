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
        return await _repository.GetAllOrdersWithDetailsAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _repository.GetOrderWithDetailsAsync(id);
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
            Name = orderDto.OrderNumber,
            OrderNumber = orderDto.OrderNumber,
            Description = orderDto.Description,
            OrderDate = DateTime.UtcNow,
            Status = "Pending"
        };

        if (orderDto.OrderBoards != null && orderDto.OrderBoards.Any())
        {
            foreach (var obDto in orderDto.OrderBoards)
            {
                order.OrderBoards.Add(new OrderBoard
                {
                    BoardId = obDto.BoardId,
                    QuantityRequired = obDto.QuantityRequired,   
                    Board = null!, 
                    Order = null!
                });
            }
        }

        await _repository.AddAsync(order);

        return order;
    }


    public async Task UpdateOrderAsync(int id, string orderNumber, string description, string status, List<OrderBoardDto>? orderBoards = null)
    {
        var order = await _repository.GetOrderWithDetailsAsync(id);
        if (order == null) throw new KeyNotFoundException($"Order with ID {id} not found.");

        if (string.IsNullOrWhiteSpace(orderNumber)) throw new ArgumentException("Order number is required");
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required");

        order.OrderNumber = orderNumber;
        order.Description = description;
        order.Status = status;


        if (orderBoards != null)
        {
            // Clear existing boards
            order.OrderBoards.Clear();

            // Add new boards
            foreach (var obDto in orderBoards)
            {
                order.OrderBoards.Add(new OrderBoard
                {
                    BoardId = obDto.BoardId,
                    QuantityRequired = obDto.QuantityRequired,
                    Board = null!,
                    Order = order
                });
            }
        }

        await _repository.UpdateAsync(order);
    }

    public async Task DeleteOrderAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<FileContentResultDto?> GetOrderDownloadAsync(int id)
    {
        var order = await _repository.GetOrderWithDetailsAsync(id);
        if (order == null) return null;

        var json = JsonSerializer.Serialize(order, new JsonSerializerOptions 
        { 
            WriteIndented = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        });
        
        var bytes = Encoding.UTF8.GetBytes(json);
        
        return new FileContentResultDto
        {
            FileContent = bytes,
            ContentType = "application/json",
            FileName = $"order_{order.OrderNumber}_{DateTime.UtcNow:yyyyMMddHHmmss}.json"
        };
    }
}
