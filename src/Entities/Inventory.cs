using System.ComponentModel.DataAnnotations;

namespace FulfillmentCenter.Entities;

public class Inventory : BaseEntity
{
    [Required]
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    [Required]
    public Guid DistributionCenterId { get; set; }
    public DistributionCenter DistributionCenter { get; set; } = null!;
    [Required]
    public int Quantity { get; set; }

    public void Increase(int amount)
    {
        if (amount <= 0) throw new InvalidOperationException("Amount must be positive");
        Quantity += amount;
    }

    public void Decrease(int amount)
    {
        if (amount <= 0) throw new InvalidOperationException("Amount must be positive");
        if (Quantity - amount < 0) throw new InvalidOperationException("Insufficient available stock");
        Quantity -= amount;
    }
}