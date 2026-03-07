using FulfillmentCenter.Enums;

namespace FulfillmentCenter.DTOs.Responses;

public class ResponseOrderDto
{
    public string CustomerName { get; set; }
    public string DeliveryAddress { get; set; }
    public DateTime CreatedAt { get; set; }
    public OrderStatus Status { get; set; }
}