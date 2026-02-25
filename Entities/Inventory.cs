namespace FulfillmentCenter.Entities;

public class Inventory
{
    public Guid Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int DistributionCenterId { get; set; }
    public FulfillmentCenter DistributionCenter { get; set; } = null!;
    public int Quantity { get; set; }
}