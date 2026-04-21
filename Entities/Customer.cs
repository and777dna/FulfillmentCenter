using System.ComponentModel.DataAnnotations;

namespace FulfillmentCenter.Entities;

public class Customer : BaseEntity
{
    [Required]
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}