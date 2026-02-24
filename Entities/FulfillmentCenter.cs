namespace FulfillmentCenter.Entities;

public class FulfillmentCenter
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public ICollection<Inventory> Inventory { get; set; } = new List<Inventory>();
    public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}