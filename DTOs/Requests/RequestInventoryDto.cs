using FulfillmentCenter.Entities;

namespace FulfillmentCenter.DTOs.Requests;

public record RequestInventoryDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public Product Product { get; set; }
    public DistributionCenter DistributionCenter { get; set; }
}