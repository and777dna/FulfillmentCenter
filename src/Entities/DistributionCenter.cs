using System.ComponentModel.DataAnnotations.Schema;

namespace FulfillmentCenter.Entities;

public class DistributionCenter : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}