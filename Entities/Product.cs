using System.ComponentModel.DataAnnotations;

namespace FulfillmentCenter.Entities;

public class Product : BaseEntity
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string SKU { get; set; } = string.Empty;
    [Required]
    public decimal Weight { get; set; }
    public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}