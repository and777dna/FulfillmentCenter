namespace FulfillmentCenter.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;  // артикул
    public decimal Weight { get; set; }
    public ICollection<Inventory> Inventory { get; set; } = new List<Inventory>();
}