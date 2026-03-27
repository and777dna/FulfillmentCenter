using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Entities;

public class Order
{
    [Key]
    [Required]
    [MaxLength(36)]
    public Guid Id { get; set; }
    [Required]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty; // TODO review: No CustomerId. Customer should be an entity with its own ID and relationships to Orders
    [Required]
    [MaxLength(200)]
    public string DeliveryAddress { get; set; } = string.Empty;
    [Required]
    [MaxLength(200)]
    public DateTime CreatedAt { get; set; } 
    //public DateTime CreatedAt { get; set; } //TODO review: CreatedAt is not auto-set. There's no default value like DateTime.UtcNow. A caller must remember to set it manually
    [Required]
    [MaxLength(200)]
    public OrderStatus Status { get; set; } = OrderStatus.Created;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public Shipment? Shipment { get; set; }
}