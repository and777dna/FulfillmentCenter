using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.DTOs.Requests;

public class RequestShipmentDto
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public Guid DistributionCenterId { get; set; }
    public DistributionCenter DistributionCenter { get; set; } = null!;
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;
    public DateTime? ShippedAt { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
}