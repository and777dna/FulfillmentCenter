using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.Entities;

namespace FulfillmentCenter.DTOs.Requests;

public record RequestInventoryDto
{
    [Required]
    public Guid ProductId { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public Guid DistributionCenterId { get; set; }
    //[Required]
    //public DistributionCenter DistributionCenter { get; set; }
}