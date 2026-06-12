using System.ComponentModel.DataAnnotations;

namespace FulfillmentCenter.DTOs.Responses;

public record ResponseInventoryDto
{
    [Required]
    public Guid ProductId { get; set; }
    [Required]
    public int Quantity { get; set; }
}