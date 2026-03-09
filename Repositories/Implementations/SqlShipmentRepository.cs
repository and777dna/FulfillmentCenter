using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Interfaces;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlShipmentRepository : IShipmentRepository
{
    public List<Shipment> Shipments;
    private bool _isCached;
    private FulfillmentCenDbContext _context;
    
    public SqlShipmentRepository()
    {
        _context = new FulfillmentCenDbContext();
        Shipments = Read();
        _isCached = true;
    }

    public void Create(Shipment shipment)
    {
        _context.Shipments.Add(shipment);
        _context.SaveChanges();
    }

    public void Delete(Guid id)//TODO: to specify id more precisely
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
    
    public void UpdateShipmentStatus(Guid id, ShipmentStatus status)
    {
        UpdateShipment(id, status, (shipmentStatus, shipment) => shipment.Status = shipmentStatus);
    }
    
    public void UpdateShipment<TUpdateParameter>(Guid id, TUpdateParameter updateParameter, Action<TUpdateParameter, Shipment> up)//TUpdateParameter
    {
        var shipmentToUpdate = _context.Shipments.FirstOrDefault(shipment => shipment.Id == id);
        up(updateParameter, shipmentToUpdate);
        _context.SaveChanges();
    }
}