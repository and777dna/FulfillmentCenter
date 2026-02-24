using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Entities;

public class Shipment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int DistributionCenterId { get; set; }
    public FulfillmentCenter DistributionCenter { get; set; } = null!;
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;
    public DateTime? ShippedAt { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
}