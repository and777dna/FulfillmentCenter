using FulfillmentCenter.Enums;

namespace FulfillmentCenter.DTOs.Requests;

public class RequestOrderDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
    public string DeliveryAddress { get; set; }
    public DateTime CreatedAt { get; set; }
    public OrderStatus Status { get; set; }
}