using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Entities;

public class Shipment : BaseEntity
{
    [Required]
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;
    [Required]
    public Guid DistributionCenterId { get; set; }
    public DistributionCenter DistributionCenter { get; set; } = null!;
    [Required]
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;
    [Required]
    public DateTime ShippedAt { get; set; }
    [Required]
    public DateTime? EstimatedDelivery { get; set; }
}