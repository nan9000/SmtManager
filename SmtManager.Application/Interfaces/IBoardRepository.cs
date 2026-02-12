using SmtManager.Core.Entities;

namespace SmtManager.Application.Interfaces;

public interface IBoardRepository : IGenericRepository<Board>
{
    Task<IEnumerable<Board>> GetAllBoardsWithDetailsAsync();
    Task<Board?> GetBoardWithDetailsAsync(int id);
}
