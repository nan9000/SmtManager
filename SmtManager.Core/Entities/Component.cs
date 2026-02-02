namespace SmtManager.Core.Entities;

public class Component : BaseEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int Quantity { get; set; }
    public ICollection<BoardComponent> BoardComponents { get; set; } = new List<BoardComponent>();
}
