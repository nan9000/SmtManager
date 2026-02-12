namespace SmtManager.Application.DTOs;

public class CreateOrderDto
{
    public required string OrderNumber { get; set; }
    public required string Description { get; set; }
    public List<OrderBoardDto> OrderBoards { get; set; } = new();
}
