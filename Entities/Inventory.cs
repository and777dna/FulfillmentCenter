namespace FulfillmentCenter.Entities;

public class Inventory
{
    public Guid Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int DistributionCenterId { get; set; }
    public FulfillmentCenter FulfillmentCenter { get; set; } = null!;//TODO: to underastand if this entity is fully related to this property?
    public int Quantity { get; set; }
}