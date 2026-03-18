using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.DTOs.Responses;

public class ResponseOrderDto
{
    [Required]
    public string CustomerName { get; set; }
    [Required]
    public string DeliveryAddress { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
    [Required]
    public OrderStatus Status { get; set; }
}