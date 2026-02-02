namespace SmtManager.Core.Entities;

public class OrderBoard
{
    public int OrderId { get; set; }
    public required Order Order { get; set; }
    public int BoardId { get; set; }
    public required Board Board { get; set; }
    public int QuantityRequired { get; set; }
}
