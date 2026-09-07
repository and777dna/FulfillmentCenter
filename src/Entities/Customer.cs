namespace FulfillmentCenter.Entities;

public class Customer : BaseEntity
{
    public string CustomerName { get; set; } = string.Empty;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}