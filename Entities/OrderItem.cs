using System.ComponentModel.DataAnnotations;

namespace FulfillmentCenter.Entities;

public class OrderItem
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;
    [Required]
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    [Required]
    public int Quantity { get; set; }
    [Required]
    public decimal PricePerUnit { get; set; }
    //TODO review: it's better to have a computed TotalPrice. PricePerUnit * Quantity is commonly used
}