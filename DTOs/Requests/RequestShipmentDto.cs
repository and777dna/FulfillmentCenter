using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.DTOs.Requests;

public class RequestShipmentDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid OrderId { get; set; }
    [Required]
    public Guid DistributionCenterId { get; set; }
    [Required]
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;
    [Required]
    public DateTime? ShippedAt { get; set; }
    [Required]
    public DateTime? EstimatedDelivery { get; set; }
}