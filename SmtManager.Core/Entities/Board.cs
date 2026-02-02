namespace SmtManager.Core.Entities;

public class Board : BaseEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public double Length { get; set; }
    public double Width { get; set; }
    public ICollection<OrderBoard> OrderBoards { get; set; } = new List<OrderBoard>();
    public ICollection<BoardComponent> BoardComponents { get; set; } = new List<BoardComponent>();
}
