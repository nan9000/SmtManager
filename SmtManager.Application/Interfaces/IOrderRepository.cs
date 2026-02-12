using SmtManager.Core.Entities;

namespace SmtManager.Application.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IEnumerable<Order>> GetAllOrdersWithDetailsAsync();
    Task<Order?> GetOrderWithDetailsAsync(int id);
}
