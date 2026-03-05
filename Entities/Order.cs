using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Entities;

public class Order
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty; // TODO review: No CustomerId. Customer should be an entity with its own ID and relationships to Orders
    public string DeliveryAddress { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } //TODO review: CreatedAt is not auto-set. There's no default value like DateTime.UtcNow. A caller must remember to set it manually
    public OrderStatus Status { get; set; } = OrderStatus.Created;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public Shipment? Shipment { get; set; }
}