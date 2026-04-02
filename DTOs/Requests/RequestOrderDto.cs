using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.DTOs.Requests;

public record RequestOrderDto
{
    [Required]
    public Guid CustomerId { get; set; }
    [Required]
    public string DeliveryAddress { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Today;
    [Required]
    public OrderStatus Status { get; set; }
}