namespace SmtManager.Core.Entities;

public class Order : BaseEntity
{
    public required string Name { get; set; }
    public required string OrderNumber { get; set; }
    public required string Description { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = "Pending";
    public ICollection<OrderBoard> OrderBoards { get; set; } = new List<OrderBoard>();
}
