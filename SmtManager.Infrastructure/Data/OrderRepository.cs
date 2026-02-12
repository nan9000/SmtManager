using Microsoft.EntityFrameworkCore;
using SmtManager.Application.Interfaces;
using SmtManager.Core.Entities;

namespace SmtManager.Infrastructure.Data;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(SmtDbContext context) : base(context) { }

    public async Task<IEnumerable<Order>> GetAllOrdersWithDetailsAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderBoards)
                .ThenInclude(ob => ob.Board)
                    .ThenInclude(b => b.BoardComponents)
                        .ThenInclude(bc => bc.Component)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderWithDetailsAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.OrderBoards)
                .ThenInclude(ob => ob.Board)
                    .ThenInclude(b => b.BoardComponents)
                        .ThenInclude(bc => bc.Component)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}
