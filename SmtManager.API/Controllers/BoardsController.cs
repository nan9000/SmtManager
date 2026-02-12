using Microsoft.AspNetCore.Mvc;
using SmtManager.Application.DTOs;
using SmtManager.Application.Interfaces;

namespace SmtManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly IBoardService _boardService;

    public BoardsController(IBoardService boardService)
    {
        _boardService = boardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var boards = await _boardService.GetAllBoardsAsync();
        return Ok(boards);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var board = await _boardService.GetBoardByIdAsync(id);
        if (board == null) return NotFound();
        return Ok(board);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBoardDto createBoardDto)
    {
        var board = await _boardService.CreateBoardAsync(createBoardDto);
        return CreatedAtAction(nameof(GetById), new { id = board.Id }, board);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateBoardDto updateBoardDto)
    {
        try
        {
            await _boardService.UpdateBoardAsync(id, updateBoardDto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _boardService.DeleteBoardAsync(id);
        return NoContent();
    }
}
