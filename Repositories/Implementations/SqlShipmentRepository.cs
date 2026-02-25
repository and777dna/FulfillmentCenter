using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlShipmentRepository : IShipmentRepository
{
    public List<Shipment> Shipments;
    private bool _isCached;
    private FulfillmentCenDbContext _context;
    
    public SqlShipmentRepository()
    {
        Shipments = Read();
        _isCached = true;
        _context = new FulfillmentCenDbContext();
    }

    public void Create(Shipment shipment)
    {
        _context.Shipments.Add(shipment);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var shipmentToDelete = _context.Shipments.FirstOrDefault(shipment => shipment.Id == id);
        _context.Shipments.Remove(shipmentToDelete);
        _context.SaveChanges();
    }

    public List<Shipment> Read()
    {
        if (!_isCached)
        {
            Shipments = _context.Shipments.ToList();
        }

        return Shipments;
    }
    public void UpdateShipment(){}
}