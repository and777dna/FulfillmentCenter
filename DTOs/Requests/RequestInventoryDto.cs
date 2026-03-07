using FulfillmentCenter.Entities;

namespace FulfillmentCenter.DTOs.Requests;

public class RequestInventoryDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public Product Product { get; set; }
    public Entities.FulfillmentCenter FulfillmentCenter { get; set; }
}