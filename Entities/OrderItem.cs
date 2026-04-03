namespace FulfillmentCenter.Entities;

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal PricePerUnit { get; set; }
    //TODO review: it's better to have a computed TotalPrice. PricePerUnit * Quantity is commonly used
}