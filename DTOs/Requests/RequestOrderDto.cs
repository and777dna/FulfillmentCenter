using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.DTOs.Requests;

public record RequestOrderDto
{
    [Required]
    public Guid CustomerId { get; set; }
    [Required]
    public string DeliveryAddress { get; set; }
    [Required]
    public OrderStatus Status { get; set; }
}