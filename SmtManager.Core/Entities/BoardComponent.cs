namespace SmtManager.Core.Entities;

public class BoardComponent
{
    public int BoardId { get; set; }
    public required Board Board { get; set; }
    public int ComponentId { get; set; }
    public required Component Component { get; set; }
    public int PlacementCount { get; set; }
}
