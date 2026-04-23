using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlShipmentRepository : IShipmentRepository
{
    private readonly FulfillmentCenDbContext _context;
    int page = 2;
    int pageSize = 50;
    
    public SqlShipmentRepository(FulfillmentCenDbContext context)
    {
        _context = context;
    }

    public async Task Create(Shipment shipment)
    {
        try
        {
            await _context.Shipments.AddAsync(shipment);
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
        var shipmentToDelete = await _context.Shipments.FirstOrDefaultAsync(shipment => shipment.Id == id);
        if(shipmentToDelete != null){shipmentToDelete.IsDeleted = true;}
        else
        {
            throw new ArgumentNullException(nameof(id), "no Shipment was found");
        }
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
        
            //Shipments = await _context.Shipment.ToListAsync();
            return await _context.Shipments.Where(shipment => shipment.IsDeleted != true && shipment.Status !=
                ShipmentStatus.Cancelled && shipment.Status != ShipmentStatus.Failed).Skip((page - 1) * pageSize)
                .Take(pageSize).OrderBy(p => p.Id).ToListAsync();
        
    }
    
    public async Task UpdateShipmentStatus(Guid id, ShipmentStatus status)
    {
        if(status == ShipmentStatus.Cancelled)
        {
            await UpdateShipment(id, status, (shipmentStatus, shipment) => shipment.Status = shipmentStatus);
            await Delete(id);
        }else if (status == ShipmentStatus.Failed) {
            //TODO: to fill this one
        }
        else if (status == ShipmentStatus.Delivered)
        {
            //await OrderStatus.Delivered TO IsDeleted = true
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
            var shipmentToUpdate = await _context.Shipments.FirstOrDefaultAsync(shipment => shipment.Id == id);
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