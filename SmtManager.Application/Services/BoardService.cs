using SmtManager.Application.DTOs;
using SmtManager.Application.Interfaces;
using SmtManager.Core.Entities;

namespace SmtManager.Application.Services;

public class BoardService : IBoardService
{
    private readonly IBoardRepository _repository;

    public BoardService(IBoardRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BoardDto>> GetAllBoardsAsync()
    {
        var boards = await _repository.GetAllBoardsWithDetailsAsync();
        return boards.Select(MapToDto);
    }

    public async Task<BoardDto?> GetBoardByIdAsync(int id)
    {
        var board = await _repository.GetBoardWithDetailsAsync(id);
        if (board == null) return null;
        return MapToDto(board);
    }

    public async Task<BoardDto> CreateBoardAsync(CreateBoardDto createBoardDto)
    {
        var board = new Board
        {
            Name = createBoardDto.Name,
            Description = createBoardDto.Description,
            Length = createBoardDto.Length,
            Width = createBoardDto.Width
        };

        if (createBoardDto.BoardComponents != null)
        {
            foreach (var bcDto in createBoardDto.BoardComponents)
            {
                board.BoardComponents.Add(new BoardComponent
                {
                    ComponentId = bcDto.ComponentId,
                    PlacementCount = bcDto.PlacementCount,
                    Board = null!,
                    Component = null!
                });
            }
        }

        await _repository.AddAsync(board);

        return MapToDto(board);
    }

    public async Task UpdateBoardAsync(int id, UpdateBoardDto updateBoardDto)
    {
        var board = await _repository.GetBoardWithDetailsAsync(id);
        if (board == null) throw new KeyNotFoundException($"Board with ID {id} not found");

        board.Name = updateBoardDto.Name;
        board.Description = updateBoardDto.Description;
        board.Length = updateBoardDto.Length;
        board.Width = updateBoardDto.Width;

        board.BoardComponents.Clear();
        if (updateBoardDto.BoardComponents != null)
        {
             foreach (var bcDto in updateBoardDto.BoardComponents)
            {
                board.BoardComponents.Add(new BoardComponent
                {
                    BoardId = board.Id,
                    ComponentId = bcDto.ComponentId,
                    PlacementCount = bcDto.PlacementCount,
                    Board = null!,
                    Component = null!
                });
            }
        }

        await _repository.UpdateAsync(board);
    }

    public async Task DeleteBoardAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static BoardDto MapToDto(Board board)
    {
        return new BoardDto
        {
            Id = board.Id,
            Name = board.Name,
            Description = board.Description,
            Length = board.Length,
            Width = board.Width,
            BoardComponents = board.BoardComponents.Select(bc => new BoardComponentDto
            {
                ComponentId = bc.ComponentId,
                PlacementCount = bc.PlacementCount
            }).ToList()
        };
    }
}
