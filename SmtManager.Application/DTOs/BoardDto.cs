namespace SmtManager.Application.DTOs;

public class BoardDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Length { get; set; }
    public double Width { get; set; }
    public List<BoardComponentDto> BoardComponents { get; set; } = new();
}

public class CreateBoardDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public double Length { get; set; }
    public double Width { get; set; }
    public List<BoardComponentDto> BoardComponents { get; set; } = new();
}

public class UpdateBoardDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public double Length { get; set; }
    public double Width { get; set; }
    public List<BoardComponentDto> BoardComponents { get; set; } = new();
}
