using SmtManager.Application.DTOs;

namespace SmtManager.Application.Interfaces;

public interface IBoardService
{
    Task<IEnumerable<BoardDto>> GetAllBoardsAsync();
    Task<BoardDto?> GetBoardByIdAsync(int id);
    Task<BoardDto> CreateBoardAsync(CreateBoardDto createBoardDto);
    Task UpdateBoardAsync(int id, UpdateBoardDto updateBoardDto);
    Task DeleteBoardAsync(int id);
}
