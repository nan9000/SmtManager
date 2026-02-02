using System.ComponentModel.DataAnnotations;

namespace SmtManager.Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Order number is required")]
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression(@"^[A-Z0-9\-]+$")]
        public required string OrderNumber { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 5)]
        public string? Description { get; set; }

        public DateTime OrderDate { get; set; }

        [Range(0, 10000)]
        public int BoardCount { get; set; }

        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }

    public enum OrderStatus
    {
        Pending = 0,
        Production = 1,
        Completed = 2
    }
}