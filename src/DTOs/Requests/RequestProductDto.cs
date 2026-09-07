using System.ComponentModel.DataAnnotations;

namespace FulfillmentCenter.DTOs.Requests;

public record RequestProductDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string SKU { get; set; }
    [Required]
    public decimal Weight { get; set; }
}