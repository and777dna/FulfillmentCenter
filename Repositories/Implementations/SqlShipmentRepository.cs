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

    public async Task Create(Shipment shipment)
    {
        try
        {
            await _context.Shipment.AddAsync(shipment);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Delete(Guid id)//TODO: to specify id more precisely
    {
        var shipmentToDelete = await _context.Shipment.FirstOrDefaultAsync(shipment => shipment.Id == id);
        if(shipmentToDelete != null){shipmentToDelete.IsDeleted = true;}
        
        //_context.Shipment.Remove(shipmentToDelete);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Shipment>> Read()
    {
        if (!_isCached)
        {
            Shipments = await _context.Shipment.ToListAsync();
        }

        return Shipments;
    }
    
    public async Task UpdateShipmentStatus(Guid id, ShipmentStatus status)
    {
        if(status == ShipmentStatus.Cancelled)
        {
            await Delete(id);
            await UpdateShipment(id, status, (shipmentStatus, shipment) => shipment.Status = shipmentStatus);
        }else if (status == ShipmentStatus.Failed) {
            //TODO: to fill this one
        }
        else
        {
            await UpdateShipment(id, status, (shipmentStatus, shipment) => shipment.Status = shipmentStatus);
        }
        
    }
    
    /*Failed = 4,
       Cancelled = 5*/
    public async Task UpdateShipment<TUpdateParameter>(Guid id, TUpdateParameter updateParameter, Action<TUpdateParameter, Shipment> up)
    {
        try
        {
            var shipmentToUpdate = await _context.Shipment.FirstOrDefaultAsync(shipment => shipment.Id == id);
            up(updateParameter, shipmentToUpdate);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}