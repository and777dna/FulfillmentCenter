using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Entities;

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Created;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public Shipment? Shipment { get; set; }
}