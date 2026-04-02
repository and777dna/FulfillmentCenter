using System.ComponentModel.DataAnnotations;

namespace FulfillmentCenter.DTOs.Responses;

public class ResponseProductDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string SKU { get; set; } = string.Empty;
    [Required]
    public decimal Weight { get; set; }
}