using System.ComponentModel.DataAnnotations;

namespace FulfillmentCenter.Entities;

public class Inventory
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    [Required]
    public Guid DistributionCenterId { get; set; }
    public DistributionCenter DistributionCenter { get; set; } = null!;
    [Required]
    public int Quantity { get; set; }
}