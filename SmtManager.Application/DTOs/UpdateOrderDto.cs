namespace SmtManager.Application.DTOs;

public class UpdateOrderDto
{
    public string OrderNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
}
