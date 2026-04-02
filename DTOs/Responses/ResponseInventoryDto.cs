using System.ComponentModel.DataAnnotations;

namespace FulfillmentCenter.DTOs.Responses;

public class ResponseInventoryDto
{
    [Required]
    public Guid ProductId { get; set; }
    [Required]
    public int Quantity { get; set; }
}