using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Entities;

public class Shipment
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;
    [Required]
    public Guid DistributionCenterId { get; set; }
    public DistributionCenter DistributionCenter { get; set; } = null!;
    [Required]
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;
    [Required]
    public DateTime? ShippedAt { get; set; }
    [Required]
    public DateTime? EstimatedDelivery { get; set; }
    [Required]
    public bool IsDeleted { get; set; } = false;
}