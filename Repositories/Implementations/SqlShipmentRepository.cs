using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlShipmentRepository : IShipmentRepository
{
    public List<Shipment> Shipments;
    private bool _isCached;
    private FulfillmentCenDbContext _context;
    
    public SqlShipmentRepository(FulfillmentCenDbContext context)
    {
        _context = context;
        Shipments = Read().Result;
        _isCached = true;
    }

    public async void Create(Shipment shipment)
    {
        await _context.Shipments.AddAsync(shipment);
        await _context.SaveChangesAsync();
    }

    public async void Delete(Guid id)//TODO: to specify id more precisely
    {
        var shipmentToDelete = await _context.Shipments.FirstOrDefaultAsync(shipment => shipment.Id == id);
        _context.Shipments.Remove(shipmentToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Shipment>> Read()
    {
        if (!_isCached)
        {
            Shipments = await _context.Shipments.ToListAsync();
        }

        return Shipments;
    }
    
    public void UpdateShipmentStatus(Guid id, ShipmentStatus status)
    {
        UpdateShipment(id, status, (shipmentStatus, shipment) => shipment.Status = shipmentStatus);
    }
    
    public async void UpdateShipment<TUpdateParameter>(Guid id, TUpdateParameter updateParameter, Action<TUpdateParameter, Shipment> up)//TUpdateParameter
    {
        var shipmentToUpdate = await _context.Shipments.FirstOrDefaultAsync(shipment => shipment.Id == id);
        up(updateParameter, shipmentToUpdate);
        await _context.SaveChangesAsync();
    }
}