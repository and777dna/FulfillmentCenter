using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Entities.Operation.Interfaces;

namespace FulfillmentCenter.DTOs.Requests;

public record RequestOrderItemDto
{
    public IOperation<OrderItem> Operation { get; set; }
    [Required]
    public Guid ProductId { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public decimal PricePerUnit { get; set; }
}