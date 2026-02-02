using SmtManager.Core.Entities;

namespace SmtManager.Application.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<Order?> GetOrderWithDetailsAsync(int id);
}
