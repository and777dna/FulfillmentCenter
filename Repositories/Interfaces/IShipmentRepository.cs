using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IShipmentRepository
{
    public void Create(Shipment shipment);
    public void Delete(Guid id);
    public List<Shipment> Read();
    //public void UpdateShipment(Guid id);
    public void UpdateShipment<TUpdateParameter>(Guid id, TUpdateParameter updateParameter,
        Action<TUpdateParameter, Shipment> up);
}