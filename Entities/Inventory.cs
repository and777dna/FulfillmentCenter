namespace FulfillmentCenter.Entities;

public class Inventory
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public Guid DistributionCenterId { get; set; }
    public DistributionCenter DistributionCenter { get; set; } = null!;//TODO: to underastand if this entity is fully related to this property?
    public int Quantity { get; set; }
}