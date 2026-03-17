using FulfillmentCenter.Enums;

namespace FulfillmentCenter.DTOs.Requests;

public record RequestOrderDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
    public string DeliveryAddress { get; set; }
    public DateTime CreatedAt { get; } = DateTime.Now; 
    public OrderStatus Status { get; set; }
}