using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.DTOs.Requests;

public record RequestOrderDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string CustomerName { get; set; }
    [Required]
    public string DeliveryAddress { get; set; }
    [Required]
    public DateTime CreatedAt { get; } = DateTime.Now; 
    [Required]
    public OrderStatus Status { get; set; }
}