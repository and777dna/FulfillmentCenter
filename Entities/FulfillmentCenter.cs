namespace FulfillmentCenter.Entities;

//TODO review: This is the most severe and consistent problem across all entities. Primary keys are Guid, but all foreign key properties are int.
//EF Core will fail to build the model or produce incorrect schema because the FK types don't match the PK types. All FK fields should be Guid

//TODO review: The class and the root namespace share the same name, which causes ambiguity. It should be renamed to FulfillmentCenterEntity or FulfillmentCenterModel or something similar
public class FulfillmentCenter
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public ICollection<Inventory> Inventory { get; set; } = new List<Inventory>(); //TODO review: should be Inventories
    public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}