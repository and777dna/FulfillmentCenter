using System.ComponentModel.DataAnnotations;

namespace FulfillmentCenter.Entities;

public class Customer
{
    [Required]
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = String.Empty;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}