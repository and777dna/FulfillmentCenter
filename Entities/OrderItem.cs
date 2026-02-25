namespace FulfillmentCenter.Entities;

public class OrderItem
{
    public Guid Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal PricePerUnit { get; set; }
}