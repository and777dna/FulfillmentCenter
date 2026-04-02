using System.ComponentModel.DataAnnotations;

namespace FulfillmentCenter.DTOs.Requests;

public record RequestOrderItemDto
{
    [Required]
    public Guid OrderId { get; set; }
    [Required]
    public Guid ProductId { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public decimal PricePerUnit { get; set; }
}