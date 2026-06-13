using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Entities;

public class Order : BaseEntity
{
    [Required]
    public Guid CustomerId { get; set; } // TODO review: No CustomerId. Customer should be an entity with its own ID and relationships to Orders
    [Required]
    [MaxLength(200)]
    public string DeliveryAddress { get; set; } = string.Empty;
    [Required] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Required] public OrderStatus Status { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public Shipment? Shipment { get; set; }
    public Customer? Customer { get; set; }
}