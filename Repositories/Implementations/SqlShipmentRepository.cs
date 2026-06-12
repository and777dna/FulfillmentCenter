using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlShipmentRepository(FulfillmentCenDbContext context, ILogger<SqlShipmentRepository> logger) : IShipmentRepository
{
    int page = 2;
    int pageSize = 50;

    public async Task CreateAsync(Shipment shipment)
    {
        try
        {
            await context.Shipments.AddAsync(shipment);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to create a shipment.");
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)//TODO: to specify id more precisely
    {
        var shipmentToDelete = await context.Shipments.FirstOrDefaultAsync(shipment => shipment.Id == id);
        if(shipmentToDelete != null){shipmentToDelete.IsDeleted = true;}
        else
        {
            throw new ArgumentNullException(nameof(id), "no Shipment was found");
        }
        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to delete a shipment.");
            throw;
        }
    }

    public async Task<List<Shipment>> ReadAsync()
    {
        
            //Shipments = await _context.Shipment.ToListAsync();
            return await context.Shipments.Where(shipment => shipment.IsDeleted != true && shipment.Status !=
                ShipmentStatus.Cancelled && shipment.Status != ShipmentStatus.Failed).Skip((page - 1) * pageSize)
                .Take(pageSize).OrderBy(p => p.Id).ToListAsync();
        
    }
    
    public async Task UpdateShipmentStatusAsync(Guid id, ShipmentStatus status)
    {
        if(status == ShipmentStatus.Cancelled)
        {
            await UpdateShipmentAsync(id, status, (shipmentStatus, shipment) => shipment.Status = shipmentStatus);
            await DeleteAsync(id);
        }else if (status == ShipmentStatus.Failed) {
            //TODO: to fill this one
        }
        else if (status == ShipmentStatus.Delivered)
        {
            //await OrderStatus.Delivered TO IsDeleted = true
        }
        else
        {
            await UpdateShipmentAsync(id, status, (shipmentStatus, shipment) => shipment.Status = shipmentStatus);
        }
        
    }
    
    /*Failed = 4,
       Cancelled = 5*/
    public async Task UpdateShipmentAsync<TUpdateParameter>(Guid id, TUpdateParameter updateParameter, Action<TUpdateParameter, Shipment> up)
    {
        try
        {
            var shipmentToUpdate = await context.Shipments.FirstOrDefaultAsync(shipment => shipment.Id == id);
            up(updateParameter, shipmentToUpdate);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to update a shipment.");
            throw;
        }
    }
}