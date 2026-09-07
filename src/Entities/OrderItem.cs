using System.ComponentModel.DataAnnotations;

namespace FulfillmentCenter.Entities;

public class OrderItem : BaseEntity
{
    [Required]
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    [Required]
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public decimal PricePerUnit { get; set; }
    //TODO review: it's better to have a computed TotalPrice. PricePerUnit * Quantity is commonly used

    
    public void Increase(int amount)
    {
        if (amount <= 0) throw new InvalidOperationException("Amount must be positive");
        Quantity += amount;
    }
    
    public void Decrease(int amount)
    {
        if (amount <= 0) throw new InvalidOperationException("Amount must be positive");
        if(Quantity - amount < 0) throw new InvalidOperationException("Insufficient available stock");
        Quantity -= amount;
    }
}