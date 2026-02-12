namespace SmtManager.Application.DTOs;

public class ComponentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class CreateComponentDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int Quantity { get; set; }
}

public class UpdateComponentDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int Quantity { get; set; }
}
