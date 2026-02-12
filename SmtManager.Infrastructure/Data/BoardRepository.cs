using Microsoft.EntityFrameworkCore;
using SmtManager.Application.Interfaces;
using SmtManager.Core.Entities;

namespace SmtManager.Infrastructure.Data;

public class BoardRepository : GenericRepository<Board>, IBoardRepository
{
    public BoardRepository(SmtDbContext context) : base(context) { }

    public async Task<IEnumerable<Board>> GetAllBoardsWithDetailsAsync()
    {
        return await _context.Boards
            .Include(b => b.BoardComponents)
                .ThenInclude(bc => bc.Component)
            .ToListAsync();
    }

    public async Task<Board?> GetBoardWithDetailsAsync(int id)
    {
        return await _context.Boards
            .Include(b => b.BoardComponents)
                .ThenInclude(bc => bc.Component)
            .FirstOrDefaultAsync(b => b.Id == id);
    }
}
